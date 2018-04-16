using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RentFlixApp.Models;

namespace RentFlixApp.Services
{
    public class UserProfileService
    {
        //GET current logged in profile
        public async Task<UserProfile> GetProfileAsync(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/userprofile");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<UserProfile>(json);
            return values;
        }

        //GET all Profiles 
        public async Task<List<UserProfile>> GetAllProfilesAsync(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/userprofile/allProfiles");
            if (json is null) return null;
            var values = JsonConvert.DeserializeObject<List<UserProfile>>(json);
            return values;
        }

        //Get Specific profile 
        public async Task<UserProfilePostsAndRatings> GetSpecificProfile(string accessToken, int userId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/userprofile/{userId}");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<UserProfilePostsAndRatings>(json);
            return values;
        }

        //POST
        public async Task<bool> PostUserProfileAsync(UserProfile userProfile, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var userP = new UserProfile()
            {
                FullName = userProfile.FullName,
                AboutMe = userProfile.AboutMe,
                Avatar = userProfile.Avatar.Replace("\"",""),
                DateOfBirth = userProfile.DateOfBirth
            };
            var json = JsonConvert.SerializeObject(userP);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/userprofile", content);
            return response.IsSuccessStatusCode;
        }

        //POST image
        public async Task<string> UploadPhoto(string accessToken, MediaFile mediaFile)
        {
            var content = new MultipartFormDataContent
            {
                {new StreamContent(mediaFile.GetStream()), "\"file\"", $"\"{mediaFile.Path}\""}
            };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var uploadServiceBaseAddress = Constants.BaseUrl + "api/profile/Upload";
            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        //PUT profile
        public async Task<bool> UpdateProfileAsync(UserProfile profile, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var json = JsonConvert.SerializeObject(profile);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync(
                Constants.BaseUrl + "api/updateprofile", content);
            return response.IsSuccessStatusCode;
        }
    }
}
