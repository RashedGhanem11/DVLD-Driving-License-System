using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsReplacementForDamagedLicense : clsApplication
    {
        public int oldLicenseID { get; set; }

        public clsReplacementForDamagedLicense(int oldLicenseID) : base(4)
        {
            this.oldLicenseID = oldLicenseID;
            this.applicationStatus = 3;
        }

        public bool save()
        {
            clsLicense oldLicense = clsLicense.getLicenseByID(oldLicenseID);

            if (!clsLicense.deactiveLicense(oldLicenseID))
                return false;

            if (!_addNewApplication())
                return false;

            clsLicense newLicense = new clsLicense();

            newLicense.applicationID = this.applicationID;
            newLicense.driverID = oldLicense.driverID;
            newLicense.licenseClassID = oldLicense.licenseClassID;
            newLicense.issueDate = DateTime.Now;
            newLicense.expirationDate = oldLicense.expirationDate;
            newLicense.notes = "";
            newLicense.paidFees = 0;
            newLicense.issueReason = 4;
            newLicense.createdByUserID = this.createdByUserID;

            return newLicense.save();
        }
    }
}
