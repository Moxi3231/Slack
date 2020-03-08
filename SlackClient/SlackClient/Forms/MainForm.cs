using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlackClient.SlackService;
using SlackClient.Classes;
using System.Threading;

namespace SlackClient.Forms
{
    public partial class MainForm : Form
    {
        private SlackServiceClient slackService = new SlackServiceClient();
        private User currentUser { get; set; }
        private UChannels currentChannel { get; set; }
        private UGroup currentGroup { get; set; }
        private UGroup[] uGroups;
        private UChannels[] uChannels { set; get; }
        private UMessage[] uMessages { get; set; }
        private GlobalClass globalClass = GlobalClass.getGlobalClassInstance();
        public MainForm()
        {
            InitializeComponent();
            MaximizeBox = false;
            panel1.Show();
            panel2.Hide();
            panel3.Hide();
            panel3.BackColor = Color.FromArgb(21, 67, 96);
            chatPanel.BackColor = Color.FromArgb(212, 230, 241);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MainLogin
            User user = new User() { Email = loginEmail.Text, Password = loginPasssword.Text };
            bool st = slackService.CheckUser(user);
            
            if (st)
            {
                panel3.Show();
                panel1.Hide();
                panel2.Hide();
                currentUser = user;
                globalClass.user = user;
                getGroups();
                return;
            }
            else
            {
                MessageBox.Show("Please Enter Valid Credentials");
                return;
            }
           
        }
        
        private void getGroups()
        {
            if (currentUser == null)
                return;
            uGroups = slackService.GetUGroups(currentUser);
            //getGroups();
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            foreach (UGroup ug in uGroups)
            {
                //Console.WriteLine(ug.GroupName);
                //var temp = new Label() { Text = ug.GroupName, Name = ug.Id.ToString() };
                listBox1.Items.Add(ug.GroupName);
                comboBox1.Items.Add(ug.GroupName);
                comboBox2.Items.Add(ug.GroupName);
                //Console.WriteLine("{0}{1}", temp, temp.Text);
            }
            
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (registerEmail.Text == null || registerName.Text == null || registerPassword.Text == null)
            {
                MessageBox.Show("One of required field is missing");
                return;
            }//Main Register
            User user = new User()
            {
                Email = registerEmail.Text,
                Name = registerName.Text,
                Password = registerPassword.Text,
                Profession = registerOccupation.Text,
                status = registerStatus.Text
            };
            bool st = slackService.RegisterUser(user);
            if(st)
            {
                panel3.Show();
                currentUser = user;
                globalClass.user = user;

                getGroups();
            }
            else
            {
                MessageBox.Show("Email Already Registered");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Switch TO LOGIN
            panel2.Hide();
            panel1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Switch TO register
            panel2.Show();
            panel1.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //new Form1();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex==-1 || comboBox1.SelectedIndex > uGroups.Length)
                return;
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
            currentGroup = uGroups[comboBox1.SelectedIndex];
            groupName.Text = "to " + comboBox1.SelectedItem.ToString();
            updateChannel();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //Add group currently logged in user
            if(newGroup.Text==null)
            {
                MessageBox.Show("Group must have name");
                return;
            }
            bool st = slackService.AddGroup(new UGroup()
            {
                dateTime = DateTime.Now,
                GroupName = newGroup.Text
            },currentUser);
            Console.WriteLine(st+"\n\n\n");
        }
        private void updateChannel()
        {
            if (currentGroup == null || currentUser == null)
                return;
            listBox2.Items.Clear();
            comboBox3.Items.Clear();
            try
            {
                uChannels = slackService.GetUChannels(currentGroup, currentUser);
                foreach (UChannels ch in uChannels)
                {
                    listBox2.Items.Add(ch.ChannelName);
                    comboBox3.Items.Add(ch.ChannelName);
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Some Exception Occurred {0}", e);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //Adding New Channel
            if(currentGroup==null)
            {
                MessageBox.Show("Please Select Group From DropDown!!\nIf not then create");
                return;
            }
            if(newChannel==null)
            {
                MessageBox.Show("Missing Channel Name");
                return;
            }
            bool st = slackService.AddChannel(new UChannels()
            {
                ChannelName = newChannel.Text,
                dateTime = DateTime.Now,
                isPublic = newChannelisPulic.Checked,
                purpose = newChannelDesc.Text
            }, currentGroup, currentUser);
            updateChannel();
            if(!st)
            {
                MessageBox.Show("Some Error Occurred");
                return;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox1_SelectedIndexChanged(sender,e);
            if (comboBox2.SelectedIndex == -1 || comboBox2.SelectedIndex > uGroups.Length)
                return;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex;
            currentGroup = uGroups[comboBox2.SelectedIndex];
            groupName.Text = "to " + comboBox1.SelectedItem.ToString();
            updateChannel();
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(currentGroup==null)
            {
                MessageBox.Show("Please Select Group");
                return;
            }
            if(comboBox3.SelectedIndex==-1 || comboBox3.SelectedIndex>uChannels.Length)
            {
                return;
            }
            currentChannel = uChannels[comboBox3.SelectedIndex];
            chatsLabel.Text = "Group: " + currentGroup.GroupName + "\t Channel: " + currentChannel.ChannelName;
            channelDescriptionLabel.Text = currentChannel.purpose;
            loadChats();
        }
        public void loadChats()
        {
            uMessages = slackService.GetUMessages(currentChannel, currentUser);
            
            Label t = new Label();
            //t.Text = "First Message";
            Size sz = new Size(0,0);
            t.Size = sz;
            sz.Width = 240;
            sz.Height = 50;
            chatPanel.Controls.Clear();
            chatPanel.Controls.Add(t);

            Point pInitial = t.Location;
            foreach(UMessage message in uMessages)
            {
                Label temp = new Label();
                temp.BackColor = Color.FromArgb(46, 134, 193);
                
                pInitial.Y += 60;
                temp.Height = 50;
                temp.Width = 240;
                if(message.user.Email == currentUser.Email)
                {
                    pInitial.X = pInitial.X - 240 + chatPanel.Width;
                    temp.Location = pInitial;
                    pInitial.X = pInitial.X + 240 - chatPanel.Width;
                    temp.Text = string.Format(message.MesssageDecription + "\n~Me \nTime: " + message.dateTime);
                }
                else
                {
                    temp.Location = pInitial;
                    temp.Text = message.MesssageDecription + "\n" + "~By " + message.user.Name + " \nTime: " + message.dateTime;
                }
                chatPanel.Controls.Add(temp);
            }
            /*
             * for(int i=1;i<100;i++)
            {
                Label temp = new Label();
                
                pInitial.Y += 60;
                temp.Location = pInitial;
                temp.Size = sz;
                temp.Text = "\nMessage" + i.ToString();
                if(i%10==0)
                {
                    pInitial.X = pInitial.X - 120 + chatPanel.Width;
                    temp.Location = pInitial;
                    pInitial.X = pInitial.X + 120 - chatPanel.Width;
                }
                chatPanel.Controls.Add(temp);   
                
            }
            */
            chatPanel.AutoSize = false;
            //chatPanel.SetAutoScrollMargin(2, );
            
            chatPanel.Show();
            
            chatPanel.BorderStyle = BorderStyle.Fixed3D;
        }

        private void chatSend_Click(object sender, EventArgs e)
        {
            if (newChatDescription.Text == null || currentChannel == null || currentUser == null || currentGroup == null)
            {
                MessageBox.Show("Cannot Send Message\nPlease Select Group or Channel Or Both");
                return;
            }
            bool st = slackService.AddMessage(new UMessage(){ 
                dateTime =DateTime.Now,
                MesssageDecription=newChatDescription.Text,
                user = currentUser
            },currentChannel,currentUser);
            if(!st)
            {
                MessageBox.Show("Couldn't Send Message \nPlease Try Again Later");
                return;
            }
            loadChats();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            if (listBox1.SelectedIndex > uGroups.Length)
                return;
            removeGroupBtn.Text = "Remove " + uGroups[listBox1.SelectedIndex].GroupName;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentChannel == null)
                return;
            if (listBox2.SelectedIndex == -1)
                return;
            if (listBox2.SelectedIndex > uChannels.Length)
                return;
            removeChannelBtn.Text = "Remove " + currentChannel.ChannelName + " From Group: " + currentGroup.GroupName;
        }
    }
}
