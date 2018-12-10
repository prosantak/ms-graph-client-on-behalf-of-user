using Lib.OnBehalfOf.AzureTokenProvider;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GraphApiOBOConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{

			try
			{
				var UserEmail = ConfigurationManager.AppSettings["AAD:UserEmail"];
				var UserPassword = ConfigurationManager.AppSettings["AAD:UserPassword"];
				var resource = ConfigurationManager.AppSettings["AAD.Resource"];


				Console.WriteLine("{0} - Connecting as user:{1}", DateTime.Now, UserEmail);


				var accessToken = AccessTokenProvider.GetAccessToken(resource, UserEmail, UserPassword);

				//Calling Graph Client
				var delegateAuthProvider = new DelegateAuthenticationProvider((requestMessage) =>
				{
					requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken.Token);

					return Task.FromResult(0);
				});

				var graphClient = new GraphServiceClient(delegateAuthProvider);

				Console.WriteLine("Reading top 20 emails......");

				var mailResults = graphClient.Me.MailFolders.Inbox.Messages.Request()
								.OrderBy("receivedDateTime DESC")
								.Select(m => new { m.Subject, m.ReceivedDateTime, m.From })
								.Top(20)
								.GetAsync()
								.Result;


				foreach (var msg in mailResults.CurrentPage)
				{
					Console.WriteLine("Subject: {0}<br/>", msg.Subject);
				}

				Console.WriteLine();
				Console.WriteLine("Done. Press any key to continue....");
				
			}
			catch (Exception err)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error:{0}", err.Message);
				while ((err = err.InnerException )!= null)
				{
					Console.WriteLine(err.Message);
				}
			}

			Console.ReadKey();

		}
	}
}
