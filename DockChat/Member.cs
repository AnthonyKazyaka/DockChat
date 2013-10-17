using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DockChat
{
    public class Member
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public bool Muted { get; set; }
        public string ImageUrl { get; set; }
        public bool Autokicked { get; set; }
        public string Guid { get; set; }
    }
}
