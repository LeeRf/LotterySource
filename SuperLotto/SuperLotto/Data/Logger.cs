using SuperLotto.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SuperLotto.Data
{
    // 日志级别枚举
    public enum LogLevel
    {
        DEBUG,  //DEBUG
        INFO,   //普通信息
        WARN,   //警告
        ERROR   //错误
    }

    /// <summary>
    /// 简单日志类
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Info 级别
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            string formatMessage = FormatThat(message, LogLevel.INFO);
            ToFile(formatMessage);
        }

        /// <summary>
        /// Debug 级别
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            string formatMessage = FormatThat(message, LogLevel.DEBUG);
            ToFile(formatMessage);
        }

        /// <summary>
        /// Warning 级别
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(string message)
        {
            string formatMessage = FormatThat(message, LogLevel.WARN);
            ToFile(formatMessage);
        }

        /// <summary>
        /// Error 级别
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            string formatMessage = FormatThat(message, LogLevel.ERROR);
            ToFile(formatMessage);
        }

        /// <summary>
        /// Error 级别
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            string formatMessage = FormatThat($"{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}", LogLevel.ERROR);
            ToFile(formatMessage);
        }

        /// <summary>
        /// 格式化日志消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string FormatThat(string message, LogLevel level)
        {
            //DEBUG是调用方法的上一层
            int stackDepth = 2;

            //Release 模式下适当减少层数
            #if !DEBUG
                stackDepth = 1;
            #endif

            // 获取调用日志的文件名和行号
            var stackFrame = new StackFrame(stackDepth, true);
            string fileName = stackFrame.GetFileName();
            int lineNumber = stackFrame.GetFileLineNumber();

            // 格式化每个字段的宽度
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string levelStr = ("[" + level + "]").PadRight(7);
            string fileNameStr = (fileName ?? "Unknown").PadRight(40);
            string lineNumStr = lineNumber.ToString().PadRight(4);

            // 格式化日志消息为标准格式
            return $"{timestamp} {levelStr} --- {fileNameStr}:{lineNumStr} -> {message}\n";
        }

        /// <summary>
        /// 将消息写道文件中
        /// </summary>
        /// <param name="message"></param>
        static void ToFile(string message)
        {
            Directory.CreateDirectory(Setting.ConfigDirectory);
            System.IO.File.AppendAllText(Setting.RunningLog, message);
        }

        /// <summary>
        /// 写入普通信息文件
        /// </summary>
        /// <param name="directory">文件目录</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="diaryText">要写入的内容</param>
        /// <returns></returns>
        public static void WriteContentToFile(string directory, string filePath, string diaryText)
        {
            Directory.CreateDirectory(directory);
            using (FileStream myself = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(myself, Encoding.GetEncoding("UTF-8")))
                {
                    writer.Write(diaryText);
                }
            }
        }
    }
}
