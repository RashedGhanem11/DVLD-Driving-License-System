using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsDetainedLicense
    {
        public int detainID {  get; set; }
        public int licenseID { get; set; }
        public DateTime detainDate { get; set; }
        public decimal fineFees {  get; set; }
        public int createdByUserID {  get; set; }

        private bool _detainLicense()
        {
            this.detainID = clsDetainedLicensesDL.detainLicense(licenseID, detainDate, fineFees, createdByUserID);

            return detainID != -1;
        }

        public clsDetainedLicense()
        {
            this.detainID = -1;
            this.licenseID = -1;
            this.detainDate = DateTime.Now;
            this.fineFees = -1;
            this.createdByUserID = -1;
        }

        private clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees,
            int createdByUserID)
        {
            this.detainID = detainID;
            this.licenseID = licenseID;
            this.detainDate = detainDate;
            this.fineFees = fineFees;
            this.createdByUserID = createdByUserID;
        }

        static public bool isLicenseDetained(int licenseID)
        {
            return clsDetainedLicensesDL.isLicenseDetained(licenseID);
        }

        static public clsDetainedLicense getDetainedLicenseByLicenseID(int licenseID)
        {
            int detainID = -1, createdByUserID = -1;

            DateTime detainDate = DateTime.Now;
            decimal fineFees = -1;

            if (clsDetainedLicensesDL.getDetainedLicenseByLicenseID(licenseID, ref detainID, ref detainDate,
                ref fineFees, ref createdByUserID)) 
            {
                return new clsDetainedLicense(detainID, licenseID, detainDate, fineFees, createdByUserID);
            }
            else
                return null;

        }

        public bool save()
        {
            return _detainLicense();
        }

        static public DataTable getListDetainedLicenses()
        {
            return clsDetainedLicensesDL.getListDetainedLicenses();
        }
    }
}
