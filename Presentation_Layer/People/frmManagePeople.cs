using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bussiness_Layer;
using System.IO;

namespace DVLD
{
    public partial class frmManagePeople : Form
    {       
        DataTable dataTable = new DataTable();
        DataView dataView = new DataView();

        public frmManagePeople()
        {
            InitializeComponent(); 
            
        }

        void changeColumnsWidth()
        {
            dgvPeopleList.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPeopleList.Columns["Gendor"].Width = 70;
            dgvPeopleList.Columns["Date Of Birth"].Width = 120;
        }

        void fillDGVPeople()
        {
            dataTable = clsPerson.getListPeopleShortDetailed();
            dataView = new DataView(dataTable);
            lblRecords.Text = dataView.Count.ToString();

            dgvPeopleList.DataSource = dataView;
            changeColumnsWidth();
        }

        void resetFiltering()
        {
            dataView.RowFilter = string.Empty;
            tbFilter.Visible = true;
            tbFilter.Text = string.Empty;
            lblRecords.Text = dataView.Count.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            fillDGVPeople();
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


            if (cbFilterBy.Text == "Person ID")
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
            if (cbFilterBy.Text == "Person ID") 
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
          
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvPeopleList.SelectedCells[0].RowIndex;

            int personID = (int)dgvPeopleList.Rows[selectedRow].Cells[0].Value;

            frmPersonDetails frm = new frmPersonDetails(personID);
            frm.ShowDialog();
            fillDGVPeople();
            cbFilterBy.SelectedIndex = 0;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvPeopleList.SelectedCells[0].RowIndex;

            int personID = (int)dgvPeopleList.Rows[selectedRow].Cells[0].Value;

            frmAdd_UpdatePerson frm = new frmAdd_UpdatePerson(personID);
            frm.ShowDialog();
            fillDGVPeople();
            cbFilterBy.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAdd_UpdatePerson frm = new frmAdd_UpdatePerson(-1);
            frm.ShowDialog();
            fillDGVPeople();
            cbFilterBy.SelectedIndex = 0;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvPeopleList.SelectedCells[0].RowIndex;

            int personID = (int)dgvPeopleList.Rows[selectedRow].Cells[0].Value;

            string picturePath = clsPerson.getPersonByID(personID).imagePath;

            if (clsPerson.deletePerson(personID)) 
            {
                try
                {
                    if (File.Exists(clsPerson.getPath(picturePath)))
                        File.Delete(clsPerson.getPath(picturePath));
                }
                catch { }

                MessageBox.Show("Person Deleted Successfully", "Person Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillDGVPeople();
                cbFilterBy.SelectedIndex = 0;
            }
            else
                MessageBox.Show("Person Deleted Failed", "Person Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

    }
}
