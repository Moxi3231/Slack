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
        private GlobalClass globalClass = GlobalClass.getGlobalClassInstance();
        public MainForm()
        {
            InitializeComponent();
            MaximizeBox = false;
            panel1.Show();
            panel2.Hide();
            panel3.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MainLogin
            User user = new User() { Email = loginEmail.Text, Password = loginPasssword.Text };
            bool st = slackService.CheckUser(user);
            if(st)
            {
                panel3.Show();
                panel1.Hide();
                panel2.Hide();
                currentUser = user;
                globalClass.user = user;
                getGroups();
            }
            else
            {
                MessageBox.Show("Please Enter Valid Credentials");
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
            try
            {
                uChannels = slackService.GetUChannels(currentGroup, currentUser);
                foreach (UChannels ch in uChannels)
                {
                    listBox2.Items.Add(ch.ChannelName);
                }
            }
            catch(Exception e)
            {

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
    }
}
