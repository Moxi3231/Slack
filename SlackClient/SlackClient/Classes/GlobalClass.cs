using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlackClient.SlackService;
namespace SlackClient.Classes
{
    public class GlobalClass
    {
        public static GlobalClass globalClass;
        public  Dictionary<string, Object> pairs { get; set; }
        public  User user { get; set; }
        private GlobalClass()
        { }
        public static  GlobalClass getGlobalClassInstance()
        {
            if(globalClass==null)
            {
                globalClass = new GlobalClass();
                globalClass.pairs = new Dictionary<string, object>();
            }
            return globalClass;
        }
        
    }
}
