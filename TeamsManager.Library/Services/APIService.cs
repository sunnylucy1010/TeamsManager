using Microsoft.Identity.Client;

namespace TeamsManager.Library
{
    public class APIService
    {
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";
        string graphJoinedTeams = "https://graph.microsoft.com/v1.0/me/joinedTeams";
        string graphTeamInfo = "https://graph.microsoft.com/v1.0/teams/";

        ////Set the scope for API call
        string[] scopes = new string[] { "user.read", "team.readbasic.all" };
        public static string accessToken;
        ///Login service
        

        public static async Task<string> LoginGetToken(string url)
        {
            AuthenticationResult authResult = null;
            var app = App.PublicClientApp;
            ResultText.Text = string.Empty;
            TokenInfoText.Text = string.Empty;

            var accounts = await app.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    return $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                return $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
            }


            // get json response from server
            if (authResult != null)
            {
                accessToken = authResult.AccessToken;
                ResultText.Text = await APIService.GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
                DisplayBasicTokenInfo(authResult);
                //this.SignOutButton.Visibility = Visibility.Visible;   
                //ResultText.Text = await GetHttpContentWithToken(graphTeamInfo, authResult.AccessToken);

            }
        }

        ///HTTP GET
        public static async Task<string> GetHttpContentWithToken(string url, string accessToken)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}