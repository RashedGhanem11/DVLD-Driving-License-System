using Bussiness_Layer;
using DVLD.ApplicationTypesForms;
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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        void fillDGV()
        {
            dgvApplicationTypesList.DataSource = clsApplicationType.getListApplicationTypes();

            dgvApplicationTypesList.Columns[0].HeaderText = "ID";
            dgvApplicationTypesList.Columns[1].HeaderText = "Title";
            dgvApplicationTypesList.Columns[2].HeaderText = "Fees";

            dgvApplicationTypesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            lblRecords.Text = dgvApplicationTypesList.RowCount.ToString();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            fillDGV();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvApplicationTypesList.SelectedCells[0].RowIndex;

            int applicationTypeID = (int)dgvApplicationTypesList.Rows[selectedRow].Cells[0].Value;

            frmUpdateApplicationType frm = new frmUpdateApplicationType(applicationTypeID);
            frm.ShowDialog();
            fillDGV();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}