using Bussiness_Layer;
using DVLD.ApplicationTypesForms;
using DVLD.TestTypesForms;
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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        void fillDGV()
        {
            dgvTestTypesList.DataSource = clsTestType.getListTestTypes();

            dgvTestTypesList.Columns[0].HeaderText = "ID";
            dgvTestTypesList.Columns[1].HeaderText = "Title";
            dgvTestTypesList.Columns[2].HeaderText = "Description";
            dgvTestTypesList.Columns[3].HeaderText = "Fees";

            dgvTestTypesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTestTypesList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            lblRecords.Text = dgvTestTypesList.RowCount.ToString();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            fillDGV();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvTestTypesList.SelectedCells[0].RowIndex;

            int testTypeID = (int)dgvTestTypesList.Rows[selectedRow].Cells[0].Value;

            frmUpdateTestType frm = new frmUpdateTestType(testTypeID);
            frm.ShowDialog();
            fillDGV();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}