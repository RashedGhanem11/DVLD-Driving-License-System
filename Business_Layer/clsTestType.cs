using clsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsTestType
    {
        public int testTypeID { get; set; }
        public string testTypeTitle { get; set; }
        public string testTypeDescription { get; set; }
        public decimal testTypeFees { get; set; }

        private bool _updateTestType()
        {
            return clsTestTypesDL.updateTestType(testTypeID, testTypeTitle, testTypeDescription, testTypeFees);
        }

        private clsTestType(int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            this.testTypeID = testTypeID;
            this.testTypeTitle = testTypeTitle;
            this.testTypeDescription = testTypeDescription;
            this.testTypeFees = testTypeFees;
        }

        static public DataTable getListTestTypes()
        {
            return clsTestTypesDL.getListTestTypes();
        }

        public bool save()
        {
            return _updateTestType();
        }

        static public clsTestType getTestTypeByID(int testTypeID)
        {
            string testTypeTitle = "", testTypeDescription = "";

            decimal testTypeFees = -1;

            if (clsTestTypesDL.getTestTypeByID(testTypeID, ref testTypeTitle, ref testTypeDescription, ref testTypeFees))
            {
                return new clsTestType(testTypeID, testTypeTitle, testTypeDescription, testTypeFees);
            }
            else
                return null;

        }
    }
}
