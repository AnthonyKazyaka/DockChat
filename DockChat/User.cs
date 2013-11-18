using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DockChat
{
    public class User
    {
        //public static List<> 
        
        private static User _currentUser = new User();
        public static User CurrentUser { get { return _currentUser; } set { _currentUser = value; } }

        private List<Group> _groups = new List<Group>(); 
        public List<Group> Groups { get{ return _groups; } set { _groups = value; } }

        public static async Task<List<Group>> GetGroups()
        {
            HttpWebRequest request =
                WebRequest.Create(GroupMeSettings.BaseGroupMeUrl + "/groups?token=" + GroupMeSettings.AccessToken) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            string responseString;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            JObject responseObject = JObject.Parse(responseString);
            JArray userGroupsArray = (JArray)responseObject["response"];
            return Group.GetGroupsFromJson(userGroupsArray);
        }

        public static async void AddGroupToUser(string groupName, string description)
        {
            HttpWebRequest request =
                WebRequest.Create(GroupMeSettings.BaseGroupMeUrl + "/groups?token=" + GroupMeSettings.AccessToken) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            Stream reqStream = await request.GetRequestStreamAsync();

            string requestString = "{\"name\": \"" + groupName + "\", \"description\": \"" + description + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(requestString);
            reqStream.Write(byteArray, 0, byteArray.Length);
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string responseString = reader.ReadToEnd();
            }
        }
    }
}
