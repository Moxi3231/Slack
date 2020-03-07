using SlackService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SlackService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISlackService" in both code and config file together.
    [ServiceContract]
    public interface ISlackService
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        bool RegisterUser(User usr);
        [OperationContract]
        bool CheckUser(User usr);
    }
}
