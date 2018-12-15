using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiminhaTV.Models
{
    [Table("Program")]
    public class ChProgram
    {

        public ChProgram() {

        }

        public int ChProgramId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Channel { get; set; }

        public string Title { get; set; }

        public ChProgram(string start, string end, string channel, string title)
        {
            try
            {

                string format = "yyyyMMddHHmmss";

                StartTime = DateTime.ParseExact(start.Substring(0, format.Length), format, null);
                EndTime = DateTime.ParseExact(end.Substring(0, format.Length), format, null);
                Channel = channel;
                Title = title;
                //20170610060000 -0300

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
