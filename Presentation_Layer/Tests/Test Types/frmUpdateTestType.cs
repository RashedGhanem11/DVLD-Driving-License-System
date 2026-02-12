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

namespace DVLD.TestTypesForms
{
    public partial class frmUpdateTestType : Form
    {
        clsTestType testType = null;

        void fillData()
        {
            lblID.Text = testType.testTypeID.ToString();
            tbTitle.Text = testType.testTypeTitle;
            tbDescription.Text = testType.testTypeDescription;
            tbFees.Text = testType.testTypeFees.ToString();
        }

        void fillTestType()
        {
            testType.testTypeTitle = tbTitle.Text;
            testType.testTypeDescription = tbDescription.Text;
            testType.testTypeFees = decimal.Parse(tbFees.Text);
        }

        public frmUpdateTestType(int ID)
        {
            InitializeComponent();
            testType = clsTestType.getTestTypeByID(ID);
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            fillData();
        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fillTestType();

            if (testType.save())
                MessageBox.Show("Test Type Saved Successfully", "Update Test Type Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Test Type Saved Failed", "Update Test Type Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
