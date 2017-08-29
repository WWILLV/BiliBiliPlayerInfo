using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace BiliBiliPlayerInfo
{
    //public class Info
    //{
    //    public static void Main(String[] args)
    //    {
    //        GetInfo info = new GetInfo("");
    //        Console.WriteLine("cid:" + info.getCid());
    //        Console.WriteLine("aid:" + info.getAid());
    //        Console.WriteLine("Title:" + info.getTitle());
    //        Console.WriteLine("keywords:" + info.getKeywords());
    //        Console.WriteLine("description:" + info.getDescription());
    //        Console.WriteLine("auther:" + info.getAuther());
    //        Console.WriteLine("autherID:" + info.getAutherID());
    //        Console.WriteLine("cover:" + info.getCover());
    //        Console.WriteLine("H5Player:" + info.getH5Player());
    //        Console.ReadKey();
    //    }
    //}

    public class Page
    {
        /// <summary>
        /// 返回页面的HTML代码
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>HTML</returns>
        public static string GetHtml(string url)
        {
            string htmlCode;
            HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            webRequest.Timeout = 30000;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

            HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
            //获取目标网站的编码格式
            string contentype = webResponse.Headers["Content-Type"];
            Regex regex = new Regex("charset\\s*=\\s*[\\W]?\\s*([\\w-]+)", RegexOptions.IgnoreCase);
            if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                    {
                        //匹配编码格式
                        if (regex.IsMatch(contentype))
                        {
                            Encoding ending = Encoding.GetEncoding(regex.Match(contentype).Groups[1].Value.Trim());
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, ending))
                            {
                                htmlCode = sr.ReadToEnd();
                            }
                        }
                        else
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.UTF8))
                            {
                                htmlCode = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            else
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.Default))
                    {
                        htmlCode = sr.ReadToEnd();
                    }
                }
            }
            return htmlCode;
        }
    }

    public class GetInfo
    {

        private string av;
        private string url;
        private string strHTML;

        /// <summary>
        /// GetInfo
        /// </summary>
        /// <param name="av">av号</param>
        public GetInfo(string av)
        {
            if (checkav(ref av))
            {
                this.av = av;
                url = @"http://www.bilibili.com/av" + av;
                strHTML = Page.GetHtml(url);
            }
            else
                throw (new Exception("av号错误"));
        }

        /// <summary>
        /// 检查av号
        /// </summary>
        /// <param name="av"></param>
        /// <returns></returns>
        private bool checkav(ref string av)
        {
            String regex = @"^av[0-9]*$|^[0-9]*$";
            av = Regex.Match(av, regex).ToString().Replace("av", "");
            if (av == "")
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取Cid
        /// </summary>
        /// <returns></returns>
        public string getCid()
        {
            string regex = @"cid=[0-9]*";
            string cid = Regex.Match(strHTML, regex).ToString().Replace("cid=", "");
            return cid;
        }

        /// <summary>
        /// 获取Aid
        /// </summary>
        /// <returns></returns>
        public string getAid()
        {
            return av;
        }

        /// <summary>
        /// 获取播放页标题
        /// </summary>
        /// <returns></returns>
        public string getTitle()
        {
            string regex = @"<title>.+</title>";
            Match title = Regex.Match(strHTML, regex);
            return title.ToString().Replace("<title>", "").Replace("</title>", "");
        }

        /// <summary>
        /// 获取视频关键字
        /// </summary>
        /// <returns></returns>
        public string getKeywords()
        {
            string regex = @"<meta name=\""keywords\"" content=.*?\"" />";
            Match keywords = Regex.Match(strHTML, regex);
            return keywords.ToString().Replace("<meta name=\"keywords\" content=\"", "").Replace(@""" />", "");
        }

        /// <summary>
        /// 获取视频描述
        /// </summary>
        /// <returns></returns>
        public string getDescription()
        {
            string regex = @"<meta name=\""description\"" content=.*?\"" />";
            Match description = Regex.Match(strHTML, regex);
            return description.ToString().Replace("<meta name=\"description\" content=\"", "").Replace(@""" />", "");
        }

        /// <summary>
        /// 获取作者名
        /// </summary>
        /// <returns></returns>
        public string getAuther()
        {
            string regex = @"card=\"".+?\"" ";
            Match auther = Regex.Match(strHTML, regex);
            return auther.ToString().Replace("card=","").Replace("\"", "");
        }

        /// <summary>
        /// 获取作者ID
        /// </summary>
        /// <returns></returns>
        public string getAutherID()
        {
            string regex = @"mid=\"".+?\"" ";
            Match autherID = Regex.Match(strHTML, regex);
            return autherID.ToString().Replace("mid=", "").Replace("\"", "");
        }

        /// <summary>
        /// 获取封面
        /// </summary>
        /// <returns></returns>
        public string getCover()
        {
            string regex = @"<img src=\""//.*?\""";
            Match cover = Regex.Match(strHTML, regex);
            return cover.ToString().Replace(@"<img src=", "http:").Replace("\"", "");
        }

        /// <summary>
        /// 获取Html5播放器地址
        /// </summary>
        /// <returns></returns>
        public string getH5Player()
        {
            string url = "http://www.bilibili.com/html/html5player.html?aid=" + getAid() + "&cid=" + getCid();
            return url;
        }
    }
}
