using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.OnBehalfOf.AzureTokenProvider
{
	public static class AccessTokenProvider
	{
		public static AccessToken GetAccessToken(string resource, string userEmail, string userEmailPassword)
		{
			if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentNullException("resource");
			if (string.IsNullOrWhiteSpace(userEmail)) throw new ArgumentNullException("userEmail");
			if (string.IsNullOrWhiteSpace(userEmailPassword)) throw new ArgumentNullException("userEmailPassword");

			return AccessTokenProvider.GetAccessToken(AadConfig.DefaultTenantId.Value, 
				AadConfig.DefaultClientId.Value, 				
				resource, userEmail, userEmailPassword);
		}

		public static AccessToken GetAccessToken(string tenantId, string clientId, string resource, string userEmail, string userEmailPassword)
		{
			if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentNullException("resource");
			if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException("clientId");			
			if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentNullException("resource");
			if (string.IsNullOrWhiteSpace(userEmail)) throw new ArgumentNullException("userEmail");
			if (string.IsNullOrWhiteSpace(userEmailPassword)) throw new ArgumentNullException("userEmailPassword");

			var Me = Assembly.GetExecutingAssembly().GetName();
			var Dots = new string('.', 70);
			try
			{
				var authority = String.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", tenantId);

				Console.WriteLine(Dots);
				Console.WriteLine(" {0} : Now {1:dd-MMM-yyyy HH:mm:ss} UTC", Me.Name, DateTime.UtcNow);
				Console.WriteLine(Dots);
				Console.WriteLine();
				Console.WriteLine(" Conencting to :{0}", authority);

				//Get auth context
				AuthenticationContext authContext = new AuthenticationContext(authority, new TokenCache());
				AuthenticationResult authResult = null;

				Console.WriteLine(" Creating credential for user:{0}, ClientId: {1}", userEmail, clientId);

				UserCredential uc = new UserPasswordCredential(userEmail, userEmailPassword);

				Console.WriteLine(" Getting access token for:{0}", resource);

				// In the case of a transient error, retry once after 1 second, then abandon.
				// Retrying is optional.  It may be better, for your application, to return an error immediately to the user and have the user initiate the retry.
				bool retry = false;
				int retryCount = 0;				
				do
				{
					retry = false;
					try
					{
						authResult = authContext.AcquireTokenAsync(resource, clientId, uc).Result;
					}
					catch (AdalException ex)
					{
						Console.WriteLine("Authorization failed, Retrying....");

						if (ex.ErrorCode == "temporarily_unavailable")
						{
							// Transient error, OK to retry.
							retry = true;
							retryCount++;
							Thread.Sleep(1000);
						}
					}
				} while ((retry == true) && (retryCount < 2));


				Console.WriteLine(" Authorization done. Returning Token.");
				Console.WriteLine();
				return new AccessToken(tenantId, clientId,  userEmail,
					resource, authResult.AccessTokenType, authResult.AccessToken, authResult.ExpiresOn);
			}
			catch (Exception err)
			{
				var exception = new Exception(string.Format("{0}: One or more error occured while processing request.", Me.Name), err);
				throw exception;
			}

			
		}
	}
}
