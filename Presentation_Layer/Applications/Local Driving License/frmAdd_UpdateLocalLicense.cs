using Bussiness_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAdd_UpdateLocalLicense : Form
    {
        clsLocalDrivingLicenseApplication localApplication = new clsLocalDrivingLicenseApplication();
        private enum enMode { addMode, updateMode };
        private enMode nowMode = enMode.addMode;

        bool checkAge(clsPerson person, int minimumAllowedAge)
        {
            int personAge = person.calculateAge();

            return personAge >= minimumAllowedAge;
        }

        bool checkData(clsPerson person, clsLicenseClass licenseClass)
        {
            int applicationID = clsLocalDrivingLicenseApplication.isPersonHaveAnActiveLocalLicenseApplication(person.personID, licenseClass.licenseClassID);

            if (applicationID != -1)
            {
                clsLocalDrivingLicenseApplication localApp = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByApplicationID(applicationID);

                if (localApp.applicationStatus == 1)
                    MessageBox.Show($"Person Have already an New application with the same license class, application ID = {applicationID}",
                        "Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (localApp.applicationStatus == 3)
                    MessageBox.Show($"Person Have already a license the same license class",
                        "Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            if (!checkAge(person, licenseClass.minimumAllowedAge)) 
            {
                MessageBox.Show($"Person age less than minimum allowed age for the license class",
                   "Saved Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }

            return true;
        }

        public frmAdd_UpdateLocalLicense(int localApplicationID)
        {
           
            InitializeComponent(); 
            if (localApplicationID != -1) 
            {
                localApplication = clsLocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(localApplicationID);
                nowMode = enMode.updateMode;
            }
        }

        void fillLocalApplicationData(clsPerson person, clsLicenseClass licenseClass)
        {
            localApplication.licenseClassID = licenseClass.licenseClassID;
            localApplication.personID = person.personID;
            localApplication.createdByUserID = clsGlobalSettings.currentUser.userID;        
        }

        void fillcbLicenseClasses()
        {
            DataTable dt = clsLicenseClass.getListLicenseClasses();

            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                cbLicenseClasses.Items.Add(dt.Rows[i]["ClassName"]);
            }

        }

        void fillDefaultData()
        {
            cbLicenseClasses.SelectedIndex = 2;
            lblApplicationDate.Text = DateTime.Now.ToShortDateString().ToString();
            lblApplicationFees.Text = decimal.ToSingle(clsApplicationType.getApplicationTypeByID(1).applicationFees).ToString();
            lblCreatedBy.Text = clsGlobalSettings.currentUser.userName;
        }

        void fillData()
        {
            lblTitle.Text = "Update Local Driving License Application";
            cnrlPersonInfoWithFindBy1.loadPersonInfoANDDisableFilter(localApplication.personID);
            lblLocalApplicationID.Text = localApplication.localDrivingLicenseAppliactionID.ToString();
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(clsLicenseClass.getLicenseClassByID(localApplication.licenseClassID).className);
            lblApplicationDate.Text = localApplication.applicationDate.ToString();
            lblApplicationFees.Text = localApplication.paidFees.ToString();
            lblCreatedBy.Text = clsUser.getUserByID(localApplication.createdByUserID).userName;

        }

        private void frmAdd_UpdateLocalLicense_Load(object sender, EventArgs e)
        {
            fillcbLicenseClasses();
            
            
            if (nowMode == enMode.updateMode)
                fillData();
            else
                fillDefaultData();

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tbApplicationInfo"] && cnrlPersonInfoWithFindBy1.isFilled())
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tbApplicationInfo"];
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["tbPersonInfo"];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsPerson person = clsPerson.getPersonByID(cnrlPersonInfoWithFindBy1.gPersonID());
            clsLicenseClass licenseClass = clsLicenseClass.getLicenseClassByName(cbLicenseClasses.Text);

            if (!checkData(person, licenseClass))
                return;

            if (nowMode == enMode.addMode)
                fillLocalApplicationData(person, licenseClass);
            else
                localApplication.licenseClassID = licenseClass.licenseClassID;

            if (localApplication.save())
            {
                MessageBox.Show("Application Saved Successfully", "Add Application Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTitle.Text = "Update Local Driving License Application";
                nowMode = enMode.updateMode;
                lblLocalApplicationID.Text = localApplication.localDrivingLicenseAppliactionID.ToString();
                cnrlPersonInfoWithFindBy1.disableFilter();
            }
            else
                MessageBox.Show("Application NOT Saved Successfully", "Add Application Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmAdd_UpdateLocalLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlPersonInfoWithFindBy1.clearImage();
        }

      
    }
}