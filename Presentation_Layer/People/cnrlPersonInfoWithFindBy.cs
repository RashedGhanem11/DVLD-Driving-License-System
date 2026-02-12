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
    public partial class cnrlPersonInfoWithFindBy : UserControl
    {
        public cnrlPersonInfoWithFindBy()
        {
            InitializeComponent();
        }

        private void cnrlPersonInfoWithFindBy_Load(object sender, EventArgs e)
        {
            cbFindBy.SelectedIndex = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFind.Text))
            {
                cnrlPersonInfo1.loadPersonInfo(-1);
                return;
            }

            if (cbFindBy.Text == "Person ID")
            {
                int personID = int.Parse(tbFind.Text);

                cnrlPersonInfo1.loadPersonInfo(personID);

            }
            else
            {
                string nationalNo = tbFind.Text;

                cnrlPersonInfo1.loadPersonInfo(nationalNo);

            }
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFindBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAdd_UpdatePerson frm = new frmAdd_UpdatePerson(-1);
            frm.databack += DataBack_AddPerson;
            frm.ShowDialog();
        }

        private void DataBack_AddPerson(object sender, int personID)
        {
            cnrlPersonInfo1.loadPersonInfo(personID);

            if (cbFindBy.Text == "Person ID")
                tbFind.Text = personID.ToString();
            else
                tbFind.Text = (clsPerson.getPersonByID(personID)).nationalNo;
        }

        public void disableFilter()
        {
            gbFilter.Enabled = false;
        }

        public void loadPersonInfo(int personID)
        {
            cnrlPersonInfo1.loadPersonInfo(personID);
        }

        public void loadPersonInfoANDDisableFilter(int personID)
        {
            cnrlPersonInfo1.loadPersonInfo(personID);
            disableFilter();
        }

        public int gPersonID()
        {
            return cnrlPersonInfo1.gpersonID;
        }

        public bool isFilled()
        {
            return cnrlPersonInfo1.isFilled;
        }

        public void clearImage()
        {
            cnrlPersonInfo1.clearImage();
        }
    }
}
