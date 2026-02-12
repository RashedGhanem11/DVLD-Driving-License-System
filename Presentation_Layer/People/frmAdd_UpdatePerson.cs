using Bussiness_Layer;
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
using static System.Net.Mime.MediaTypeNames;


namespace DVLD
{
    public partial class frmAdd_UpdatePerson : Form
    {
        clsPerson person = new clsPerson();
        string prevPicturePath = "";
        private enum enMode { addMode, updateMode };
        private enMode nowMode = enMode.addMode;

        void selectDefultCountry(string defultCountry)
        {
            cbCountry.Text = defultCountry;
        }

        void fillCBCountries()
        {
            DataTable dtCountries = clsCountry.getListCountries();

            for (int i = 0; i < dtCountries.Rows.Count; i++)
            {
                cbCountry.Items.Add(dtCountries.Rows[i]["CountryName"].ToString());
            }

        }

        void fillData()
        {
            lblTitle.Text = "Update Person";
            lblPersonID.Text = person.personID.ToString();
            tbFirstName.Text = person.firstName;
            tbSecondName.Text = person.secondName;
            tbThirdName.Text = person.thirdName;
            tbLastName.Text = person.lastName;
            tbNationalNo.Text = person.nationalNo;
            dtpDateOfBirth.Text = person.dateOfBirth.ToString();
            tbPhone.Text = person.phone;
            tbEmail.Text = person.email;
            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.getCountryByID(person.nationalityCountryID).countryName);
            tbAddress.Text = person.address;

            if (person.gendor == 1)
            {
                rbFemale.Checked = true;
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Female 512"));
                pbPersonPicture.Tag = "Female";
            }
            else
                rbMale.Checked = true;

            if (person.imagePath != "")
            {
                string path = clsPerson.getPath(person.imagePath);

                pbPersonPicture.Image = System.Drawing.Image.FromFile(path);
                pbPersonPicture.Tag = "";

                openFileDialog1.FileName = path;

                lblRemove.Visible = true;
            }
            else
            {
               
            }

        }

        void fillPersonObject()
        {
            person.firstName = tbFirstName.Text;
            person.secondName = tbSecondName.Text;
            person.thirdName = tbThirdName.Text;
            person.lastName = tbLastName.Text;
            person.nationalNo = tbNationalNo.Text;
            person.phone = tbPhone.Text;
            person.email = tbEmail.Text;
            person.address = tbAddress.Text;
            person.dateOfBirth = dtpDateOfBirth.Value;
            person.nationalityCountryID = clsCountry.getCountryByCountryName(cbCountry.Text).countryID;

            if (rbMale.Checked)
                person.gendor = 0;
            else
                person.gendor = 1;

        }

        void clearPicture()
        {
            pbPersonPicture.Image.Dispose();
            pbPersonPicture.Image = null;
        }

        void loadDefultPicture()
        {
            if (pbPersonPicture.Tag.ToString() == "Male")
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
            else
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Female 512"));


        }

        void personPictureCases()
        {

            // if picture was empty tag will be "Male" OR "Female"
            if (pbPersonPicture.Tag.ToString() != "")
            {         

                if (prevPicturePath != "")
                {
                    clearPicture();
                    File.Delete(prevPicturePath);
                    loadDefultPicture();
                }   
                person.imagePath = "";
                prevPicturePath = "";
                openFileDialog1.FileName = "";

                return;
            }

            //did not change picture
            if (openFileDialog1.FileName == "")
                return;

            //same picture selected
            if (openFileDialog1.FileName == prevPicturePath)
                return;


            string guid = Guid.NewGuid().ToString();

            person.imagePath = guid;

            string newPicturePath = clsPerson.getPath(guid);
            
            File.Copy(openFileDialog1.FileName, newPicturePath);

            if (prevPicturePath != "")
                File.Delete(prevPicturePath);

            prevPicturePath = newPicturePath;
            openFileDialog1.FileName = "";
        }

        bool savePerson()
        {
            fillPersonObject();

            personPictureCases();

            return person.save();
        }

        public frmAdd_UpdatePerson(int personID)
        {
            if (personID != -1)
            {
                person = clsPerson.getPersonByID(personID);
                nowMode = enMode.updateMode;

                if (person.imagePath != "")
                    prevPicturePath = clsPerson.getPath(person.imagePath);
            }

            InitializeComponent();
            openFileDialog1.FileName = "";

        }

        private void frmAdd_UpdatePerson_Load(object sender, EventArgs e)
        {
            DateTime dateBefore18Years = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);

            dtpDateOfBirth.MaxDate = dateBefore18Years;

            pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));

            fillCBCountries();
            selectDefultCountry("Jordan");

            if (nowMode == enMode.updateMode)
                fillData();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if ((pbPersonPicture.Tag).ToString() == "Female")
            {
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
                pbPersonPicture.Tag = "Male";
            }

        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if ((pbPersonPicture.Tag).ToString() == "Male")
            {
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Female 512"));
                pbPersonPicture.Tag = "Female";
            }
        }

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (clsPerson.isPersonExists(tbNationalNo.Text))
            {
                errorProvider1.SetError(tbNationalNo, "NationalNo already exists!");
            }
            else
            {
                errorProvider1.SetError(tbNationalNo, "");
            }
        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbEmail.Text) || tbEmail.Text.EndsWith("@gmail.com"))
            {
                errorProvider1.SetError(tbEmail, "");
            }
            else
            {
                errorProvider1.SetError(tbEmail, "Invalid Format");
            }
        }

        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pbPersonPicture.Image.Dispose();
                    pbPersonPicture.Image = null;
                    pbPersonPicture.Image = System.Drawing.Image.FromFile(openFileDialog1.FileName);
                    lblRemove.Visible = true;
                    pbPersonPicture.Tag = "";

                }
            }
            catch
            {
                MessageBox.Show("Add person picture went wrong!", "Operation NOT Completed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                openFileDialog1.FileName = "";
            }

        }

        private void lblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this picture?", "Delete Picture",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            clearPicture();

            if (rbMale.Checked)
            {
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/Male 512"));
                pbPersonPicture.Tag = "Male";
            }
            else
            {
                pbPersonPicture.Image = System.Drawing.Image.FromFile(clsPerson.getPath("Defult Images/female 512"));
                pbPersonPicture.Tag = "Female";
            }

            lblRemove.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void DataBackEventHandler(object sender, int personID);

        public event DataBackEventHandler databack;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) || string.IsNullOrEmpty(tbSecondName.Text) ||
               string.IsNullOrEmpty(tbLastName.Text) || string.IsNullOrEmpty(tbNationalNo.Text) ||
                string.IsNullOrEmpty(tbPhone.Text) || string.IsNullOrEmpty(tbAddress.Text))
            {
                MessageBox.Show("Some Fields NOT Filled", "Save NOT Completed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (savePerson())
            {
                MessageBox.Show("Person Saved Successfully", "Add Person Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTitle.Text = "Update Person";    
                lblPersonID.Text = person.personID.ToString();
                databack?.Invoke(this, person.personID);
            }
            else
                MessageBox.Show("Person NOT Saved Successfully", "Add Person Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void frmAdd_UpdatePerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            clearPicture();
        }
    }
}
