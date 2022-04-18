using System.Text.Json;
using TeamsManager.Library.Models;
using TeamsManager.Models;

namespace TeamsManager.Library
{
    //public class DataCrawler
    //{
    //    string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";
    //    string graphJoinedTeams = "https://graph.microsoft.com/v1.0/me/joinedTeams";
    //    string graphTeamInfo = "https://graph.microsoft.com/v1.0/teams/";
    //    public async void CrawlData(string accessToken, List<Team> joinedTeams)
    //    {
    //        string jsonString = await APIService.GetHttpContentWithToken(graphJoinedTeams, accessToken);
    //        JoinedTeamDeser.Root? root = JsonSerializer.Deserialize<JoinedTeamDeser.Root>(jsonString);
    //        foreach (var item in root.value)
    //        {
    //            jsonString = await APIService.GetHttpContentWithToken(graphTeamInfo + item.id, accessToken);
    //            EachTeamDeser? value = JsonSerializer.Deserialize<EachTeamDeser>(jsonString);

    //            joinedTeams.Add(new Team()
    //            {
    //                Id = item.id,
    //                DisplayName = item.displayName
    //            });
    //            joinedTeams.Add(new Team()
    //            {
    //                CreatedDateTime = (DateTime)Convert.ChangeType(value.createdDateTime, typeof(DateTime)),
    //                WebUrl = value.webUrl,
    //                InternalId = value.internalId,
    //                Description = value.description,
    //            });

    //            //// Teams created in 20212 sem
    //            //DateTime d1 = (DateTime)Convert.ChangeType(item.createdDateTime, typeof(DateTime));
    //            //DateTime d2 = (DateTime)Convert.ChangeType("2022-03-01T00:00:00.000Z", typeof(DateTime));

    //        }
    //    }

      
    //    public static void CrawData(string accessToken, List<Team> joinedTeams)
    //    {
    //        CrawData(accessToken, joinedTeams);
    //    }
    //}
}