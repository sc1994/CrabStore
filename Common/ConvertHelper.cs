﻿using System;
using Newtonsoft.Json;

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

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T JsonToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static int ToInt(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, o + "int类型转换失败---VALUE:" + o);
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

        public static double ToDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception e)
            {
                LogHelper.Log(" ToDouble Error " + e.Message);
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

    }
}
