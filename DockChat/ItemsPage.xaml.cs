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

        private void GroupListBoxItem_Clicked(object sender, RoutedEventArgs e)
        {
            string groupId = (sender as ListBoxItem).Name;
            GetAndDisplayGroupMessages(groupId);
        }

        private async void GetAndDisplayGroupMessages(string groupId)
        {
            CurrentGroupId = groupId;
            await GetGroupMessages(groupId);
            ShowMessages(groupId);
        }

        private void ShowMessages(string groupId)
        {
            Group group = User.CurrentUser.Groups.First(x => x.GroupId == groupId);
            foreach (Message message in group.Messages)
            {
                ListBoxItem lbi = new ListBoxItem()
                {
                    Content = message.Text
                };
                MessagesListBox.Items.Add(lbi);
            }
            
            MessagesListBox.ScrollIntoView(MessagesListBox.Items[0]);
        }

        private async Task<bool> GetGroupMessages(string groupId)
        {
            Group group = User.CurrentUser.Groups.First(x => x.GroupId == groupId);
            if (group.Messages != null)
            {
                return true;
            }
            User.CurrentUser.Groups.First(x => x.GroupId == groupId).Messages = await Group.GetGroupMessages(groupId);
            return true;
        }


        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Group.PostMessageToGroupAsync(CurrentGroupId, MessageTextBox.Text);
        }
    }
}
