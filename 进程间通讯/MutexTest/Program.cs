using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Runtime.InteropServices;

namespace MutexTest
{
    class Program
    {
        static Mutex gM1;
        static Mutex gM2;
        const int ITERS = 100;
        static AutoResetEvent Event1 = new AutoResetEvent(false);
        static AutoResetEvent Event2 = new AutoResetEvent(false);
        static AutoResetEvent Event3 = new AutoResetEvent(false);
        static AutoResetEvent Event4 = new AutoResetEvent(false);

        public static void Main(String[] args)
        {
            Console.WriteLine("Mutex Sample ");
            //创建一个Mutex对象，并且命名为MyMutex  
            //可在其他进程用Mutex mutex = Mutex.OpenExisting("testmapmutex");打开这个Mutex，进而实现进程间的同步
            gM1 = new Mutex(true, "MyMutex");
            //创建一个未命名的Mutex 对象.  
            gM2 = new Mutex(true);
            Console.WriteLine(" - Main Owns gM1 and gM2");

            AutoResetEvent[] evs = new AutoResetEvent[4];
            evs[0] = Event1; //为后面的线程t1,t2,t3,t4定义AutoResetEvent对象  
            evs[1] = Event2;

            Program tm = new Program();
            Thread t1 = new Thread(new ThreadStart(tm.t1Start));
            Thread t2 = new Thread(new ThreadStart(tm.t2Start));
            t1.Start();// 使用gM1.WaitOne();方法等待gM1的释放   
            t2.Start();// 使用gM2.WaitOne();方法等待gM2的释放  

            Thread.Sleep(2000);
            Console.WriteLine(" - Main releases gM1");
            gM1.ReleaseMutex(); //线程t1结束条件满足  

            Thread.Sleep(1000);
            Console.WriteLine(" - Main releases gM2");
            gM2.ReleaseMutex(); //线程t2结束条件满足  

            //等待所有线程结束  
            WaitHandle.WaitAll(evs);
            Console.WriteLine(" Mutex Sample");
            Console.ReadLine();
        }

        public void t1Start()
        {
            Console.WriteLine("t1Start started, gM1.WaitOne();");
            gM1.WaitOne();//等待gM1释放  
            Thread.Sleep(2000);
            Console.WriteLine("t1Start finished, gM1.WaitOne(); satisfied");
            Event1.Set(); //线程结束，将Event1设置为有信号状态  
        }
        public void t2Start()
        {
            Console.WriteLine("t2Start started, gM2.WaitOne( )");
            gM2.WaitOne();//等待gM2被释放 
            Console.WriteLine("t2Start finished, gM2.WaitOne( ) satisfied");
            Event2.Set();//线程结束，将Event2设置为有信号状态  
        }
    }
}
