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
    public partial class frmManageUsers : Form
    {  
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();
        
        public frmManageUsers()
        {
            InitializeComponent();
        }

        void fillDGVUsers()
        {
            dataTable = clsUser.getListUsersDetailed();

            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();


            dgvUsersList.DataSource = dataView;

            dgvUsersList.Columns["Full Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }
   
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            fillDGVUsers();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetFiltering();

            if (cbFilterBy.Text == "Is Active")
            {
                tbFilter.Visible = false;
                cbIsActive.Visible = true;
            }
            else
            {
                tbFilter.Visible = true;
                cbIsActive.Visible = false;
                cbIsActive.SelectedIndex = 0;
            }

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


            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                dataView.RowFilter = "[" + cbFilterBy.Text + "] =" + "'" + tbFilter.Text + "'";
            else
            {
               // dataView.RowFilter = "[National No.] like 'N3%'"; example

                string formatResults = "'" + tbFilter.Text + "%'";
                string formatFilterBy = "[" + cbFilterBy.Text + "]";
                string query = formatFilterBy + " like " + formatResults;
                dataView.RowFilter = query;
            }

            lblRecords.Text = dataView.Count.ToString();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAdd_UpdateUser frm = new frmAdd_UpdateUser(-1);
            frm.ShowDialog();
            fillDGVUsers();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbIsActive.Text)
            {

                case "Yes":
                    dataView.RowFilter = "[Is Active] = " + "1";
                    break;

                case "No":
                    dataView.RowFilter = "[Is Active] = " + "0";
                    break;

                default:
                    dataView.RowFilter = string.Empty;
                    break;

            }
            lblRecords.Text = dataView.Count.ToString();


        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdd_UpdateUser frm = new frmAdd_UpdateUser(-1);
            frm.ShowDialog();
            fillDGVUsers();
            cbFilterBy.SelectedIndex = 0;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvUsersList.SelectedCells[0].RowIndex;

            int userID = (int)dgvUsersList.Rows[selectedRow].Cells[0].Value;

            frmAdd_UpdateUser frm = new frmAdd_UpdateUser(userID);
            frm.ShowDialog();
            fillDGVUsers();
            cbFilterBy.SelectedIndex = 0;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvUsersList.SelectedCells[0].RowIndex;

            int userID = (int)dgvUsersList.Rows[selectedRow].Cells[0].Value;

            if (clsUser.deleteUser(userID))
            {
                MessageBox.Show("User Deleted Successfully", "User Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillDGVUsers();
                cbFilterBy.SelectedIndex = 0;
            }
            else
                MessageBox.Show("User Deleted Failed", "User Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvUsersList.SelectedCells[0].RowIndex;

            int userID = (int)dgvUsersList.Rows[selectedRow].Cells[0].Value;

            frmUserDetails frm = new frmUserDetails(userID);
            frm.ShowDialog();
           
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvUsersList.SelectedCells[0].RowIndex;

            int userID = (int)dgvUsersList.Rows[selectedRow].Cells[0].Value;

            frmChangePassword frm = new frmChangePassword(userID);
            frm.ShowDialog();
        }
    }
}
