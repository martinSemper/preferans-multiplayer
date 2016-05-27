using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://*:8080";

            using (WebApp.Start(url))
            {
                Console.WriteLine("Sever running on {0}", url);
                Console.ReadLine();
            }            
        }
    }
}
