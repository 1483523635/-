using NetChatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>['/
        [STAThread]
        static void Main()
        {
            FileHelper filehelper = OpenClientRemoting();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fm_client(filehelper));
        }
        #region 连接到服务器远程的Remoting
        private static  FileHelper OpenClientRemoting()
        {
            NetChat chat = new NetChat();
            string url = "http://" + chat.RemoteIP1 + ":" +
                          chat.Port_Remoting.ToString() + "/"
                          + new FileHelper().GetType();
            Remoting_Helper remoting = new Remoting_Helper();
            return remoting.Client<FileHelper>(url);
        }
        #endregion
    }
}
