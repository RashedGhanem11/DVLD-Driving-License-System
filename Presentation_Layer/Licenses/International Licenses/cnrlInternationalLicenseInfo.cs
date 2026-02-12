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
    public partial class cnrlInternationalLicenseInfo : UserControl
    {
        public void loadLicenseInfo(int internationalLicenseID)
        {
            clsInternationalLicense IntLicense = clsInternationalLicense.getInternationalLicenseByID(internationalLicenseID);

            clsPerson person = clsPerson.getPersonByID(IntLicense.personID);
         
            lblName.Text = clsPerson.getPersonFullName(person.personID);
            lblInternationalLicenseID.Text = IntLicense.internationalLicenseID.ToString();
            lblLicenseID.Text = IntLicense.issuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = person.nationalNo;

            if (person.gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            lblApplicationID.Text = IntLicense.applicationID.ToString();
            lblDateOfBirth.Text = person.dateOfBirth.ToShortDateString();
            lblDriverID.Text = IntLicense.driverID.ToString();
            lblIssueDate.Text = IntLicense.applicationDate.ToShortDateString();
            lblExpirationDate.Text = IntLicense.expirationDate.ToShortDateString();

            if (IntLicense.isActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

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

        public cnrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }
    }
}
