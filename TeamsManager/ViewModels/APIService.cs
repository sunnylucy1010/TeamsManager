using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace TeamsManager.ViewModels
{
    public class APIService
    {
        
        ///Login service
        

        
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