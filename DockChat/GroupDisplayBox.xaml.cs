using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DockChat
{
    public sealed partial class GroupDisplayBox : UserControl
    {

        public Group Group { get; set; }

        private Message _lastMessageInGroup {get { return Group.Messages.Last(); }}
        public string LastMessageDisplayText {get { return _lastMessageInGroup.Name + ": " + _lastMessageInGroup.Text; } }

        public string ImageUrl {get { return Group.ImageUrl; }}
        public string Description {get { return Group.Description; }}
        public string GroupName {get { return Group.Name; }}

        public GroupDisplayBox()
        {
            this.InitializeComponent();
        }

        public GroupDisplayBox(Group group)
        {
            this.InitializeComponent();

            Group = group;
        }

    }
}
