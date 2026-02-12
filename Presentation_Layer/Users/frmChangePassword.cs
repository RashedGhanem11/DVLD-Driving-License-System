using Bussiness_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmChangePassword : Form
    {
        clsUser user = new clsUser();

        public frmChangePassword(int userID)
        {
            InitializeComponent();
            user = clsUser.getUserByID(userID);
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            cnrlUserInfo1.loadUserInfo(user.userID);
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text != tbConfirmPassword.Text)
            {
                errorProvider1.SetError(tbConfirmPassword, "Password are NOT match!");
            }
            else
            {
                errorProvider1.SetError(tbConfirmPassword, "");
            }
        }

        private void tbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (clsHashing.ComputeHush(tbCurrentPassword.Text) != user.password) 
            {
                errorProvider1.SetError(tbCurrentPassword, "Wrong Password!");
            }
            else
            {
                errorProvider1.SetError(tbCurrentPassword, "");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (user.password != clsHashing.ComputeHush(tbCurrentPassword.Text)) 
            {
                MessageBox.Show("Current Password NOT Correct", "Saved NOT Complete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbNewPassword.Text)) 
            {
                MessageBox.Show("Enter the new password", "Saved NOT Complete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tbNewPassword.Text != tbConfirmPassword.Text) 
            {
                MessageBox.Show("Password NOT Match", "Saved NOT Complete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            user.password = clsHashing.ComputeHush(tbNewPassword.Text);

            if (user.save())
                MessageBox.Show("Password Saved Successfully", "Saved Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Something Went Wrong", "Saved NOT Complete", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void frmChangePassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlUserInfo1.clearImage();
        }
    }
}
