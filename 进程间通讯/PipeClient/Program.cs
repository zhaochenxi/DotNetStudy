using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(
                new AnonymousPipeClientStream(PipeDirection.In, args[0])))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine("Echo: {0}", line);
                }
            }
        }
    }
}
