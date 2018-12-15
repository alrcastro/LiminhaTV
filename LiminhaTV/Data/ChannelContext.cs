using LiminhaTV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiminhaTV.Data
{
    public class ChannelContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }

        public DbSet<ChProgram> Programs { get; set; }

        public DbSet<ListContainer> ListContainer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=channels.db");
        }
    }
}
