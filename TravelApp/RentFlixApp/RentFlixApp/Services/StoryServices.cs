using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RentFlixApp.Models;

namespace RentFlixApp.Services
{
    public class StoryServices
    {
        //GET STORY
        public async Task<Story> GetStoryAsync(string accessToken, int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/" + id);
            if (json is null)
                return null;
            var value = JsonConvert.DeserializeObject<Story>(json);
            return value;
        }

        //GET stories
        public async Task<RootStory> GetStoriesAsync(string accessToken/*, int pageIndex, int pageSize*/)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<RootStory>(json);
            //values.Stories.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return values;
        }

        //SEARCH stories
        public async Task<RootStory> SearchStoriesAsync(string accessToken, string keyword)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/search/{keyword}");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<RootStory>(json);
            return values;
        }

        //Most Apreciated stories
        public async Task<RootStory> MostAppreciatedStories(string accessToken, int skip, int take)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/topstories/{skip}/{take}");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<RootStory>(json);
            return values;
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
            var uploadServiceBaseAddress = Constants.BaseUrl + "api/story/gallery";
            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        //POST story, return id for uploading photos
        public async Task<string> PostStoryAsync(string accessToken, Story story)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var s = new Story()
            {
                BannerImage= story.BannerImage.Replace("\"", ""),
                HeadTitle = story.HeadTitle,
                HeadLines = story.HeadLines,
                Location = story.Location,
                Region = story.Region,
                Rating = story.Rating,
                UserProfileModelId = story.UserProfileModelId,
                StoryText = story.StoryText
            };
            var json = JsonConvert.SerializeObject(s);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/story", content);
            return await response.Content.ReadAsStringAsync();
        }

        //POST IMAGE WITH DESCRIPTION TO STORY
        public async Task<bool> AddPhotoToGallery(string accessToken, Gallery gallery)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var gal = new Gallery()
            {
                StoriesModelId = gallery.StoriesModelId,
                Description = gallery.Description,
                FullPath = gallery.FullPath
            };
            var json = JsonConvert.SerializeObject(gal);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/story/photogallery", content);
            return response.IsSuccessStatusCode;
        }

        //GET GALLERY WITH COMMENTS
        public async Task<List<Gallery>> GetStoryPhotoGallery(string accessToken, int storyId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/{storyId}/gallery");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<List<Gallery>>(json);
            return values;
        }

        //POST COMMENTS
        public async Task<bool> AddStoryComment(string accessToken, Comment comment)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var comm = new Comment()
            {
                StoriesModelId = comment.StoriesModelId,
                CommentText= comment.CommentText,
                UserProfileId = comment.UserProfileId
            };
            var json = JsonConvert.SerializeObject(comm);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/story/comments", content);
            return response.IsSuccessStatusCode;
        }
        //GET COMMENTS SECTION - RootComment
        public async Task<RootComment> GetStoryComments(string accessToken, int storyId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/{storyId}/comments");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<RootComment>(json);
            return values;
        }

        //POST LIKE
        public async Task<bool> AddStoryLike(string accessToken, StoryLike storyLike)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var like = new StoryLike()
            {
                StoryModelId = storyLike.StoryModelId,
                UserProfileId= storyLike.UserProfileId
            };
            var json = JsonConvert.SerializeObject(like);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/story/likes", content);
            return response.IsSuccessStatusCode;
        }

        //GET LIKE
        public async Task<StoryLike> GetStoryLike(string accessToken, int storyId, int userId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/{storyId}/likes/{userId}");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<StoryLike>(json);
            return values;
        }

        //UNLIKE LIKE
        public async Task<bool> DeleteStoryLikeAsync(string accessToken, int likeId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = await client.DeleteAsync(
                $"{Constants.BaseUrl}api/story/likes/{likeId}");

            return response.IsSuccessStatusCode;
        }

        //GET User Star
        public async Task<Stars> GetProfileStars(string accessToken, int userSent, int userReceived)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var json = await client.GetStringAsync($"{Constants.BaseUrl}api/story/stars/{userSent}/{userReceived}");
            if (json is null)
                return null;
            var values = JsonConvert.DeserializeObject<Stars>(json);
            return values;
        }
        //POST User Star
        public async Task<bool> AddStar(string accessToken, Stars stars)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var star = new Stars()
            {
                UserWhoReceived = stars.UserWhoReceived,
                UserWhoSent = stars.UserWhoSent
            };
            var json = JsonConvert.SerializeObject(star);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{Constants.BaseUrl}api/story/stars", content);
            return response.IsSuccessStatusCode;
        }

    }
}
