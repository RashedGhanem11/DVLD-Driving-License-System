using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bussiness_Layer
{
    public class clsApplicationType
    {
        public int applicationTypeID { get; set; }
        public string applicationTypeTitle { get; set; }
        public decimal applicationFees { get; set; }

        private bool _updateApplicationType()
        {
            return clsApplicationTypesDL.updateApplicationType(applicationTypeID, applicationTypeTitle, applicationFees);
        }

        private clsApplicationType(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            this.applicationTypeID = applicationTypeID;
            this.applicationTypeTitle = applicationTypeTitle;
            this.applicationFees = applicationFees;
        }

        static public DataTable getListApplicationTypes()
        {
            return clsApplicationTypesDL.getListApplicationTypes();
        }

        public bool save()
        {
            return _updateApplicationType();
        }

        static public clsApplicationType getApplicationTypeByID(int applicationTypeID)
        {
            string applicationTypeTitle = "";

            decimal applicationFees = -1;

            if (clsApplicationTypesDL.getApplicationTypeByID(applicationTypeID, ref applicationTypeTitle, ref applicationFees)) 
            {
                return new clsApplicationType(applicationTypeID, applicationTypeTitle, applicationFees);        
            }
            else
                return null;

        }

    }
}
