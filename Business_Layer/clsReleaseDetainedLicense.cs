using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsReleaseDetainedLicense : clsApplication
    {
        public int detainID {  get; set; }

        bool _releaseDetainedLicense()
        {
            return clsDetainedLicensesDL.releaseDetainedLicense(detainID, applicationDate, createdByUserID, applicationID);
        }

        public clsReleaseDetainedLicense(int detainID) : base(5)
        {
            this.detainID = detainID;
            this.applicationStatus = 3;
        }

        public bool save()
        {
            if (_addNewApplication())
            {
                nowMode = enMode.updateMode;
                return _releaseDetainedLicense();
            }
            return false;
        }

    }
}
