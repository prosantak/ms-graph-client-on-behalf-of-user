This assembly get access token from azure AD on behalf of user.
It uses oAuth.

Setup
==============
1. Logon to portal.azure.com with your credential
2. Navigate to Azure Active Directory. 
3. Open App Registration and create a ne application.
4. Name application as "email-read-client", Type is Native and redirect URI should be 
   https://your-tenant/email-read-client
5. Allow read email permission for Microsoft Graph or whatever resource you want to use
6. Save it
7. Get Application Id and apply following setting on CONSUMER
	<add key="AAD.TenantId" value="<tenant-name>" />
    <add key="AAD.ClientId" value="<application-id>" />  
    <add key="AAD.Resource" value="https://graph.microsoft.com" />  
    <add key="AAD:UserEmail" value="<user-email-address>" />
    <add key="AAD:UserPassword" value="<email-password>" />