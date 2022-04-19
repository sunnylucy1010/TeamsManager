using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using TeamsManager.Models;
using TeamsManager.ViewModels;

namespace TeamsManager.Views
{
    /// <summary>
    /// Interaction logic for TeamsListPage.xaml
    /// </summary>
    public partial class TeamsListPage : Page
    {
        public List<Team> joinedTeams = new List<Team>();
        string accessToken = LoginPage.accessToken;
        string graphJoinedTeams = "https://graph.microsoft.com/v1.0/me/joinedTeams";
        string graphTeamInfo = "https://graph.microsoft.com/v1.0/teams/";
        public TeamsListPage()
        {
            InitializeComponent();
            CrawlTeamsData(accessToken);
        }

        private void DisplayJoinedTeams_Click(object sender, RoutedEventArgs e)
        {
            TeamList.Text = "";
            foreach (var item in joinedTeams)
            {
                TeamList.Text += item.Id + "\n";
            }
        }

        private async void CrawlTeamsData(string accessToken)
        {
            string jsonString = await APIService.GetHttpContentWithToken(graphJoinedTeams, accessToken);
            JoinedTeamDeser.Root? root = JsonSerializer.Deserialize<JoinedTeamDeser.Root>(jsonString);
            foreach (var item in root.value)
            {
                jsonString = await APIService.GetHttpContentWithToken(graphTeamInfo + item.id, accessToken);
                EachTeamDeser? value = JsonSerializer.Deserialize<EachTeamDeser>(jsonString);

                joinedTeams.Add(new Team()
                {
                    Id = item.id,
                    DisplayName = item.displayName,
                    Description = value.description,
                    CreatedDateTime = (DateTime)Convert.ChangeType(value.createdDateTime, typeof(DateTime)),
                    WebUrl = value.webUrl,
                    InternalId = value.internalId
                });
                
            }
        }
    }


}
