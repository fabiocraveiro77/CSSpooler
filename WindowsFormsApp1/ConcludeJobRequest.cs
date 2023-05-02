using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class ConcludeJobRequest
    {
        public String jobGuid { get; set; }
        public EJOB_STATUS status { get; set; }
        public String tenantGuid { get; set; }

        public enum EJOB_STATUS
        {
            NEW, STARTED, FINISHED, ERROR
        }
    }
}
