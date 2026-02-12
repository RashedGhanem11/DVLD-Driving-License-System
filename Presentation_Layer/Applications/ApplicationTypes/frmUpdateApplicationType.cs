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

namespace DVLD.ApplicationTypesForms
{
    public partial class frmUpdateApplicationType : Form
    {
        clsApplicationType applicationType = null;

        void fillData()
        {
            lblID.Text = applicationType.applicationTypeID.ToString();
            tbTitle.Text = applicationType.applicationTypeTitle;
            tbFees.Text = applicationType.applicationFees.ToString();
        }

        void fillApplicationType()
        {
            applicationType.applicationTypeTitle = tbTitle.Text;
            applicationType.applicationFees = decimal.Parse(tbFees.Text);
        }

        public frmUpdateApplicationType(int ID)
        {
            applicationType = clsApplicationType.getApplicationTypeByID(ID);
            InitializeComponent();
        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            fillData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fillApplicationType();

            if (applicationType.save())
                MessageBox.Show("Application Type Saved Successfully", "Update Application Type Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Application Type Saved Failed", "Update Application Type Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
