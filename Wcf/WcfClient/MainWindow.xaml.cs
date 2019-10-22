using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.FileService;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        FileServiceClient client;

        public MainWindow()
        {
            InitializeComponent();
            client = new FileServiceClient();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Fdialog = new OpenFileDialog();

            if (Fdialog.ShowDialog().Value)
            {

                using (Stream fs = new FileStream(Fdialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    string message;
                    this.filepath.Text = Fdialog.SafeFileName;
                    bool result = client.UpLoadFile(Fdialog.SafeFileName, fs.Length, fs, out message);

                    if (result == true)
                    {
                        MessageBox.Show("上传成功！");
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }
                }

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string filename = this.filename.Text;
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\client\";
            bool issuccess = false;
            string message = "";
            Stream filestream = new MemoryStream();
            long filesize = client.DownLoadFile(filename, out issuccess, out message, out filestream);

            if (issuccess)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] buffer = new byte[filesize];
                FileStream fs = new FileStream(path + filename, FileMode.Create, FileAccess.Write);
                int count = 0;
                while ((count = filestream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }

                //清空缓冲区
                fs.Flush();
                //关闭流
                fs.Close();
                MessageBox.Show("下载成功！");

            }
            else
            {

                MessageBox.Show(message);

            }
        }
    }
}
