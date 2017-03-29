using System;
using System.IO;

namespace NetChatLib
{
    /// <summary>
    /// 显示文件传输的进度类
    /// </summary>
    public  class ShowSchedule
    {
        private FileInfo info;
        private long fileBytes;
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public ShowSchedule(string filePath)
        {
            info = new FileInfo(filePath);
            fileBytes = info.Length;
        }
      /// <summary>
      /// 获取发送的百分比
      /// </summary>
      /// <param name="sent">已经发送的长度</param>
      /// <returns>百分比</returns>
        public  string GetPercent(int sent)
        {
            decimal allbytes = Convert.ToDecimal(fileBytes);
            decimal currentSend = Convert.ToDecimal(sent);
            decimal percent = (currentSend / fileBytes) * 100;
            percent = Math.Round(percent, 1);
            if (percent.ToString() == "100.0")
            {
                return "100";
            }
            return percent.ToString();
        }
    }
}
