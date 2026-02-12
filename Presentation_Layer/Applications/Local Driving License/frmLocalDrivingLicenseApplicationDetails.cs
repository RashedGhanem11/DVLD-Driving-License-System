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
    public partial class frmLocalDrivingLicenseApplicationDetails : Form
    {

        public frmLocalDrivingLicenseApplicationDetails(int localAppID)
        {
            InitializeComponent();
            cnrlLocalDrivingLicenseApplicationInfo1.loadLocalDrivingLicenseApplicationInfo(localAppID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
