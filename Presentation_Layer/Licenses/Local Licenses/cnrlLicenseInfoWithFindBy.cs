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
    public partial class cnrlLicenseInfoWithFindBy : UserControl
    {
        public event Action<int> OnFindLicenseButtonClick;

        protected virtual void licenseSelected(int licenseID)
        {
            Action<int> handler = OnFindLicenseButtonClick;

            if (handler != null) 
            {
                handler(licenseID);
            }
        }

        public cnrlLicenseInfoWithFindBy()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFind.Text))
            {
                cnrlLicenseInfo1.loadLicenseInfo(-1);
                OnFindLicenseButtonClick(-1);
                return;
            }

            int licenseID = int.Parse(tbFind.Text);

            clsLicense license = clsLicense.getLicenseByID(licenseID);

            if (license != null)
                cnrlLicenseInfo1.loadLicenseInfo(licenseID);
            else
            {
                cnrlLicenseInfo1.loadLicenseInfo(-1);
                OnFindLicenseButtonClick(-1);
                return;
            }

            if(OnFindLicenseButtonClick != null)
                OnFindLicenseButtonClick(licenseID);
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        public void clearImage()
        {
            cnrlLicenseInfo1.clearImage();
        }

        public void disableFilter()
        {
            gbFilter.Enabled = false;
        }

        public void loadLicenseInfo(int licenseID)
        {
            tbFind.Text = licenseID.ToString();
            cnrlLicenseInfo1.loadLicenseInfo(licenseID);
        }
    }
}
