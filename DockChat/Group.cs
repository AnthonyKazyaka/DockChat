using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace DockChat
{
    public class Group
    {
        public enum GroupType
        {
            Private,
            Public
        }

        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public GroupType PrivacyType { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CreatorUserId { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
        public bool OfficeMode { get; set; }
        public string ShareUrl { get; set; }
        public List<Member> Members { get; set; }

        public List<Message> Messages { get; set; }

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
            return GetGroupsFromJson(userGroupsArray);
        }

        /// <summary>
        /// Returns a list of messages ordered by date of creation
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static async Task<List<Message>> GetGroupMessages(string groupId)
        {
            List<Message> messageList = new List<Message>();

            HttpWebRequest request =
                WebRequest.Create(GroupMeSettings.BaseGroupMeUrl + "/groups/" + groupId + "/messages?token=" + GroupMeSettings.AccessToken) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            string responseString;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            JObject responseObject = JObject.Parse(responseString);
            JArray messagesArray = (JArray)responseObject["response"]["messages"];

            messageList = GetMessagesFromJson(messagesArray);
            messageList.Reverse();

            return messageList;
        }

        private static List<Group> GetGroupsFromJson(JArray groupsArray)
        {
            List<Group> groupList = new List<Group>();
            foreach (JObject jsonObject in groupsArray)
            {
                List<Member> membersList = new List<Member>();
                JArray membersArray = (JArray)jsonObject["members"];
                foreach (JObject member in membersArray)
                {
                    Member newMember = new Member()
                    {
                        Id = (string)member["id"],
                        UserId = (string)member["user_id"],
                        Nickname = (string)member["nickname"],
                        Muted = (bool)member["muted"],
                        ImageUrl = (string)member["image_url"],
                        Autokicked = (bool)member["autokicked"]
                    };
                    membersList.Add(newMember);
                }
                Group newGroup = new Group()
                {
                    Id = (string)jsonObject["id"],
                    GroupId = (string)jsonObject["group_id"],
                    Name = (string)jsonObject["name"],
                    PhoneNumber = (string)jsonObject["phone_number"],
                    PrivacyType = ((string)jsonObject["type"] == "private") ? Group.GroupType.Private : Group.GroupType.Public,
                    Description = (string)jsonObject["description"],
                    ImageUrl = (string)jsonObject["image_url"],
                    CreatorUserId = (string)jsonObject["creator_user_id"],
                    CreatedAt = (long)jsonObject["created_at"],
                    UpdatedAt = (long)jsonObject["updated_at"],
                    OfficeMode = (bool)jsonObject["office_mode"],
                    ShareUrl = (string)jsonObject["share_url"],
                    Members = membersList
                };

                groupList.Add(newGroup);
            }

            return groupList;
        }

        private static List<Message> GetMessagesFromJson(JArray messageArray)
        {
            List<Message> messageList = new List<Message>();
            foreach (JObject jsonObject in messageArray)
            {
                Message message = new Message()
                {
                    Id = (string)jsonObject["id"],
                    CreatedAt = (long)jsonObject["created_at"],
                    UserId = (string)jsonObject["user_id"],
                    GroupId = (string)jsonObject["group_id"],
                    Name = (string)jsonObject["name"],
                    AvatarUrl = (string)jsonObject["avatar_url"],
                    Text = (string)jsonObject["text"],
                    System = (bool)jsonObject["system"]
                };
                messageList.Add(message);
            }
            return messageList;
        }

        /// <summary>
        /// Asynchronously posts a message to the specified group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="message"></param>
        public static async void PostMessageToGroupAsync(string groupId, string message)
        {
            HttpWebRequest request =
                WebRequest.Create(GroupMeSettings.BaseGroupMeUrl + "/groups/" + groupId + "/messages?token=" + GroupMeSettings.AccessToken) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            Stream reqStream = await request.GetRequestStreamAsync();

            string requestString = "{\"message\": {\"text\": \"" + message + "\"} }";
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
