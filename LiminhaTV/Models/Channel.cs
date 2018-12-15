using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiminhaTV.Models
{
    [Table("Channel")]
    public class Channel
    {
        public Channel() {

        }

        public Channel(string title, string logo, string group)
        {
           // Duration = duration;
            Title = title;
            LogoURL = logo;
            Group = group;
            // Path = path;
        }
        public int ChannelId { get; set; }
        public string Title { get; set; }
        public string LogoURL { get; set; }
        public string Group { get; set; }
        public string Path { get; set; }
        public int? Category { get; set; }

    }
}