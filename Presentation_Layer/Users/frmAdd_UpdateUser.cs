using Bussiness_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAdd_UpdateUser : Form
    {
        clsUser user = new clsUser();
        private enum enMode { addMode, updateMode };
        private enMode nowMode = enMode.addMode;

        public frmAdd_UpdateUser(int userID)
        {
            if (userID != -1)
            {
                user = clsUser.getUserByID(userID);
                nowMode = enMode.updateMode;
                
            }
            InitializeComponent();
        }

        void fillData()
        {
            lblTitle.Text = "Update User";
            cnrlPersonInfoWithFindBy1.loadPersonInfoANDDisableFilter(user.personID);
            lblUserID.Text = user.userID.ToString();
            tbUserName.Text = user.userName;
            tbPassword.Text = user.password;
            tbConfirmPassword.Text = user.password;
            chIsActive.Checked = user.isActive;
        }

        void fillUserObject()
        {
            user.personID = cnrlPersonInfoWithFindBy1.gPersonID();
            user.userName = tbUserName.Text;
            user.password = clsHashing.ComputeHush(tbPassword.Text);
            user.isActive = chIsActive.Checked;

        }

        bool saveUser()
        {
            fillUserObject();

            return user.save();
        }

        private void frmAdd_UpdateUser_Load(object sender, EventArgs e)
        {
            if (nowMode == enMode.updateMode) 
            {
                fillData();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tbLoginInfo"])
            {
                if (clsUser.isUserExists(cnrlPersonInfoWithFindBy1.gPersonID()) && nowMode == enMode.addMode) 
                {
                    MessageBox.Show("User already exists with the same Person ID OR National No!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControl1.SelectedTab = tabControl1.TabPages["tbPersonInfo"];
                    return;
                }

                if (cnrlPersonInfoWithFindBy1.isFilled())
                    btnSave.Enabled = true;
            }
            else
                btnSave.Enabled = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tbPersonInfo"];
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tbLoginInfo"];
        }

        private void tbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "User Name is required!");
            }
            else
            {
                errorProvider1.SetError(tbUserName, "");
            }
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                errorProvider1.SetError(tbConfirmPassword, "Password are NOT match!");
            }
            else
            {
                errorProvider1.SetError(tbConfirmPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cnrlPersonInfoWithFindBy1.isFilled() == false || string.IsNullOrEmpty(tbUserName.Text) ||
                string.IsNullOrEmpty(tbPassword.Text) || string.IsNullOrEmpty(tbConfirmPassword.Text))
            {
                MessageBox.Show("Some Fields NOT Filled", "Save NOT Completed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (saveUser())
            {
                MessageBox.Show("User Saved Successfully", "Add User Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTitle.Text = "Update User";
                nowMode = enMode.updateMode;
                lblUserID.Text = user.userID.ToString();
                cnrlPersonInfoWithFindBy1.disableFilter();
            }
            else
                MessageBox.Show("User NOT Saved Successfully", "Add User Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdd_UpdateUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlPersonInfoWithFindBy1.clearImage();
        }
    }
}
