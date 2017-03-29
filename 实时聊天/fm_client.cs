using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace 实时聊天
{
    public partial class fm_client : Form
    {
        /// <summary>
        /// 远程的IP地址
        /// </summary>
        static string remoteIp = "192.168.0.127";
        /// <summary>
        /// 远程的端口号
        /// </summary>
        static int RemotePort = 12345;
        public fm_client()
        {
            InitializeComponent();
            this.Load += fm_client_Load;
        }

        void fm_client_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                IPAddress ip = IPAddress.Parse(remoteIp);
                TcpClient client = new TcpClient();
                while (client.Connected==false)
                {
                    try
                    {
                        client.Connect(ip, RemotePort);
                    }
                    catch (Exception e1)
                    {
                        Console.WriteLine("服务器连接失败！{0}", e1);
                    }
                }
            
               tb_rec.AppendText("\r\n"+"连接的远程服务器为： "+client.Client.RemoteEndPoint);
            }
        );
            t.IsBackground = true;
            t.Start();
        }
    }
}
