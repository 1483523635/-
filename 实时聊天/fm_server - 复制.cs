using NetChatLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 实时聊天
{
    public partial class fm_server : Form
    {
        #region 字段的声明
        /// <summary>
        /// 这是用来存放聊天TCp的List 
        /// </summary>
        IList<TcpClient> ClientList = null;
        /// <summary>
        /// 用来存放文件传输TCp的List
        /// </summary>
        IList<TcpClient> ClientList_file = null;
        TcpListener listener = null;
        TcpClient remoteClient = null;
        TcpListener lisener_file = null;
        TcpClient remoteClient_file = null;
        NetChat chat;
        #endregion
        public fm_server()
        {
            InitializeComponent();
            //设置Combox的属性为只读
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            CheckForIllegalCrossThreadCalls = false;
            Load += fm_server_Load;
        }
      
        #region 初始化数据
        private void Init_Data()
        {
            chat = new NetChat();
            listener = new TcpListener(IPAddress.Parse(chat.RemoteIP1), chat.Port1);
            lisener_file = new TcpListener(IPAddress.Parse(chat.RemoteIP1), chat.Port_file);
            ClientList = new List<TcpClient>();
            ClientList_file = new List<TcpClient>();
        }
        #endregion
        void fm_server_Load(object sender, EventArgs e)
        {
            #region 初始化数据及套接字
            Init_Data();
            #endregion

            #region 文件接收服务
         
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        lisener_file.Start();
                        remoteClient_file = lisener_file.AcceptTcpClient();
                        ClientList_file.Add(remoteClient);
                    }
                    catch
                    {
                    }

                }
            })
            { IsBackground = true }.Start();
            #endregion

            #region 开启接收消息的线程
            new Thread(() =>
            {
                ShowMsg("开始监听");
                while (true)
                {
                    try
                    {
                        listener.Start();
                        //获取连接的远程主机的信息
                        remoteClient = listener.AcceptTcpClient();
                        ShowMsg("连接的远程主机： " + remoteClient.Client.RemoteEndPoint);
                        comboBox1.Items.Add(remoteClient.Client.RemoteEndPoint);
                        ClientList.Add(remoteClient);
                        RecMsg(remoteClient);
                    }
                    catch { }

                }
            }
           )
            {
                IsBackground = true
            }.Start();
            #endregion
        }

        #region 接收消息的线程
        void RecMsg(TcpClient client)
        {

            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        string msg = chat.RecMsg(client);
                        if (msg != null)
                            ShowMsg("收到的信息为" + client.Client.RemoteEndPoint + "(客户端)：" + msg);
                    }
                }
                catch
                {
                    Console.WriteLine("对方已经下线！");
                }

            })
            { IsBackground = true }.Start();

        }
        #endregion

        #region 向文本框中追加内容
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

        #region 发送消息按钮点击事件处理
        private void btn_send_Click(object sender, EventArgs e)
        {
            string msg = tb_send.Text;
            new Thread(() =>
            {
                SendMsg(GetComBoxVal(), msg);
                ShowMsg(msg);
            })
            { IsBackground = true }.Start();
        }
        #endregion

        #region 发送消息的处理
        /// <summary>
        /// 发送消息函数
        /// </summary>
        /// <param name="client">要发送的Tcp</param>
        /// <param name="text">消息内容</param>
        private void SendMsg(TcpClient client, string text)
        {

            loop1: if (!chat.SendMsg(client, text))
            {
                if (MessageBox.Show("消息发送失败！是否重新发送", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    goto loop1;
                }
            }
        }
        #endregion

        #region 接收文件按钮的点击处理
        private void btn_recFile_Click(object sender, EventArgs e)
        {
            ShowMsg("准备接收文件");
            string saveFilePath = getSaveFilePath();
            //下边这是原来的
            //  RecFile(GetComBoxVal(), getSaveFilePath());
            ThreadPool.QueueUserWorkItem(_ =>
            {
                RecFile(remoteClient_file, saveFilePath);
            });

         

        }
        #endregion

        #region 获取选择保存文件的路径

        /// <summary>
        /// 获取保存文件的路径
        /// </summary>
        /// <returns>路径</returns>
        private string getSaveFilePath()
        {
            string path = null;
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "选择文件保存路径";
                fbd.ShowNewFolderButton = true;
                loop: if (fbd.ShowDialog() == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                }
                if (path == null) goto loop;
            }
            return path;
        }
        #endregion

        #region 接收文件的处理
        /// <summary>
        /// 接收文件的处理
        /// </summary>
        /// <param name="c">连接的TCp</param>
        /// <param name="filepath">文件的路径</param>
        private void RecFile(TcpClient c, string filepath)
        {
            ShowMsg("开始接收文件");
            filepath = filepath + "\\" + new FileHelper().GetFileName();
            chat.recFile(c, filepath);
            ShowMsg("文件接收成功");
        }
        #endregion

        #region 获取Cmbox选中的值
        /// <summary>
        /// 获取Cmb选中的值
        /// </summary>
        /// <returns>TCP</returns>
        private TcpClient GetComBoxVal()
        {

            int i = comboBox1.SelectedIndex;
            return i == -1 ? ClientList[0] : ClientList[i];
        }
        #endregion

    }
}
