using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Common
{
    public static class ConvertHelper
    {
        public static bool IsNullOrEmpty(this string s)
        {
            if (s == null) return true;
            if (s.Trim() == string.Empty) return true;
            return false;
        }

        /// <summary>
        /// 当字符串为空时, 展示的替代字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="show">替代字符(默认为未知)</param>
        /// <returns></returns>
        public static string ShowNullOrEmpty(this string s, string show = "未知")
        {
            if (s == null) return show;
            if (s.Trim() == string.Empty) return show;
            return s;
        }

        /// <summary>
        /// 当枚举为空时, 展示的替代字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="show">替代字符(默认为未知)</param>
        /// <returns></returns>
        public static string ShowNullOrEmpty(this Enum s, string show = "未知")
        {
            if (s == null) return show;
            return s.ToString();
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        public static T XmlToObject<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static int ToInt(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool ToBool(this object o)
        {
            try
            {
                return Convert.ToBoolean(o);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, o + "bool类型转换失败---VALUE:" + o);
                return false;
            }
        }

        public static double ToDouble(this object o)
        {
            try
            {
                return Convert.ToDouble(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DateTime ToDate(this string str)
        {
            try
            {
                return Convert.ToDateTime(str);
            }
            catch (Exception e)
            {
                LogHelper.Log(" ToDate Error " + e.Message);
                return Convert.ToDateTime("1900-1-1");
            }
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var ret = bflag ? Convert.ToInt64(ts.TotalSeconds).ToString() : Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ret;
        }

        /// <summary>
        /// 一定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetNonce(int length)
        {
            const string n = "qwertyuioplkjhgfdsazxcvbnm1234567890QWERTYUIOPLKJHGFDSAZXCVBNM";
            var s = "";
            var r = new Random();
            for (var i = 0; i < length; i++)
            {
                s += n[r.Next(0, n.Length)];
            }
            return s;
        }

        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string ToSha1(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>  
        /// MD5 加密字符串  
        /// </summary>  
        /// <param name="str">源字符串</param>  
        /// <returns>加密后字符串</returns>  
        public static string ToMd5(this string str)
        {
            var md5 = MD5.Create();
            var bs = Encoding.UTF8.GetBytes(str);
            var hs = md5.ComputeHash(bs);
            var sb = new StringBuilder();
            foreach (var b in hs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static TToObject Map<TToObject, TFromObject>(this TFromObject from)
        {
            var fs = typeof(TFromObject).GetProperties();
            var ts = typeof(TToObject).GetProperties();
            var to = Activator.CreateInstance<TToObject>();
            foreach (var f in fs)
            {
                foreach (var t in ts)
                {
                    if (f.Name == t.Name)
                    {
                        var propertyInfo = to.GetType().GetProperty(t.Name);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(to, f.GetValue(from, null), null);
                            break;
                        }
                    }
                }
            }
            return to;
        }

        public static decimal ToDecimal(this object o)
        {
            try
            {
                return Convert.ToDecimal(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DataTable ToDataTable<T>(this List<T> entitys)
        {
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }
            var entityType = entitys[0].GetType();
            var entityProperties = entityType.GetProperties();

            var dt = new DataTable("dt");
            foreach (var t in entityProperties)
            {
                dt.Columns.Add(t.Name);
            }

            foreach (object entity in entitys)
            {
                var entityValues = new object[entityProperties.Length];
                for (var i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static IList<T> ToList<T>(this DataTable table) where T : new()
        {
            var properties = typeof(T).GetProperties().ToList();
            return (from object row in table.Rows select CreateItemFromRow<T>((DataRow)row, properties)).ToList();
        }

        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }

        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties)
            {
                try
                {
                    property.SetValue(item, row[property.Name] ?? "", null);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return item;
        }

        public static string SubString(this string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            return str;
        }

    }

}
