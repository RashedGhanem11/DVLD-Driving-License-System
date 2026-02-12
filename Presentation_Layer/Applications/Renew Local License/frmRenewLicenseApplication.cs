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
    public partial class frmRenewLicenseApplication : Form
    {
        int personID = -1;
        int newLicenseID = -1;
        int oldLicenseID = -1;
        decimal appFees = 0;

        public frmRenewLicenseApplication()
        {
            InitializeComponent();
        }

        void loadDefaultData()
        {
            personID = -1;
            newLicenseID = -1;
            lblOldLicenseID.Text = "???";
            lblExpirationDate.Text = "???";
            lblLicenseFees.Text = "???";
            lblTotalFees.Text = "???"; 
            btnIssue.Enabled = false;
            lblShowNewLicenseInfo.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        bool checkData(int licenseID)
        {
            clsLicense oldlicense = clsLicense.getLicenseByID(licenseID);
            lblOldLicenseID.Text = licenseID.ToString();

            clsLicenseClass oldLicenseClass = clsLicenseClass.getLicenseClassByID(oldlicense.licenseClassID);
            lblLicenseFees.Text = decimal.ToSingle(oldLicenseClass.classFees).ToString();
            lblTotalFees.Text = decimal.ToSingle(oldLicenseClass.classFees + appFees).ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(oldLicenseClass.defaultValidityLength).ToShortDateString();

            personID = clsDriver.getDriverByID(oldlicense.driverID).personID;
            oldLicenseID = oldlicense.licenseID;

            if (!oldlicense.isActive)
            {
                MessageBox.Show($"Local license is NOT active", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (DateTime.Compare(DateTime.Now, oldlicense.expirationDate) < 1)
            {
                MessageBox.Show($"Local license expiration date ends in {oldlicense.expirationDate}", "NOT Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void cnrlLicenseInfoWithFindBy1_OnFindLicenseButtonClick(int obj)
        {
            loadDefaultData();

            if (obj == -1)
                return;
            else
                lblShowLicensesHistory.Enabled = true;

            if (checkData(obj))
            {
                btnIssue.Enabled = true;
            }
        }

        private void frmRenewLicenseApplication_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsGlobalSettings.currentUser.userName;
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            
            decimal appFees = clsApplicationType.getApplicationTypeByID(2).applicationFees;
            lblAppFees.Text = decimal.ToSingle(appFees).ToString();
            this.appFees = appFees;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsRenewLicense newLicense = new clsRenewLicense(oldLicenseID, string.IsNullOrEmpty(tbNotes.Text) ? "" : tbNotes.Text);

            newLicense.personID = personID;
            newLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            if (newLicense.save()) 
            {
                clsLicense license = clsLicense.getLicenseByApplicationID(newLicense.applicationID);
                newLicenseID = license.licenseID;
                MessageBox.Show($"New License Saved Successfully with ID = {license.licenseID}", "License Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cnrlLicenseInfoWithFindBy1.disableFilter();
                tbNotes.Enabled = false;
                btnIssue.Enabled = false;
                lblShowNewLicenseInfo.Enabled = true;
                lblNewLicenseID.Text = license.licenseID.ToString();
                lblNewAppID.Text = license.applicationID.ToString();
            }
            else
            {
                MessageBox.Show("New License Saved Failed ", "License Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void frmRenewLicenseApplication_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlLicenseInfoWithFindBy1.clearImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicensesHistory frm = new frmPersonLicensesHistory(personID);
            frm.ShowDialog();
        }

        private void lblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails frm = new frmLicenseDetails(newLicenseID);
            frm.ShowDialog();
        }
    }
}
