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
    public partial class frmInternationalLicenseDetails : Form
    {
        public frmInternationalLicenseDetails(int internationalLicenseID)
        {
            InitializeComponent();
            cnrlInternationalLicenseInfo1.loadLicenseInfo(internationalLicenseID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInternationalLicenseDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlInternationalLicenseInfo1.clearImage();
        }
    }
}
