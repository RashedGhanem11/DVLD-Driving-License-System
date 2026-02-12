using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsRenewLicense : clsApplication
    {
        public int oldLicenseID {  get; set; }
        public string notes {  get; set; }

        public clsRenewLicense(int oldLicenseID, string notes) : base(2)
        {
            this.oldLicenseID = oldLicenseID;
            this.notes = notes;
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
            newLicense.expirationDate = DateTime.Now.AddYears(clsLicenseClass.getLicenseClassByID(newLicense.licenseClassID).defaultValidityLength);
            newLicense.notes = this.notes;
            newLicense.paidFees = clsLicenseClass.getLicenseClassByID(newLicense.licenseClassID).classFees;
            newLicense.issueReason = 2;
            newLicense.createdByUserID = this.createdByUserID;

            return newLicense.save();
        }
    }
}
