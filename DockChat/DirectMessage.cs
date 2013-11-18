using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockChat
{
    public class DirectMessage
    {
        public string SourceGuid { get; set; }
        public string Id { get; set; }
        public long CreatedAt { get; set; }
        public string UserId { get; set; }
        //public string GroupId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Text { get; set; }
        public bool System { get; set; }
    }
}
