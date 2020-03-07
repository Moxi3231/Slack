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
namespace SlackClient.Forms
{
    public partial class MainForm : Form
    {
        private SlackServiceClient slackService = new SlackServiceClient();
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
                //this.Close();
                getGroups();
               
            }
            else
            {
                MessageBox.Show("Please Enter Valid Credentials");
            }
        }
        
        private void getGroups()
        {
            for(int i=0;i<10;i++)
            {
                listBox1.Items.Add("Item" + i.ToString());
                comboBox1.Items.Add("Item" + i.ToString());
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
            MessageBox.Show(comboBox1.SelectedIndex.ToString());
            groupName.Text = "to " + comboBox1.SelectedItem.ToString();
        }
    }
}
