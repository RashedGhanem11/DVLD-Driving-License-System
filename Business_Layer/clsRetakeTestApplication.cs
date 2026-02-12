using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer
{
    public class clsRetakeTestApplication : clsApplication
    {


        public clsRetakeTestApplication() : base(7)
        { 

        }

        public bool save()
        {
            return _addNewApplication();
        }

       static public clsRetakeTestApplication getRetakeTestApplicationByID(int retakeTestApplicationID)
        {
            clsRetakeTestApplication retakeTest = new clsRetakeTestApplication();

            if (retakeTest.getApplicationByID(retakeTestApplicationID)) 
                return retakeTest;
            else
                return null;
        }
    }
}
