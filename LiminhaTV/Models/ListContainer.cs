using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiminhaTV.Models
{
    [Table("ListContainer")]
    public class ListContainer
    {
        public int ListContainerID { get; set; }

        public string Url { get; set; }

        public ListType Type { get; set; }
    }

    public enum ListType {

        EPG,
        Channel
    }
}
