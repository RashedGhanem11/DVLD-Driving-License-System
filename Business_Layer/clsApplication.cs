using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public abstract class clsApplication
    {
        public int applicationID { get; set; }
        public int personID { get; set; }
        public DateTime applicationDate { get; set; }
        public int applicationTypeID { get; set; }
        public int applicationStatus { get; set; }
        public DateTime lastStatusDate { get; set; }
        public decimal paidFees { get; set; }
        public int createdByUserID { get; set; }
        protected enum enMode { addMode,updateMode }
        protected enMode nowMode { get; set; }

        protected bool _addNewApplication()
        {
            this.applicationID = clsApplicationsDL.addNewApplication(this.personID, this.applicationDate, this.applicationTypeID,
                this.applicationStatus, this.lastStatusDate, this.paidFees, this.createdByUserID);

            return applicationID != -1;
        }

        public clsApplication(int applicationTypeID)
        {
            this.applicationID = -1;
            this.personID = -1;
            this.applicationDate = DateTime.Now;
            this.applicationTypeID = applicationTypeID;
            this.applicationStatus = 1;
            this.lastStatusDate = DateTime.Now;
            this.paidFees = clsApplicationType.getApplicationTypeByID(applicationTypeID).applicationFees;
            this.createdByUserID = -1;
            nowMode = enMode.addMode;
        }

        public clsApplication()
        {
            this.applicationID = -1;
            this.personID = -1;
            this.applicationDate = DateTime.Now;
            this.applicationTypeID = -1;
            this.applicationStatus = -1;
            this.lastStatusDate = DateTime.Now;
            this.paidFees = 0;
            this.createdByUserID = -1;
            nowMode = enMode.addMode;
        }

        protected bool getApplicationByID(int applicationID)
        {
            int personID = -1, applicationTypeID = -1, applicationStatus = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            if (clsApplicationsDL.getApplicationByID(applicationID, ref personID, ref applicationDate, ref applicationTypeID,
                ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                this.applicationID = applicationID;
                this.personID = personID;
                this.applicationTypeID = applicationTypeID;
                this.applicationStatus = applicationStatus;
                this.createdByUserID = createdByUserID;
                this.applicationDate = applicationDate;
                this.lastStatusDate = lastStatusDate;
                this.paidFees = paidFees;
                nowMode = enMode.updateMode;
                return true;
            }
            return false;
        }

        public string getStatus()
        {
            switch(applicationStatus)
            {
                case 1:
                    return "New";
                case 2:
                    return "Cancelled";
                case 3:
                    return "Completed";
                default:
                    return "UnKnown";                   

            }
        }

        static protected bool deleteApplication(int applicationID)
        {
            return clsApplicationsDL.deleteApplication(applicationID);
        }

        static public bool cancelApplication(int applicationID)
        {
            return clsApplicationsDL.cancelApplication(applicationID);
        }

        static public bool completeApplication(int applicationID)
        {
            return clsApplicationsDL.completeApplication(applicationID);
        }
    }
   
}