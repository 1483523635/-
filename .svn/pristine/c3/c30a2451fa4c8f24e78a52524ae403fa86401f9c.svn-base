using System.Web.Security;
namespace NetChatLib
{
    /// <summary>
    /// 加密算法类
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 这是一个MD5加密算法（大写）
        /// </summary>
        /// <param name="s">要转化的字符串</param>
        /// <returns></returns>
        public static string Upper32(string s)
        {
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5");
            if (hashPasswordForStoringInConfigFile != null)
                s = hashPasswordForStoringInConfigFile.ToString();
            return s.ToUpper();
        }
    }
}
