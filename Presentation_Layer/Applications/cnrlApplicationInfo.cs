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
    public partial class cnrlApplicationInfo : UserControl
    {

        int gPersonID = -1;

        public cnrlApplicationInfo()
        {
            InitializeComponent();
        }

        public void loadApplicationInfo(clsApplication app, string type)
        {
            lblID.Text = app.applicationID.ToString();
            lblStatus.Text = app.getStatus();
            lblFees.Text = decimal.ToSingle(app.paidFees).ToString();
            lblType.Text = type;
            lblApplicant.Text = clsPerson.getPersonFullName(app.personID);
            lblDate.Text = app.applicationDate.ToString();
            lblStatusDate.Text = app.lastStatusDate.ToString();
            lblCreatedBy.Text = clsUser.getUserByID(app.createdByUserID).userName;
            gPersonID = app.personID;
            linklblPersonInfo.Enabled = true;
        }

        private void linklblPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frmPerson = new frmPersonDetails(gPersonID);
            frmPerson.ShowDialog();
        }
    }
}
