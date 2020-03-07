using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlackService.Model
{
    public class SlackContext : DbContext
    {
        public SlackContext() : base("name = SlackDbContext") { }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<UGroup> groups { get; set; }
        public virtual DbSet<UChannels> channels { get; set; }
        public virtual DbSet<UMessage> messages { get; set; }
    }
}
