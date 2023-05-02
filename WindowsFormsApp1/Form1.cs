using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RawPrint;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO.Compression;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String zpl = textBox1.Text;
            IPrinter printer = new Printer();

            printer.PrintRawStream(@"ELGIN L42Pro", GenerateStreamFromString(zpl), @"docname");
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        //NuGet\Install-Package RawPrint -Version 0.5.0

        //Install-Package Microsoft.AspNet.WebApi.Client


        public static string Decompress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(msi, CompressionMode.Decompress))
                {
                    int count;

                    while ((count = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        memoryStream.Write(bytes, 0, count);
                    }
                }

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        static System.Net.Http.HttpClient client = new HttpClient();

        private const string URL = "http://192.168.15.11:5002/";
        private string urlSpooler = "track-service/api/spooler/detail/guid/fe9ad3fd-5d5e-460c-944f-8a1ddfef4bb7/tenant/fbaf61fe-7b18-4c89-9a62-a2a03bb8751c";
        private string urlJobs = "track-service/api/job/spooler/getfirst/##GUID##/tenant/fbaf61fe-7b18-4c89-9a62-a2a03bb8751c/";
        private string urlJobConclude = "track-service/api/job/concludeJob";

        private void button2_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("token", "78039900-41b5-4cf1-8399-d3b2863ed88478039900-41b5-4cf1-8399-d3b2863ed884");

            Spooler spooler = new Spooler();
            HttpResponseMessage response = client.GetAsync(urlSpooler).Result; 
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = (APIResponseSpooler)response.Content.ReadAsAsync<APIResponseSpooler>().Result;
                spooler = dataObjects.data;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            PrintJob printJob = new PrintJob();
            urlJobs = urlJobs.Replace("##GUID##", spooler.site.guid);
            HttpResponseMessage response2 = client.GetAsync(urlJobs).Result;
            StringBuilder sbZPL = new StringBuilder();
            if (response2.IsSuccessStatusCode)
            {
                var dataObjects = (APIResponseJob)response2.Content.ReadAsAsync<APIResponseJob>().Result;
                printJob = dataObjects.data;

                var valueBytes = System.Convert.FromBase64String(printJob.zpl);
                sbZPL.Append(Decompress(valueBytes));
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response2.StatusCode, response2.ReasonPhrase);
            }

            // impressora
            IPrinter printer = new Printer();

            printer.PrintRawStream(@"ELGIN L42Pro", GenerateStreamFromString(sbZPL.ToString()), @"docname");

            //conclude
            ConcludeJobRequest cjob = new ConcludeJobRequest();
            cjob.jobGuid = printJob.guid;
            cjob.tenantGuid = "fbaf61fe-7b18-4c89-9a62-a2a03bb8751c";
            cjob.status = ConcludeJobRequest.EJOB_STATUS.FINISHED;

            var sercjob = JsonConvert.SerializeObject(cjob);
            var content = new StringContent(sercjob, Encoding.UTF8, "application/json");

            HttpResponseMessage response3 = client.PostAsync(urlJobConclude, content).Result;
            if (response3.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response3.StatusCode, response3.ReasonPhrase);
            }

                client.Dispose();
        }
    }
}

