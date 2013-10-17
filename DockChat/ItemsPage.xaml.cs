using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using DockChat.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233
using Newtonsoft.Json.Linq;

namespace DockChat
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : DockChat.Common.LayoutAwarePage
    {

        public List<Group> UserGroups { get; set; }
        public string CurrentGroupId { get; set; }
        public List<Message> CurrentGroupMessages { get; set; }

        public ItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            // var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);

            Task<JArray> userGroupsTask = GetGroups();

            UserGroups = GetGroupsFromJson(await userGroupsTask);

            foreach (Group group in UserGroups)
            {
                ListBoxItem lbi = new ListBoxItem()
                {
                    Content = group.Name,
                    Name = group.Id + ""
                };
                lbi.Tapped += GroupListBoxItem_Clicked;
                GroupListBox.Items.Add(lbi);
            }
            // this.DefaultViewModel["Items"] = userGroupsArray;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(SplitPage), groupId);
        }

        private async Task<JArray> GetGroups()
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
            return userGroupsArray;
        }

        private List<Group> GetGroupsFromJson(JArray groupsArray)
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
                        Id = (string) member["id"],
                        UserId = (string) member["user_id"],
                        Nickname = (string) member["nickname"],
                        Muted = (bool) member["muted"],
                        ImageUrl = (string) member["image_url"],
                        Autokicked = (bool) member["autokicked"]
                    };
                    membersList.Add(newMember);
                }
                Group newGroup = new Group()
                {
                    Id = (string) jsonObject["id"],
                    GroupId = (string) jsonObject["group_id"],
                    Name = (string) jsonObject["name"],
                    PhoneNumber = (string) jsonObject["phone_number"],
                    PrivacyType = ((string) jsonObject["type"] == "private") ? Group.GroupType.Private : Group.GroupType.Public,
                    Description = (string) jsonObject["description"],
                    ImageUrl = (string) jsonObject["image_url"],
                    CreatorUserId = (string) jsonObject["creator_user_id"],
                    CreatedAt = (long) jsonObject["created_at"],
                    UpdatedAt = (long) jsonObject["updated_at"],
                    OfficeMode = (bool) jsonObject["office_mode"],
                    ShareUrl = (string) jsonObject["share_url"],
                    Members = membersList
                };

                groupList.Add(newGroup);
            }

            return groupList;
        }

        private void GroupListBoxItem_Clicked(object sender, RoutedEventArgs e)
        {
            string groupdId = (sender as ListBoxItem).Name;
            CurrentGroupId = groupdId;
            GetGroupMessages(groupdId);
        }

        private async void GetGroupMessages(string groupId)
        {
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

            CurrentGroupMessages = GetMessagesFromJson(messagesArray);
            MessagesListBox.Items.Clear();
            List<Message> reversedGroupMessages = CurrentGroupMessages;
            reversedGroupMessages.Reverse();

            foreach (Message m in reversedGroupMessages)
            {
                ListBoxItem lbi = new ListBoxItem()
                {
                    Content = m.Text + "\n- (" + m.Name + ")"
                };
                MessagesListBox.Items.Add(lbi);
            }
            if (MessagesListBox.Items.Count > 0)
            {
                MessagesListBox.ScrollIntoView(MessagesListBox.Items[MessagesListBox.Items.Count - 1]);
            }


            //return messagesArray;
            //{
            //  "id": "138034088713787624",
            //  "source_guid": "79708b4b-21fd-47e2-bfa4-d62f0b30dd76",
            //  "created_at": 1380340887,
            //  "user_id": "11899441",
            //  "group_id": "5630912",
            //  "name": "Anthony Kazyaka",
            //  "avatar_url": "http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3",
            //  "text": "Joe is the fucking man!",
            //  "system": false,
            //  "attachments": [],
            //  "favorited_by": []
            //}
        }

        private List<Message> GetMessagesFromJson(JArray messageArray)
        {
            List<Message> messageList = new List<Message>();
            foreach (JObject jsonObject in messageArray)
            {
                Message message = new Message()
                {
                    Id = (string) jsonObject["id"],
                    CreatedAt = (long) jsonObject["created_at"],
                    UserId = (string) jsonObject["user_id"],
                    GroupId = (string) jsonObject["group_id"],
                    Name = (string) jsonObject["name"],
                    AvatarUrl = (string) jsonObject["avatar_url"],
                    Text = (string) jsonObject["text"],
                    System = (bool) jsonObject["system"]
                };
                messageList.Add(message);
            }
            return messageList;
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            PostMessageToGroup(CurrentGroupId, MessageTextBox.Text);
        }

        private async void PostMessageToGroup(string groupId, string message)
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
