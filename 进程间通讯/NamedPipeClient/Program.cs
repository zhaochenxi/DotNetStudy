using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;

namespace NamedPipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (NamedPipeClientStream pipeClient =
              new NamedPipeClientStream("localhost", "testpipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None))
                {
                    pipeClient.Connect();
                    while (true)
                    {
                        using (StreamWriter sw = new StreamWriter(pipeClient))
                        {
                            sw.WriteLine("client=>server");
                            sw.Flush();
                        }
                        //using (StreamReader sr = new StreamReader(pipeClient))
                        //{
                        //    string con = sr.ReadToEnd();
                        //    Console.WriteLine(con);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
