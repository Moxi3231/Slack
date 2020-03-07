using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SlackService.Model;
namespace SlackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SlackService" in both code and config file together.
    public class SlackService : ISlackService
    {
        private SlackContext con = new SlackContext();
        public void DoWork()
        {
           
            con.users.Add(new User() { Email = "b", Name = "a", Password = "n", Profession = "n", status = "working" });
            con.SaveChanges();
           
            Console.WriteLine("User Saved");
            Console.WriteLine(con.users.Select(q => q).ToList().ToString());
            var users = con.users.Select(q => q).ToList();
            foreach(User u in users)
            {
                Console.WriteLine(u.Email);
            }
        }
        public bool RegisterUser(User u)
        {
            var temp = con.users.Where(q => q.Email == u.Email).Count();
            
            if (temp > 0)
                return false;
            try
            {
                con.users.Add(u);
                con.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            } return true;
        }
        public bool CheckUser(User u)
        {
            
            var temp = con.users.Where(q => q.Email == u.Email && q.Password == u.Password).ToList();
            Console.WriteLine(temp.Count);
            if (temp.Count == 0)
                return false;
            return true;
        }
    }
}
