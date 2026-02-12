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
    public partial class cnrlPersonInfo : UserControl
    {
        public int gpersonID = -1;
        clsPerson person = new clsPerson();
        public bool isFilled = false;

        public void emptyInfo()
        {
            isFilled = false;
            gpersonID = -1;
            linklblEditPerson.Enabled = false;
            lblPersonID.Text = "???";
            lblName.Text = "???";
            lblNationalNo.Text = "???";
            lblGendor.Text = "???";
            lblEmail.Text = "???";
            lblAddress.Text = "???";
            lblPhone.Text = "???";
            lblCountry.Text = "???";
            lblDateOfBirth.Text = "???";
            pbPersonPicture.Image = Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
        }
    
        private void _loadPersonInfo(clsPerson person)
        {

            gpersonID = person.personID;

            lblPersonID.Text = person.personID.ToString();

            if (person.thirdName != "")
                lblName.Text = person.firstName + " " + person.secondName + " " + person.thirdName + " " + person.lastName;
            else
                lblName.Text = person.firstName + " " + person.secondName + " " + person.lastName;

            lblNationalNo.Text = person.nationalNo;

            if (person.gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            lblEmail.Text = person.email;
            lblAddress.Text = person.address;
            lblDateOfBirth.Text = person.dateOfBirth.ToShortDateString();
            lblPhone.Text = person.phone;
            lblCountry.Text = clsCountry.getCountryByID(person.nationalityCountryID).countryName;

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

        private void _checkPersonExists(clsPerson person)
        {
            if (person == null) 
            {
                MessageBox.Show("Person Not Found!", "Person Found Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                emptyInfo();
                return;
            }

            isFilled = true;
            linklblEditPerson.Enabled = true;
            _loadPersonInfo(person);
      
        }

        public cnrlPersonInfo()
        {
            InitializeComponent();
        }

        public void loadPersonInfo(string nationalNo)
        {
            person = clsPerson.getPersonByNationalNo(nationalNo);

            _checkPersonExists(person);
        }

        public void loadPersonInfo(int personID)
        {
            person = clsPerson.getPersonByID(personID);

            _checkPersonExists(person);
        }

        public void clearImage()
        {
            pbPersonPicture.Image.Dispose();
            pbPersonPicture.Image = null;
        }

        private void linklblEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Make the picture empty in case the user delete the picture path
            clearImage();

            frmAdd_UpdatePerson frm = new frmAdd_UpdatePerson(gpersonID);
            frm.ShowDialog();
            loadPersonInfo(gpersonID);
        }
    }
}
