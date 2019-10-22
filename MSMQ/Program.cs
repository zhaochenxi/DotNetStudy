#define Buged
using System;
using System.Linq;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //先安装windows消息队列服务:打开或关闭Windows功能
            //http://blog.csdn.net/pukuimin1226/article/details/19030691

            MQzwh mq = new MQzwh("zwhQueue");

            System.Messaging.MessageQueue mq_temp = mq.MQ;

            mq_temp.Purge(); //清空消息

            for (int i = 0; i < 1; i++)
            {
                mq.sendSimpleMsg();
            }
            for (int i = 0; i < 5; i++)
            {
                mq.receiveSimpleMsg();
            }

            mq.sendComplexMsg();
            mq.receiveComplexMsg();

            Console.WriteLine("mq 消息数目：" + mq_temp.GetAllMessages().Count());

            //Console.WriteLine("mq 消息数目：" + mq_temp.GetAllMessages().Count());

            Console.WriteLine("-------------------\nok");
            Console.ReadKey();

          
        }
    }
}
