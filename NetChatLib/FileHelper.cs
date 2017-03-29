using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetChatLib
{
    public class FileHelper : MarshalByRefObject
    {
        /// <summary>
        /// 文件名和拓展名
        /// </summary>
        private static string fileName;
        /// <summary>
        /// 传输文件的路径
        /// 在显示传输进度条上要使用
        /// </summary>
        private static string filePath;
        /// <summary>
        /// 设置文件的路径
        /// </summary>
        /// <param name="path"></param>
        public void SetFilePath(string path)
        {
            filePath = path;
        }
        /// <summary>
        /// 获取文件的路径
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return filePath;
        }
        /// <summary>
        /// 设置文件名和拓展名
        /// </summary>
        public void SetFileName(string name)
        {
            fileName = name;
        }
        /// <summary>
        /// 获取文件名和拓展名
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return fileName;
        }
        /// <summary>
        /// 重写了基类GetType这个方法
        /// 反射时要用到
        /// </summary>
        /// <returns></returns>
        public new string GetType()
        {
            return "FileHelper";
        }


    }
}
