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
    public partial class cnrlUserInfo : UserControl
    {
        public cnrlUserInfo()
        {
            InitializeComponent();
        }

        public void loadUserInfo(int userID)
        {
            clsUser user = clsUser.getUserByID(userID);

            cnrlPersonInfo1.loadPersonInfo(user.personID);

            lblUserID.Text = user.userID.ToString();
            lblUserName.Text = user.userName;

            if (user.isActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }

        public void clearImage()
        {
            cnrlPersonInfo1.clearImage();
        }
    }
}
