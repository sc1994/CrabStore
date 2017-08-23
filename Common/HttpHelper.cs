using System;
using System.IO;
using System.Net;
using System.Text;

namespace Common
{
    public class HttpHelper
    {
        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="data">发送的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string data)
        {
            try
            {
                //创建post请求
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                var payload = Encoding.UTF8.GetBytes(data);
                request.ContentLength = payload.Length;

                //发送post的请求
                var writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                //接受返回来的数据
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();

                // ReSharper disable once AssignNullToNotNullAttribute
                var reader = new StreamReader(stream, Encoding.UTF8);
                var value = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();

                return value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 发送GET请求
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            try
            {
                //创建Get请求
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                //接受返回来的数据
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                // ReSharper disable once AssignNullToNotNullAttribute
                var streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                var retString = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                response.Close();

                return retString;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
