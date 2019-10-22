using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Process process = new Process();
            process.StartInfo.FileName = "ConsoleApp1.exe";
            //创建匿名管道流实例，
            using (AnonymousPipeServerStream pipeStream =
                new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            {
                //将句柄传递给子进程
                process.StartInfo.Arguments = pipeStream.GetClientHandleAsString();
                process.StartInfo.UseShellExecute = false;
                process.Start();
                //销毁子进程的客户端句柄？
                pipeStream.DisposeLocalCopyOfClientHandle();

                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    //向匿名管道中写入内容
                    sw.WriteLine(Console.ReadLine());
                }
            }
            process.WaitForExit();
            process.Close();

        }
    }
}
