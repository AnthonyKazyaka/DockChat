using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
