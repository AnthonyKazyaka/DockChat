using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockChat
{
    public static class GroupMeSettings
    {
        public static string RedirectUrl
        {
            get { return "https://api.groupme.com/oauth/authorize?client_id=oCAwCoRX3rViGnW98CRTB6jv7VfW1PaUwG98kK6hubB8Dndk"; }
        }

        public static string ClientId {get { return "oCAwCoRX3rViGnW98CRTB6jv7VfW1PaUwG98kK6hubB8Dndk"; }}

        public static string AccessToken {get { return "21c76d1000d1013193d116905812a7de"; }}

        public static string RequestUrl { get { return "https://api.groupme.com/oauth/authorize?client_id=" + ClientId; } }

        public static string BaseGroupMeUrl {get { return "https://api.groupme.com/v3"; }}

        public static bool IsAuthenticated { get; set; }

    }
}
