using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class SpoolerConfig
    {
		public String apiAddress { get; set; }
		public String apiSpoolerGuid { get; set; }
		public String apiTenantGuid { get; set; }
		public String apiToken { get; set; }
		public String apiUrlJobList { get; set; }
		public String apiUrlJobListQueued { get; set; }
		public String apiUrlSpoolerId { get; set; }
		public String apiUrlConcludejobs { get; set; }
		public Boolean hubstringsEnable { get; set; }
		public String hubstringsApplicationName { get; set; }
		public String hubstringsServerUrl { get; set; }
		public String hubstringsHostName { get; set; }
	}
}
