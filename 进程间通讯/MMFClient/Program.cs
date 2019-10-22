using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;

namespace MMFClient
{
    /// <summary>
    /// 用于共享内存方式通信的 值类型 结构体
    /// </summary>
    public struct ServiceMsg
    {
        public int Id;
        public long NowTime;
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("请输入共享内存公用名(默认:testmap):");
            string shareName = Console.ReadLine();
            if (string.IsNullOrEmpty(shareName))
                shareName = "testmap";
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen(shareName, 1024000, MemoryMappedFileAccess.ReadWrite))
            {
                Mutex mutex = Mutex.OpenExisting("testmapmutex");
                mutex.WaitOne();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream(20, 0)) //注意这里的偏移量，server端已向MMF中写了5个四字节无符号整型数据，所以偏移5*4=20，将其跳过
                {
                    var writer = new BinaryWriter(stream);
                    for (int i = 5; i < 10; i++)
                    {
                        writer.Write(i);
                        Console.WriteLine("{0}位置写入流:{0}", i);
                    }
                }
                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(1024, 10240))
                {
                    int smSize = Marshal.SizeOf(typeof(ServiceMsg));
                    var sm = new ServiceMsg();
                    for (int i = 0; i < smSize * 5; i += smSize)
                    {
                        sm.Id = i;
                        sm.NowTime = DateTime.Now.Ticks;
                        //accessor.Read(i, out color);
                        accessor.Write(i, ref sm);
                        Console.WriteLine("{1}\tNowTime:{0}", new DateTime(sm.NowTime), sm.Id);
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(5000);

                mutex.ReleaseMutex();
            }
            Console.WriteLine("测试： 我是 即时通讯 - 状态服务 我启动啦！！！");
            Console.ReadKey();
        }
    }
}
