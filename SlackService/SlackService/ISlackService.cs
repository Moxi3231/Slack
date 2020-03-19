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
        [OperationContract]
        bool AddGroup(UGroup uGroup,User user);
        [OperationContract]
        bool AddChannel(UChannels uChannels,UGroup ugroup,User user);
        [OperationContract]
        bool AddMessage(UMessage message,UChannels channels,User user);
        [OperationContract]
        IEnumerable<UGroup> GetUGroups(User u);
        [OperationContract]
        IEnumerable<UChannels> GetUChannels(UGroup g, User u);
        [OperationContract]
        IEnumerable<UMessage> GetUMessages(UChannels channels);
        [OperationContract]
        bool AddUserToGroup(User user, UGroup group);
        [OperationContract]
        IEnumerable<User> getGroupMember(UGroup group);
        [OperationContract]
        bool removeUserFromGroup(User u, UGroup gr);
    }
}
