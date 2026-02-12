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
    public partial class cnrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        public int gLocalApp = -1;
        public int glicenseID = -1;
        public cnrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void loadLocalDrivingLicenseApplicationInfo(int localAppID)
        {
            gLocalApp = localAppID;

            clsLocalDrivingLicenseApplication clsLocalApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localAppID);

            cnrlApplicationInfo1.loadApplicationInfo(clsLocalApp, clsApplicationType.getApplicationTypeByID(clsLocalApp.applicationTypeID).applicationTypeTitle);

            lblLocalID.Text = clsLocalApp.localDrivingLicenseAppliactionID.ToString();
            lblAppliedFor.Text = clsLicenseClass.getLicenseClassByID(clsLocalApp.licenseClassID).className;

            int passedTestNumbers = clsTest.passedTestsNumber(localAppID);
            lblPassedTests.Text = passedTestNumbers.ToString() + "/3";

            if (passedTestNumbers == 3)
                if (clsLicense.isLicenseExists(clsLocalApp.applicationID))
                {
                    clsLicense license = clsLicense.getLicenseByApplicationID(clsLocalApp.applicationID);
                    glicenseID = license.licenseID;
                    lblShowLicenseInfo.Enabled = true;
                }
        }

        public void refreshData()
        {
            loadLocalDrivingLicenseApplicationInfo(gLocalApp);
        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails frm = new frmLicenseDetails(glicenseID);
            frm.ShowDialog();
        }
    }
}
