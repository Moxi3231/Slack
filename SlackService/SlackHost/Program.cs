using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
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
                Console.ReadLine();
            }
        }
    }
}
