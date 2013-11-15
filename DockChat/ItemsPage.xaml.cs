using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Windows.UI;
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

            User.CurrentUser.Groups = await Group.GetGroups();

            foreach (Group group in User.CurrentUser.Groups)
            {
                GroupDisplayBox gdb = new GroupDisplayBox(group);
                group.Messages = await Group.GetGroupMessages(group.Id);
                gdb.Tapped += GroupListBoxItem_Clicked;
                GroupListBox.Items.Add(gdb);
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

        private void GroupListBoxItem_Clicked(object sender, RoutedEventArgs e)
        {
            Group group = (sender as GroupDisplayBox).Group;
            GetAndDisplayGroupMessages(group);
        }

        private async void GetAndDisplayGroupMessages(Group group)
        {
            CurrentGroupId = group.Id;
            await GetGroupMessages(group);
            ClearMessagesListBox();
            ShowMessages(group);
            ClearGroupMembersList();
            DisplayGroupMembers(group);
        }

        private void ShowMessages(Group group)
        {
            foreach (Message message in group.Messages)
            {
                MessageTextBox mtb = new MessageTextBox(message, Color.FromArgb(255, 255, 0, 255));
                MessagesListBox.Items.Add(mtb);
            }
            
            MessagesListBox.ScrollIntoView(MessagesListBox.Items.LastOrDefault());
        }

        private async Task<bool> GetGroupMessages(Group group)
        {
            if (group.Messages != null)
            {
                return true;
            }
            User.CurrentUser.Groups.First(x => x.GroupId == group.Id).Messages = await Group.GetGroupMessages(group);
            return true;
        }


        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessageToGroup();
        }

        private void SendMessageToGroup()
        {
            Group.PostMessageToGroupAsync(CurrentGroupId, MessageTextBox.Text);            
        }

        private void DisplayGroupMembers(Group group)
        {
            foreach (Member member in group.Members)
            {
                GroupMemberDisplay display = new GroupMemberDisplay(member);
                GroupMembersListBox.Items.Add(display);
            }
        }

        private void ClearMessagesListBox()
        {
            MessagesListBox.Items.Clear();
        }

        private void ClearGroupMembersList()
        {
            GroupMembersListBox.Items.Clear();
        }
    }
}
