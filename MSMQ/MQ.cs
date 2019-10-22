using System;
using System.Messaging;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace demo
{
    public class MQzwh
    {
        private MessageQueue _mq = null;

        public MessageQueue MQ { get { return this._mq; } }

        #region 知识补充
        [Obsolete("我过时了")]
        [Conditional("Buged")]
        public void TestTest()
        {
            Console.WriteLine("我执行了");
        }
        #endregion

        protected void Test()
        {
            CreateNewQueue("MsgQueue");//创建一个消息队列  
            sendSimpleMsg();//每一个队列最好只发送和接收同一种格式的信息，不然不好转换格式。  
            receiveSimpleMsg();//  

            //sendComplexMsg();  
            //receiveComplexMsg();  
            MsgModel m = receiveComplexMsg<MsgModel>();
            Console.WriteLine(m.ToString());

        }

        public MQzwh(string name)
        {
            this.CreateNewQueue(name);
        }

        /// <summary>  
        /// 创建消息队列  
        /// </summary>  
        /// <param name="name">消息队列名称</param>  
        /// <returns></returns>  
        private void CreateNewQueue(string name)
        {
            string path = ".\\private$\\" + name;

            if (!MessageQueue.Exists(path))//检查是否已经存在同名的消息队列  
            {
                _mq = MessageQueue.Create(path);
                _mq.Label = path;
                Console.WriteLine("创建成功！");
            }
            else
            {
                if (_mq == null)
                {
                    _mq = new MessageQueue(path);
                }
                //System.Messaging.MessageQueue.Delete(".\\private$\\" + name);//删除一个消息队列  
                Console.WriteLine("已经存在");
            }
        }

        /// <summary>
        /// 发送简单消息
        /// </summary>
        public void sendSimpleMsg()
        {
            //实例化MessageQueue,并指向现有的一个名称为VideoQueue队列  
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //MQ.Send("消息测试", "测试消息");   
            System.Messaging.Message message = new System.Messaging.Message();
            message.Label = "我是消息lable";
            message.Body = "我是消息body我是消息body我是消息body我是消息body";
            this.MQ.Send(message);
        }
        /// <summary>
        /// 接收简单消息
        /// </summary>
        public void receiveSimpleMsg()
        {
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //调用MessageQueue的Receive方法接收消息  
            if (this.MQ.GetAllMessages().Length > 0)
            {
                Message message = this.MQ.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    //message.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "Message.Bussiness.VideoPath,Message" });//消息类型转换  
                    message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(string) });
                    string str = string.Format("接收消息成功！【lable: {0}】 【body: {1} 】 时间：{2}"
                                                , message.Label, message.Body.ToString(), DateTime.Now);
                    Console.WriteLine(str);
                }
            }
            else
            {
                Console.WriteLine("没有消息了！");
            }
        }
        /// <summary>
        /// 发送复杂消息
        /// </summary>
        public void sendComplexMsg()
        {
            //实例化MessageQueue,并指向现有的一个名称为VideoQueue队列  
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //MQ.Send("消息测试", "测试消息");  
            BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
            FileStream stream = default(FileStream);
            binFormat.Serialize(stream, new MsgModel("1", "消息1"));
            System.Messaging.Message message = new System.Messaging.Message(stream, new BinaryMessageFormatter());
            message.Label = "复杂消息lable";
            //message.Body = new MsgModel("1", "消息1");
            MQ.Send(message);

            Console.WriteLine("成功发送消息，" + DateTime.Now + "");
        }
        /// <summary>
        /// 接受复杂消息
        /// </summary>
        public void receiveComplexMsg()
        {
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //调用MessageQueue的Receive方法接收消息  
            if (MQ.GetAllMessages().Length > 0)
            {
                System.Messaging.Message message = MQ.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    //message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(MsgModel) });//消息类型转换  
                    message.Formatter = new BinaryMessageFormatter();
                    MsgModel msg = (MsgModel)message.Body;
                    Console.WriteLine(string.Format("接收消息成功,lable:{0},body:{1},{2}", message.Label, msg, DateTime.Now));
                }
            }
            else
            {
                Console.WriteLine("没有消息了！");
            }
        }
        public T receiveComplexMsg<T>()
        {
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //调用MessageQueue的Receive方法接收消息  
            if (MQ.GetAllMessages().Length > 0)
            {
                System.Messaging.Message message = MQ.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(T) });//消息类型转换  
                    T msg = (T)message.Body;
                    return msg;
                }
            }

            return default(T);
        }


        /// <summary>
        /// 发送一张图片
        /// </summary>
        public void sendImgMsg()
        {
            //实例化MessageQueue,并指向现有的一个名称为VideoQueue队列  
            //MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //MQ.Send("消息测试", "测试消息");  

            FileStream fs = new FileStream(@"help\windows消息队列查看.png", FileMode.Open);
            Image myImage = new Bitmap(fs); //Bitmap.FromFile(@"help\windows消息队列查看.png");
            System.Messaging.Message message =
                                new System.Messaging.Message(myImage, new BinaryMessageFormatter());
            message.Label = "我是一张图片";
            this.MQ.Send(message);

            //Console.WriteLine("成功发送消息，" + DateTime.Now + "");
        }
        /// <summary>
        /// 接受一张图片
        /// </summary>
        public void receiveImgMsg()
        {
            _mq.Formatter = new System.Messaging.BinaryMessageFormatter();
            var myMessage = _mq.Receive();
            Bitmap myImage = (Bitmap)myMessage.Body;
            myImage.Save("test.png");
        }

    }

    [Serializable]
    public class MsgModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public MsgModel() { }
        public MsgModel(string _id, string _Name)
        {
            id = _id;
            Name = _Name;
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(Name)) return "";
            return string.Format("id--{0},Name--{1}", id, Name);
        }
    }

}
