using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.OnBehalfOf.AzureTokenProvider
{
	public class AccessToken
	{		

		public string ApplicationId	{
			get;
		}

		public string UserEmail
		{
			get;
		}
		
		public string Resource
		{
			get;
		}

		public string TenantId
		{
			get;
		}

		public string Token
		{
			get;
		}

		public string TokenType
		{
			get;
		}

		public DateTimeOffset ExpiredOn
		{
			get;
		}

	

		
		internal AccessToken(string tenantId, string applicationId,  
			string userEmail,  string resource, string tokenType, string token, DateTimeOffset expiredOn)
		{
			if (tenantId == null) throw new ArgumentNullException("tenantId");
			if (applicationId == null) throw new ArgumentNullException("applicationId");			
			if (userEmail == null) throw new ArgumentNullException("userEmail");			
			if (resource == null) throw new ArgumentNullException("resource");
			if (tokenType == null) throw new ArgumentNullException("tokenType");
			if (token == null) throw new ArgumentNullException("token");

			this.TenantId = tenantId;
			this.ApplicationId = applicationId;			
			this.UserEmail = userEmail;			
			this.Resource = resource;
			this.TokenType = tokenType;
			this.Token = token;
			this.ExpiredOn = expiredOn;
		}
	}
}
