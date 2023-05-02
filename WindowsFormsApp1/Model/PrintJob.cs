using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
	class PrintJob
	{
		public String guid { get; set; }
		public DateTime createdDate { get; set; }
		public String userGuid { get; set; }
		public String siteGuid { get; set; }
		public String description { get; set; }
		public EJOB_STATUS jobStatus { get; set; }
		public EJOB_TYPE jobType { get; set; }
		public String printerGuid { get; set; }
		public String printerIpAddress { get; set; }
		public String printerPort { get; set; }
		public String zpl { get; set; }
		public Int32 delaySpooler { get; set; }

		public enum EJOB_STATUS
		{
			NEW, STARTED, FINISHED, ERROR
		}

		public enum EJOB_TYPE
		{
			INVOICE, REPRINT, RECODE, ONCE, TEST
		}
	}
}
