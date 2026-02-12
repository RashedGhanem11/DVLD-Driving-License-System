using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsDriver
    {
        public int driverID {  get; set; }
        public int personID { get; set; }
        public int createdByUserID {  get; set; }
        public DateTime createdDate { get; set; }

        private bool _addNewDriver()
        {
            driverID = clsDriversDL.addNewDriver(personID, createdByUserID, createdDate);

            return driverID != -1;
        }

        public clsDriver() 
        {
            this.driverID = -1;
            this.personID = -1;
            this.createdByUserID = -1;
            this.createdDate = DateTime.Now;
        }

        private clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.driverID = driverID;
            this.personID = personID;
            this.createdByUserID = createdByUserID;
            this.createdDate = createdDate;
        }

        static public bool isDriverExists(int personID)
        {
            return clsDriversDL.isDriverExists(personID);
        }

        static public clsDriver getDriverByID(int driverID)
        {
            int personID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriversDL.getDriverByID(driverID, ref personID, ref createdByUserID, ref createdDate))
                return new clsDriver(driverID, personID, createdByUserID, createdDate);

            return null;
        }

        static public clsDriver getDriverByPersonID(int personID)
        {
            int driverID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriversDL.getDriverByPersonID(personID, ref driverID, ref createdByUserID, ref createdDate))
                return new clsDriver(driverID, personID, createdByUserID, createdDate);

            return null;
        }

        public bool save()
        {
            return _addNewDriver();
        }

        static public DataTable getListDriversDetailed()
        {
            return clsDriversDL.getListDriversDetailed();
        }
    }
}
