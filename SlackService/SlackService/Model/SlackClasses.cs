using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace SlackService.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string Profession { get; set; }

        public string status { get; set; }
        public IEnumerable<UGroup> groups { get; set; }

    }
    public class UGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public string GroupName { get; set; }
        public IEnumerable<User> adminUser { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<UChannels> channels { get; set; }

        [Required]
        public DateTime creationTime { get; set; }
    }
    public class UChannels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public string ChannelName { get; set; }
        [Required]
        public UGroup group { get; set; }
        [Required]
        public string purpose { get; set; }
        [Required]
        public bool isPublic { get; set; }
        
        public IEnumerable<UMessage> messages { get; set; }

    }
    public class UMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MesssageDecription { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public User user { get; set; }
        [Required]
        public UChannels channel { get; set; }
    }

}
