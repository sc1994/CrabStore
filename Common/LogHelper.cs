﻿using System;
using System.IO;
using System.Text;

namespace Common
{
    public static class LogHelper
    {
        public static void Log(string msg, string title = "", LogTypeEnum type = LogTypeEnum.Info)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\log\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            var str = new StringBuilder();
            str.AppendLine("----------------------------------------------------------");
            str.AppendLine("时间:" + DateTime.Now);
            if (!title.IsNullOrEmpty())
            {
                str.AppendLine("标题:" + title);
            }
            str.AppendLine("内容:" + msg);
            str.AppendLine("类型:" + type);
            str.AppendLine("----------------------------------------------------------\r\n\r\n");
            try
            {
                File.AppendAllText(path, str.ToString(), Encoding.UTF8);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    public enum LogTypeEnum
    {
        Info,
        Waiting,
        Error
    }
}
