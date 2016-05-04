using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using(Microsoft.Owin.Hosting.WebApp.Start(url))
            {
                Console.WriteLine("Sever running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}
