using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Identity.Client;
using System.Text.Json;
using TeamsManager.ViewModels;
using TeamsManager.Models;

namespace TeamsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        //Set the API Endpoint to Graph 'me' endpoint
        string accessToken;
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";
        string graphJoinedTeams = "https://graph.microsoft.com/v1.0/me/joinedTeams";
        string graphTeamInfo = "https://graph.microsoft.com/v1.0/teams/";

        //Teams storage
        public List<Team> joinedTeams = new List<Team>();
        ////Set the scope for API call to user.read
        string[] scopes = new string[] { "user.read", "team.readbasic.all" };


        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Call AcquireToken - to acquire a token requiring user to sign-in
        /// </summary>
        private async void CallGraphButton_Click(object sender, RoutedEventArgs e)
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
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
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
                
        /// <summary>
        /// Sign out the current user
        /// </summary>
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = await App.PublicClientApp.GetAccountsAsync();

            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                    this.ResultText.Text = "User has signed-out";
                    //this.CallGraphButton.Visibility = Visibility.Visible;
                    //this.SignOutButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    ResultText.Text = $"Error signing-out user: {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Display basic information contained in the token
        /// </summary>
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult != null)
            {
                TokenInfoText.Text += $"Username: {authResult.Account.Username}" + Environment.NewLine;
                TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
            }
        }

        private async void DisplayTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = await APIService.GetHttpContentWithToken(graphJoinedTeams, accessToken);
            JoinedTeamDeser.Root? root = JsonSerializer.Deserialize<JoinedTeamDeser.Root>(jsonString);
            TeamList.Text = "";
            foreach (var item in root.value)
            {
                TeamList.Text += item.id + item.displayName + "\n";
            }
        }

        private async void CrawlDataButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = await APIService.GetHttpContentWithToken(graphJoinedTeams, accessToken);
            JoinedTeamDeser.Root? root = JsonSerializer.Deserialize<JoinedTeamDeser.Root>(jsonString);
            TeamList.Text = "";
            foreach (var item in root.value)
            {
                jsonString = await APIService.GetHttpContentWithToken(graphTeamInfo + item.id, accessToken);
                EachTeamDeser? value = JsonSerializer.Deserialize<EachTeamDeser>(jsonString);

                joinedTeams.Add(new Team()
                {
                    Id = item.id,
                    DisplayName = item.displayName
                });
                joinedTeams.Add(new Team()
                {
                    CreatedDateTime = (DateTime)Convert.ChangeType(value.createdDateTime, typeof(DateTime)),
                    WebUrl = value.webUrl,
                    InternalId = value.internalId,
                    Description = value.description,
                });


            }
        }
    }

}
