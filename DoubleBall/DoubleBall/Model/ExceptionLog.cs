using SQLite;
using System;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 软件异常类
    /// </summary>
    [Table("exception_log")]
    public class ExceptionLog
    {
        /// <summary>
        /// ID
        /// </summary>
        [Column("id"), PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// 异常时间
        /// </summary>
        [Column("exception_date"), NotNull]
        public string ExceptionDate { get; set; }
        /// <summary>
        /// 是否灾难性错误
        /// </summary>
        [Column("is_disaster_error"), NotNull]
        public bool IsDisasterError { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        [Column("exception_message"), NotNull]
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// 异常方法名
        /// </summary>
        [Column("exception_method"), NotNull]
        public string ExceptionMethod { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        [Column("exception_type"), NotNull]
        public string ExceptionType { get; set; }
        /// <summary>
        /// 异常对象
        /// </summary>
        [Column("exception_source"), NotNull]
        public string ExceptionSource { get; set; }
        /// <summary>
        /// 异常编码
        /// </summary>
        [Column("encoding"), NotNull]
        public string Encoding { get; set; }
        /// <summary>
        /// 调用堆栈信息
        /// </summary>
        [Column("call_stack"), NotNull]
        public string CallStack { get; set; }

        public ExceptionLog()
        {
        }

        public ExceptionLog(string exceptionData, bool isDisasterError, string exceptionMessage, string exceptionMethod, string exceptionType, string exceptionSource, string encoding, string callStack)
        {
            ExceptionDate = exceptionData;
            IsDisasterError = isDisasterError;
            ExceptionMessage = exceptionMessage;
            ExceptionMethod = exceptionMethod;
            ExceptionType = exceptionType;
            ExceptionSource = exceptionSource;
            Encoding = encoding;
            CallStack = callStack;
        }
    }
}