using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace NamedPipeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NamedPipeServerStream pipeServer =
           new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1))
            {
                try
                {
                    pipeServer.WaitForConnection();
                    pipeServer.ReadMode = PipeTransmissionMode.Byte;
                    while (true)
                    {
                        using (StreamReader sr = new StreamReader(pipeServer))
                        {
                            string con = sr.ReadToEnd();
                            Console.WriteLine(con);
                        }
                        //using (StreamWriter sw = new StreamWriter(pipeServer))
                        //{
                        //    sw.WriteLine("server=>client");
                        //    sw.Flush();
                        //}
                    }
                }
                catch (IOException e)
                {
                    throw e;
                }
            }
        }
    }
}
