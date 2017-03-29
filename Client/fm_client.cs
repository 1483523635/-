using NetChatLib;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class fm_client : Form
    {
        #region 字段的声明
        /// <summary>
        /// 用于聊天的类
        /// </summary>
        NetChat chat = null;
        /// <summary>
        /// 连接到服务器用于传输文件
        /// </summary>
        TcpClient CLientToServerFile = null;
        /// <summary>
        /// 连接到远程服务器的套接字用于会话
        /// </summary>
        TcpClient ClientToServerChat = null;
        private FileHelper filehelper;
        #endregion

        #region 系统的默认的窗体初始化

        #region 系统默认的构造函数（已过时）
        public fm_client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.Load += fm_client_Load;
            this.btn_sendFile.Click += Btn_sendFile_Click;
        }
        #endregion

        public fm_client(FileHelper filehelper)
        {

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Init_date();
            this.Load += fm_client_Load;
            this.btn_sendFile.Click += Btn_sendFile_Click;
            this.filehelper = filehelper;
        }

        #region 初始化数据


        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Init_date()
        {
            ClientToServerChat = new TcpClient();
            CLientToServerFile = new TcpClient();
            chat = new NetChat();

        }
        #endregion
        #endregion

        #region 窗体加载事件
        void fm_client_Load(object sender, EventArgs e)
        {
            #region 传输消息
            #region  初始化TCP并连接到服务器（用于连接会话） 

            ThreadPool.QueueUserWorkItem(_ =>
            {
                do
                {
                    try
                    {

                        ClientToServerChat.Connect(IPAddress.Parse(chat.RemoteIP1), chat.Port1);
                        ShowMsg("连接的远程终端为： " + ClientToServerChat.Client.RemoteEndPoint.ToString());
                    }
                    catch
                    {

                    }
                    Thread.Sleep(1000);
                } while (!ClientToServerChat.Connected);
            });

            #endregion

            #region 用于接受消息
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        string msg = chat.RecMsg(ClientToServerChat);
                        if (msg != null)
                            ShowMsg("收到" + ClientToServerChat.Client.RemoteEndPoint + "(服务器)的消息为： " + msg);

                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("出现异常{0}", e2.Message);
                    }
                }

            })
            { IsBackground = true }.Start();
            #endregion
            #endregion

            #region 传输文件

            #region 初始化Tcp 创建连接
            new Task(() =>
            {
                do
                {
                    try
                    {
                        CLientToServerFile.Connect(IPAddress.Parse(chat.RemoteIP1), chat.Port_file);
                    }
                    catch
                    {
                    }
                    Thread.Sleep(1000);
                } while (!CLientToServerFile.Connected);
            }).Start();

            #endregion

            #region 用于接收文件
            new Thread(() =>
            {        
                    ShowMsg("准备接收文件");
                    string filepath = @"D:\Users\Dinger\Desktop\接受文件测试文件夹 " + "\\" + "接收的文件.txt";
                    chat.recFile(ClientToServerChat, filepath);
                    ShowMsg("接受文件成功！");          
            })
            { IsBackground = true }.Start();
            #endregion

            #endregion



        }
        #endregion

        #region 清空按钮点击时的事件
        private void btn_clear_Click(object sender, EventArgs e)
        {
            tb_send.Text = "";
        }
        #endregion

        #region 发送消息按钮点击时处理的事件
        private void btn_send_Click(object sender, EventArgs e)
        {
            string msg = tb_send.Text;
            new Task(() =>
            {
                sendMsg(ClientToServerChat, msg);
                ShowMsg(ClientToServerChat.Client.LocalEndPoint + "(本机):" + msg);
            }).Start();
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息的事件
        /// </summary>
        /// <param name="client">连接到服务器的套接字</param>
        /// <param name="msg">消息内容</param>
        void sendMsg(TcpClient client, string msg)
        {
            loop1: if (!chat.SendMsg(client, msg))
            {
                if (MessageBox.Show("消息发送失败！是否重新发送", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    goto loop1;
                }
            }

        }
        #endregion

        #region 显示消息到文本框中
        void ShowMsg(string text)
        {
            if (string.IsNullOrWhiteSpace(tb_rec.Text))
            {
                tb_rec.AppendText(text);
                return;
            }
            tb_rec.AppendText("\r\n" + text);
        }
        #endregion

        #region 发送文件按钮事件处理
        private void Btn_sendFile_Click(object sender, EventArgs e)
        {
            string fileName = GetSendFilePath();
            ShowMsg("准备发送文件。    已通知对方接收！");
            NoticeServer("你有一个文件需要接收！");
            ThreadPool.QueueUserWorkItem(_ =>
              {
                  chat.sendFile(CLientToServerFile, fileName);
                  ShowMsg("文件发送成功！");
              });

        }
        #endregion

        #region 通知服务器接收文件的消息
        /// <summary>
        /// 通知服务器接收文件的消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        private void NoticeServer(string msg)
        {
            sendMsg(ClientToServerChat, msg);
        }
        #endregion

        #region 获取要发送文件的路径

        private string GetSendFilePath()
        {
            //这里进行了重构 不清楚多少地方用了这个方法
            return chat.GetSendFilePath(filehelper);
        }
        #endregion


    }
}
