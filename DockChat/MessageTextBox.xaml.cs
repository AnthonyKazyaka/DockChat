using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DockChat
{
    public partial class MessageTextBox : UserControl   //, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public string MessageText {get { return Message.Text; } }

        //protected virtual void OnPropertyChanged(string property)
        //{
        //    var handler = PropertyChanged;
        //    if (handler != null) handler(this, new PropertyChangedEventArgs(property));
        //}

        public string ImageUrl {get { return Message.AvatarUrl; } }

        public Color BackgroundColor { get; set; }

        public Message Message { get; set; }

        public MessageTextBox()
        {
            this.InitializeComponent();
        }

        public MessageTextBox(Message message)
        {
            
        }

        public MessageTextBox(Message message, Color backgroundColor)
        {
            this.InitializeComponent();

            Message = message;

            //BitmapImage profileImage = new BitmapImage();
            //profileImage.UriSource = new Uri(imageUrl, UriKind.Absolute);
            //UserProfileImage.Stretch = Stretch.UniformToFill;
            //UserProfileImage.Source = profileImage;

            BackgroundColor = backgroundColor;
        }
    }
}
