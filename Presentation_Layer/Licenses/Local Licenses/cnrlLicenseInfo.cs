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
    public partial class cnrlLicenseInfo : UserControl
    {
        public void emptyInfo()
        {
            lblClass.Text = "???";
            lblName.Text = "???";
            lblLicenseID.Text = "???";
            lblNationalNo.Text = "???";
            lblGendor.Text = "???";
            lblIssueReason.Text = "???";
            lblNotes.Text = "???";
            lblDateOfBirth.Text = "???";
            lblDriverID.Text = "???";
            lblIssueDate.Text = "???";
            lblExpirationDate.Text = "???";
            lblIsActive.Text = "???";
            lblIsDetained.Text = "???";
            pbPersonPicture.Image = Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
        }

        public void loadLicenseInfo(int licenseID)
        {
            if (licenseID == -1)
            {
                MessageBox.Show("License Not Found!", "License Found Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                emptyInfo();
                return;
            }

            clsLicense license = clsLicense.getLicenseByID(licenseID);
         
            int personID = clsDriver.getDriverByID(license.driverID).personID;
            
            clsPerson person = clsPerson.getPersonByID(personID);

            bool isDetained = clsDetainedLicense.isLicenseDetained(license.licenseID);

            lblClass.Text = clsLicenseClass.getLicenseClassByID(license.licenseClassID).className;
            lblName.Text = clsPerson.getPersonFullName(person.personID);
            lblLicenseID.Text = license.licenseID.ToString();
            lblNationalNo.Text = person.nationalNo;

            if (person.gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";
            
            switch(license.issueReason)
            {
                case 1:
                    lblIssueReason.Text = "First Time";
                    break;
                
                case 2:
                    lblIssueReason.Text = "Renew";
                    break;

                case 3:
                    lblIssueReason.Text = "Replaced For Lost";
                    break;

                case 4:
                    lblIssueReason.Text = "Replaced For Damaged";
                    break;
            }

            if (license.notes != "")
                lblNotes.Text = license.notes;
            else
                lblNotes.Text = "No Notes";

            lblDateOfBirth.Text = person.dateOfBirth.ToShortDateString();
            lblDriverID.Text = license.driverID.ToString();
            lblIssueDate.Text = license.issueDate.ToShortDateString();
            lblExpirationDate.Text = license.expirationDate.ToShortDateString();

            if (license.isActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

            if (isDetained)
                lblIsDetained.Text = "Yes";
            else
                lblIsDetained.Text = "No";

            if (person.imagePath != "")
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath(person.imagePath));
            else
            {
                if (person.gendor == 0)
                    pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
                else
                    pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Female 512"));

            }
        }

        public void clearImage()
        {
            pbPersonPicture.Image.Dispose();
            pbPersonPicture.Image = null;
        }

        public cnrlLicenseInfo()
        {
            InitializeComponent();
        }
    }
}
