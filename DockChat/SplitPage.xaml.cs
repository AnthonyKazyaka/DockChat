using System.Net;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
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
// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234
using Newtonsoft.Json.Linq;

namespace DockChat
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for the
    /// currently selected item.
    /// </summary>
    public sealed partial class SplitPage : DockChat.Common.LayoutAwarePage
    {
        public SplitPage()
        {
            this.InitializeComponent();
            //DisplayGroups();
        }

        private async void DisplayGroups()
        {
            
            

            //  "{\"meta\":{\"code\":200},\"response\":[{\"id\":\"5369531\",\"group_id\":\"5369531\",\"name\":\"SteamHatch (Anthony is Gay)\",\"phone_number\":\"+1 8455763366\",\"type\":\"private\",\"description\":null,\"image_url\":null,\"creator_user_id\":\"13020970\",\"created_at\":1377453109,\"updated_at\":1379292695,\"office_mode\":false,\"share_url\":null,\"members\":[{\"id\":\"26020134\",\"user_id\":\"13020970\",\"nickname\":\"Justin Wright\",\"muted\":false,\"image_url\":null,\"autokicked\":false},{\"id\":\"26077503\",\"user_id\":\"13199206\",\"nickname\":\"Alex Cyr\",\"muted\":false,\"image_url\":\"http://i.groupme.com/5e1bb520f0b50130749c32838ab99425\",\"autokicked\":false},{\"id\":\"26020146\",\"user_id\":\"11899441\",\"nickname\":\"Anthony Kazyaka\",\"muted\":false,\"image_url\":\"http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3\",\"autokicked\":false},{\"id\":\"26020145\",\"user_id\":\"7010494\",\"nickname\":\"Sam Moore\",\"muted\":false,\"image_url\":null,\"autokicked\":false}],\"messages\":{\"count\":16,\"last_message_id\":\"137929269564661450\",\"last_message_created_at\":1379292695,\"preview\":{\"nickname\":\"Anthony Kazyaka\",\"text\":\"What do you guys think?\",\"image_url\":\"http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3\"}}},{\"id\":\"5630912\",\"group_id\":\"5630912\",\"name\":\"Team TechSmith\",\"phone_number\":\"+1 3125440839\",\"type\":\"private\",\"description\":null,\"image_url\":null,\"creator_user_id\":\"11899441\",\"created_at\":1379288320,\"updated_at\":1379288620,\"office_mode\":false,\"share_url\":null,\"members\":[{\"id\":\"27798839\",\"user_id\":\"13786653\",\"nickname\":\"Mike Quiroga\",\"muted\":false,\"image_url\":\"http://i.groupme.com/sms_avatar\",\"autokicked\":false},{\"id\":\"27798570\",\"user_id\":\"13786607\",\"nickname\":\"Joe Lindlbauer\",\"muted\":false,\"image_url\":\"http://i.groupme.com/sms_avatar\",\"autokicked\":false},{\"id\":\"27798568\",\"user_id\":\"7456140\",\"nickname\":\"David Jones\",\"muted\":false,\"image_url\":\"http://i.groupme.com/sms_avatar\",\"autokicked\":false},{\"id\":\"27798445\",\"user_id\":\"11899441\",\"nickname\":\"Anthony Kazyaka\",\"muted\":false,\"image_url\":\"http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3\",\"autokicked\":false}],\"messages\":{\"count\":6,\"last_message_id\":\"137928862093247559\",\"last_message_created_at\":1379288620,\"preview\":{\"nickname\":\"Mike Quiroga\",\"text\":\"Yay\\n- Textfree Mike\",\"image_url\":\"http://i.groupme.com/sms_avatar\"}}},{\"id\":\"4781505\",\"group_id\":\"4781505\",\"name\":\"HE HE\",\"phone_number\":\"+1 4028580680\",\"type\":\"private\",\"description\":null,\"image_url\":\"http://i.groupme.com/8c0d6e00bfea013041f4164679adaffb\",\"creator_user_id\":\"11773241\",\"created_at\":1371688043,\"updated_at\":1379275266,\"office_mode\":false,\"share_url\":null,\"members\":[{\"id\":\"22504629\",\"user_id\":\"11899441\",\"nickname\":\"Anthony Kazyaka\",\"muted\":false,\"image_url\":\"http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3\",\"autokicked\":false},{\"id\":\"25590267\",\"user_id\":\"13020970\",\"nickname\":\"Justin Wright 2\",\"muted\":false,\"image_url\":null,\"autokicked\":false},{\"id\":\"22231418\",\"user_id\":\"11773241\",\"nickname\":\"Jeremy\",\"muted\":false,\"image_url\":\"http://i.groupme.com/acaa6e60c7430130f0981ae261cd424b\",\"autokicked\":false}],\"messages\":{\"count\":134,\"last_message_id\":\"137927526682105482\",\"last_message_created_at\":1379275266,\"preview\":{\"nickname\":\"Anthony Kazyaka\",\"text\":\"There's a website you can use to send these? That's pretty baller, actually.\",\"image_url\":\"http://i.groupme.com/9699a540e19d0130f8d86ae2556dddb3\"}}}]}"
            //}
        }

        #region Page state management

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var group = SampleDataSource.GetGroup((String)navigationParameter);
            this.DefaultViewModel["Group"] = group;
            this.DefaultViewModel["Items"] = group.Items;

            if (pageState == null)
            {
                this.itemListView.SelectedItem = null;
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    var selectedItem = SampleDataSource.GetItem((String)pageState["SelectedItem"]);
                    this.itemsViewSource.View.MoveCurrentTo(selectedItem);
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = (SampleDataItem)this.itemsViewSource.View.CurrentItem;
                if (selectedItem != null) pageState["SelectedItem"] = selectedItem.UniqueId;
            }
        }

        #endregion

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed, or null
        /// for the current view state.  This parameter is optional with null as the default
        /// value.</param>
        /// <returns>True when the view state in question is portrait or snapped, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is Snapped)
        /// displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return
                // to the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // When logical page navigation is not in effect, or when there is no selected
                // item, use the default back button behavior.
                base.GoBack(sender, e);
            }
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion
    }
}
