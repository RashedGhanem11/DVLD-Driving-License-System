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
    public partial class frmLicenseDetails : Form
    {
        public frmLicenseDetails(int licenseID)
        {
            InitializeComponent();
            cnrlLicenseInfo1.loadLicenseInfo(licenseID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLicenseDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlLicenseInfo1.clearImage();
        }
    }
}
