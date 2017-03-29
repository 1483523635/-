using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace NetChatLib
{
    public class NetChat
    {
        #region 初始字段的声明


        /// <summary>
        /// 用于remoting监听的端口号
        /// </summary>
        private int port_Remoting = 12346;
        /// <summary>
        /// 用于接受文件的端口号
        /// </summary>
        private int port_file = 12344;
        /// <summary>
        /// 会话缓冲区的大小1M
        /// </summary>
        private static int MaxBufSize = 1024 * 1024;
        /// <summary>
        /// 文件传输缓冲区10M
        /// </summary>
        private static int MaxBufSizeFile = 1024 * 1024 * 10;
        /// <summary>
        /// 远程的Ip地址
        /// 从配置文件中读取
        /// </summary>
        private string RemoteIP = ConfigHelper.GetRemoteIP();
        /// <summary>
        /// 远程的端口
        /// </summary>
        private int Port = 12345;
        #endregion

        #region 字段的属性设置


        public string RemoteIP1 { get => RemoteIP; }
        /// <summary>
        /// 远程Ip的端口号
        /// </summary>
        public int Port1 { get => Port; }
        /// <summary>
        /// 用于接收文件的端口号
        /// </summary>
        public int Port_file { get => port_file; }
        /// <summary>
        /// 用于Remoting的端口号
        /// </summary>
        public int Port_Remoting { get => port_Remoting; }

        #endregion

        #region 类内的方法

        #region 接受消息的方法


        /// <summary>
        /// 接受消息的方法 
        /// 返回的字符串若为空
        /// 则可能为 原来的字符串为空串
        /// 或者出现了异常
        /// </summary>
        /// <param name="c">链接的Tcp套接字</param>
        /// <returns>接受到的字符串</returns>
        public string RecMsg(TcpClient c)
        {
            try
            {
                NetworkStream stream = c.GetStream();
                byte[] buf = new byte[MaxBufSize];
                int ReadSize = stream.Read(buf, 0, buf.Length);
                string msg = Encoding.Unicode.GetString(buf, 0, ReadSize);
                string md5 = msg.Split('~')[0];
                string text = msg.Split('~')[1];
                if (md5 == Encrypt.Upper32(text))
                    return ReadSize == 0 ? null : text;
                return "严重警告！消息被人更改过";
            }
            catch
            {

                return null;
            }


        }
        #endregion

        #region 发送消息的方法


        /// <summary>
        /// 发送消息
        /// 正常情况下返回true
        /// 异常情况下返回false
        /// </summary>
        /// <param name="c">Tcp套接字</param>
        /// <param name="text">发送的文本</param>
        /// <returns>是否发送成功</returns>
        public bool SendMsg(TcpClient c, string text)
        {
            try
            {
                NetworkStream stream = c.GetStream();
                byte[] buf = new byte[MaxBufSize];
                //这里进行了加密处理 貌似作用不大
                buf = Encoding.Unicode.GetBytes(Encrypt.Upper32(text) + "~" + text);
                stream.Write(buf, 0, buf.Length);
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region 接受文件的方法


        /// <summary>
        /// 用于接收文件
        /// </summary>
        /// <param name="c">Tcp套接字</param>
        /// <param name="path">传输文件路径</param>
        public void recFile(TcpClient c, string path)
        {
            
            if (CheckFileExist(path))
            {
                MessageBox.Show("文件已经存在", "提示", MessageBoxButtons.OK);
                return;
            }
            #region 这是同步代码（已通过测试）          
            NetworkStream stream = c.GetStream();
            FileStream filestream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            byte[] buf = new byte[MaxBufSizeFile];
            int size = 0;
            while (true)
            {
                size = stream.Read(buf, 0, buf.Length);
                if (size == 0) break;
                filestream.Write(buf, 0, size);
            }
            filestream.Close();
            stream.Close();
            #endregion

        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        private bool CheckFileExist(string path)
        {
            return File.Exists(path);
        }
        #endregion

        #region 发送文件的方法

        #region 一对一发送


        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fileName">文件路径</param>
        public void sendFile(TcpClient client, string fileName)
        {
            #region 这是同步代码（测试已通过）
            NetworkStream stream = client.GetStream();
            using (FileStream filestream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] filebuf = new byte[MaxBufSizeFile];
                int size = 0;
                while (true)
                {
                    size = filestream.Read(filebuf, 0, filebuf.Length);
                    stream.Write(filebuf, 0, size);
                    if (size == 0) break;
                }
            }
            stream.Close();
            #endregion                
        }
        #endregion
        #region 一对多发送
        public void SendFile_Much(List<TcpClient> list,string fileName)
        {
            foreach (TcpClient item in list)
            {
                sendFile(item, fileName);
            }
        }
        #endregion
        #endregion

        #region 获取发送文件路径
        /// <summary>
        /// 设置文件发送的路径
        /// </summary>
        /// <param name="filehelper">具有Remonting的FileHelper</param>
        /// <returns>文件路径</returns>
        public string GetSendFilePath(FileHelper filehelper)
        {
            string fileName = null;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {

                loop: if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                }
                if (fileName == null) goto loop;
                filehelper.SetFileName(ofd.SafeFileName);
            }
            return fileName;
        }
        #endregion

        #region 获取保存文件路径
        /// <summary>
        /// 获取保存文件的路径
        /// </summary>
        /// <returns>路径</returns>
        public string getSaveFilePath()
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

        #endregion
    }
}
