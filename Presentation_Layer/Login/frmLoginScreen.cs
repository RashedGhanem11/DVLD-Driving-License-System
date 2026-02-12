using Bussiness_Layer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmLoginScreen : Form
    {
        string path = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

        bool directLogin = false;

        void saveLoginDataInWindowsRegistry()
        {


            try
            {
                if (cbRememberMe.Checked)
                {
                    Registry.SetValue(path, "User Name", tbUserName.Text);
                    Registry.SetValue(path, "Password", tbPassword.Text);
                    Registry.SetValue(path, "Remember", "True");
                }
                else
                {
                    Registry.SetValue(path, "User Name", "");
                    Registry.SetValue(path, "Password", "");
                    Registry.SetValue(path, "Remember", "false");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void readFromWindowsRegistry()
        {

            bool remember = false;
            string userName = "";
            string password = "";

            try
            {
                string sRemember = (string)Registry.GetValue(path, "Remember", "false");

                remember = sRemember == "True";

                if (remember)
                {
                    userName = (string)Registry.GetValue(path, "User Name", "");
                    password = (string)Registry.GetValue(path, "Password", "");

                    tbUserName.Text = userName;
                    tbPassword.Text = password;
                    cbRememberMe.Checked = true;
                }
                else
                    directLogin = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error retrieving data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            

        }

        public frmLoginScreen(bool directLogin)
        {
            this.directLogin = directLogin;
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            readFromWindowsRegistry();
            if (directLogin)
                btnLogin_Click(sender, e);

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbUserName.Text) || string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Please Fill The Data", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsUser user = clsUser.getUserByUserName(tbUserName.Text);

            if (user != null)
            {
                if (user.password != clsHashing.ComputeHush(tbPassword.Text))  
                {
                    MessageBox.Show("UserName OR Password are NOT correct", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!user.isActive) 
                {
                    MessageBox.Show("Your Account is NOT active, please contact your admin", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                saveLoginDataInWindowsRegistry();
                
                frmMainForm frm = new frmMainForm();
                this.Hide();
                clsGlobalSettings.currentUser = user;
                frm.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("UserName OR Password are NOT correct", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
