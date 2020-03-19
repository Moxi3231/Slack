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
                System.Console.WriteLine(e);
                return false;
            } return true;
        }
        public bool CheckUser(User u)
        {
            
            var temp = con.users.Where(q => q.Email == u.Email && q.Password == u.Password).ToList();
            //Console.WriteLine(temp.Count);
            if (temp.Count == 0)
                return false;
            return true;
        }

        public bool AddGroup(UGroup uGroup,User user)
        {
            User u = getUser(user);
            uGroup = getFilteredGroup(uGroup);
            if (u == null || uGroup==null)
                return false;
            try
            {
                uGroup.users.Add(u);
                u.groups.Add(uGroup);
                con.groups.Add(uGroup);
                con.SaveChanges();
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
            return true;
            
            //throw new NotImplementedException();
        }
        private User getUser(User user)
        {
            return con.users.Where(q => q.Email == user.Email && q.Password == user.Password).FirstOrDefault();
        }
        private User getUserWithoutPass(User user)
        {
            return con.users.Where(q => q.Email == user.Email).FirstOrDefault();
        }
        private int getUserId(User user)
        {
            var temp = getUser(user);
            if (temp == null)
                return -1;
            return temp.Id;
        }
        public IEnumerable<UGroup> GetUGroups(User user)
        {
            User u = getUser(user);
            if(u==null)
            {
                return null;
            }
            var temp = new List<UGroup>();
            try
            {
                temp = con.users.Where(q => q.Email == user.Email && q.Password == user.Password).SelectMany(q => q.groups).ToList();
                //Console.WriteLine(temp.Count);
                //   Where(q => q.users.Contains(u)).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in UGROUPS\n{0}",e);
            }
            //Console.WriteLine("\nInner,{0}",temp);
            //foreach (UGroup ug in temp)
              //  Console.WriteLine(ug.GroupName);
            return temp;
        }
        public bool AddChannel(UChannels uChannels, UGroup uGroup, User user)
        {
            try
            {
                int UID = getUserId(user);
                if (UID == -1)
                    return false;
                var group = con.groups.Where(x => x.Id == uGroup.Id && x.GroupName == uGroup.GroupName).FirstOrDefault();
                if (group == null)
                    return false;
                con.channels.Add(uChannels);
                group.channels.Add(uChannels);
                con.SaveChanges();
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception Occurred {0}",e);
            }
            return true;
        }
        public IEnumerable<UChannels> GetUChannels(UGroup groups,User user)
        {
            var temp = new List<UChannels>();
            //Console.WriteLine("I Was Called");
            try
            {
                int UID = getUserId(user);
                //Console.WriteLine(UID);
                if (UID == -1)
                    return null;
                var tG = con.users.Where(u => u.Id == UID).SelectMany(x => x.groups).Where(x => x.Id == groups.Id);
                temp = tG.SelectMany(x => x.channels).ToList();
                //Console.WriteLine(temp.Count);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Some Exception Occurred {0}", e);
            }
            return temp;
        }
        public bool AddMessage(UMessage message,UChannels uChannels,User user)
        {
            var temp = con.channels.Where(q => q.Id == uChannels.Id).FirstOrDefault();
            if (temp == null)
                return false;
            var u = getUser(user);
            if (u == null)
                return false;
            message.user = u;
            temp.messages.Add(message);
            con.SaveChanges();
            return true;
            //throw new NotImplementedException();
        }
        public IEnumerable<UMessage> GetUMessages(UChannels channels) 
        {
            var temp = con.channels.Where(q => q.Id == channels.Id).SelectMany(x=>x.messages ).ToList();
            var fin = new List<UMessage>();
            foreach(UMessage uMessage in temp)
            {
                var chT = con.messages.Where(q => q.Id == uMessage.Id).Select(x=>x.user).FirstOrDefault();
                
                //Console.WriteLine(chT.Email);
                uMessage.user = chT;
                fin.Add(uMessage);
            }
            //Console.WriteLine(temp.Count);
            return temp;
            
        }
        public bool AddUserToGroup(User user,UGroup group)
        {
            var us = getUserWithoutPass(user);
            var gr = con.groups.Where(q => q.Id == group.Id).FirstOrDefault();
            if (us == null || gr == null)
                return false;
            us.groups.Add(gr);
            con.SaveChanges();
            return true;
        }
        public IEnumerable<User> getGroupMember(UGroup gr)
        {
            var group = con.groups.Where(p => p.Id == gr.Id).FirstOrDefault();
            if (group == null)
                return null;
            var users = con.groups.Where(p => p.Id == group.Id).SelectMany(u => u.users).ToList();
            return users;
//            return null;
        }
        public bool removeUserFromGroup(User user,UGroup ugroup)
        {
            User utemp = getUserWithoutPass(user);
            if (utemp == null)
                return false;
            UGroup ug = con.groups.Where(u => u.Id == ugroup.Id).FirstOrDefault();
            if (ug == null)
                return false;
            var res = con.groups.Include("users").Where(u => u.Id == ug.Id).FirstOrDefault();
            res.users.Remove(utemp);

            con.SaveChanges();
            return true;
        }
        private UGroup getFilteredGroup(UGroup u)
        {
            UGroup fin = new UGroup() { dateTime = u.dateTime, GroupName = u.GroupName, Id = u.Id };
            return fin;
        }
    }
}
