using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SlackService.Model;
namespace SlackHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost sh = new ServiceHost(typeof(SlackService.SlackService)))
            {
                sh.Open();
                Console.WriteLine("Service Started @: {0}", DateTime.Now.ToString());
                /*
                 * SlackContext con = new SlackContext();
                User u = con.users.Where(q => q.Email == "Email2" && q.Password == "Email").SingleOrDefault();
                
                //new User() { Email = "Email2", Password = "Email", Name = "Email" };
                Console.WriteLine(u.Name);
                UGroup g1 = new UGroup() { GroupName = "g1", dateTime = DateTime.Now };
                UGroup g2 = new UGroup() { GroupName = "g2", dateTime = DateTime.Now };
                //u.groups.Add(g1);
                //u.groups.Add(g2);
                //con.users.Add(u);

                
                //Console.WriteLine(u.groups);
                *///con.SaveChanges();
                Console.ReadLine();

            }
        }
    }
}
