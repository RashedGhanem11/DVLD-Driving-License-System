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
    public partial class frmUserDetails : Form
    {

        public frmUserDetails(int userID)
        {
            InitializeComponent();
            cnrlUserInfo1.loadUserInfo(userID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            cnrlUserInfo1.clearImage();
        }
    }
}
