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
    public partial class frmManageDrivers : Form
    {
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();

        public frmManageDrivers()
        {
            InitializeComponent();
        }

        void changeColumnsWidth()
        {
            dgvDriversList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDriversList.Columns[4].Width = 150;
            dgvDriversList.Columns[5].Width = 120;
        }

        void changeColumnsNames()
        {
            dataTable.Columns[0].ColumnName = "Driver ID";
            dataTable.Columns[1].ColumnName = "Person ID";
            dataTable.Columns[2].ColumnName = "National No.";
            dataTable.Columns[3].ColumnName = "Full Name";
            dataTable.Columns[4].ColumnName = "Date";
            dataTable.Columns[5].ColumnName = "Active Licenses";
        }

        void fillDGVDrivers()
        {
            dataTable = clsDriver.getListDriversDetailed();
            changeColumnsNames();
            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();

            dgvDriversList.DataSource = dataView;
            changeColumnsWidth();
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            fillDGVDrivers();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetFiltering();

            if (cbFilterBy.Text == "None")
                tbFilter.Visible = false;
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == string.Empty)
            {
                resetFiltering();
                return;
            }

            if (cbFilterBy.Text == "Driver ID")
                dataView.RowFilter = "[Driver ID] = " + "'" + tbFilter.Text + "'";
            else if (cbFilterBy.Text == "Person ID")
                dataView.RowFilter = "[Person ID] = " + "'" + tbFilter.Text + "'";
            else
            {
                //dataView.RowFilter = "[National No.] like 'N3%'"; example

                string formatResults = "'" + tbFilter.Text + "%'";
                string formatFilterBy = "[" + cbFilterBy.Text + "]";
                string query = formatFilterBy + " like " + formatResults;
                dataView.RowFilter = query;
            }

            lblRecords.Text = dataView.Count.ToString();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Driver ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
