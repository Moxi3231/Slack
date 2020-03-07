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
                Console.WriteLine(temp.Count);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Some Exception Occurred {0}", e);
            }
            return temp;
        }
        public bool AddMessage(UMessage message)
        {
            throw new NotImplementedException();
        }



        private UGroup getFilteredGroup(UGroup u)
        {
            UGroup fin = new UGroup() { dateTime = u.dateTime, GroupName = u.GroupName, Id = u.Id };
            return fin;
        }
    }
}
