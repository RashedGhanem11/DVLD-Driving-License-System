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
    public partial class frmIssueDrivingLicenseForFirstTime : Form
    {
        public frmIssueDrivingLicenseForFirstTime(int localAppID)
        {

            InitializeComponent();
            cnrlLocalDrivingLicenseApplicationInfo1.loadLocalDrivingLicenseApplicationInfo(localAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsLicense newLicense = new clsLicense();
            clsLocalDrivingLicenseApplication localApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(cnrlLocalDrivingLicenseApplicationInfo1.gLocalApp);

            newLicense.applicationID = localApp.applicationID;
            newLicense.licenseClassID = localApp.licenseClassID;

            if (!string.IsNullOrEmpty(tbNotes.Text))
                newLicense.notes = tbNotes.Text;

            newLicense.createdByUserID = clsGlobalSettings.currentUser.userID;

            newLicense.issueDate = DateTime.Now;
            short licenseValidity = clsLicenseClass.getLicenseClassByID(newLicense.licenseClassID).defaultValidityLength;
            newLicense.expirationDate = new DateTime(DateTime.Now.Year + licenseValidity, DateTime.Now.Month, DateTime.Now.Day);
            newLicense.paidFees = clsLicenseClass.getLicenseClassByID(newLicense.licenseClassID).classFees;

            if (newLicense.save())
            {
                MessageBox.Show("License Issued Successfully", "Issue License Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("License NOT Saved Successfully", "Issue License Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
