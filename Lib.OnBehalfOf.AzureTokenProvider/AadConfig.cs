using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.OnBehalfOf.AzureTokenProvider
{
	internal static class AadConfig
	{
		internal readonly static Lazy<string> DefaultTenantId;

		internal readonly static Lazy<string> DefaultClientId;

		static AadConfig()
		{
			AadConfig.DefaultTenantId = new Lazy<string>(new Func<string>(AadConfig.ReadTenanteId));
			AadConfig.DefaultClientId = new Lazy<string>(new Func<string>(AadConfig.ReadClientId));						
		}

		private static string GetAppSetting(string name, string defaultValue)
		{
			string item = ConfigurationManager.AppSettings[name];
			if (string.IsNullOrWhiteSpace(item))
			{
				return defaultValue;
			}
			return item.Trim();
		}

		private static string ReadClientId()
		{
			string appSetting = AadConfig.GetAppSetting(N.ClientId, null);
			if (appSetting == null)
			{
				appSetting = AadConfig.GetAppSetting(N.ApplicationId, null);
				if (appSetting == null)
				{
					throw new Exception(string.Concat(string.Format("AppSettings missing the entry '{0}'.", N.ApplicationId), Environment.NewLine, "Use 'Application ID' which is a Guid or 'App ID URI' available under Settings -> Properties."));
				}
			}
			return appSetting;
		}

		

		private static string ReadTenanteId()
		{
			string appSetting = AadConfig.GetAppSetting(N.TenantId, null);
			if (appSetting == null)
			{
				throw new Exception(string.Format("AppSettings missing the entry '{0}'. Exmple: mydomain.com or mydomain.onmicrosoft.com", N.TenantId));
			}
			return appSetting;
		}

		private static class N
		{
			public const string TenantId = "AAD.TenantId";

			public const string ClientId = "AAD.ClientId";

			public const string ApplicationId = "AAD.ApplicationId";		
			
		}
	}
}
