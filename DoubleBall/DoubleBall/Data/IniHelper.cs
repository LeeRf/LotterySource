using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Data
{
    /// <summary>
    /// ini文件操作类
    /// </summary>
    public static class IniHelper
    {
        #region ini的API声明
        /// <summary>
        /// 获取所有节点名称(Section)
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        /// <summary>
        /// 获取某个指定节点(Section)中所有KEY和Value
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

        //另一种声明方式,使用 StringBuilder 作为缓冲区类型的缺点是不能接受\0字符，会将\0及其后的字符截断,
        //所以对于lpAppName或lpKeyName为null的情况就不适用
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        //再一种声明，使用string作为缓冲区的类型同char[]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 将指定的键值对写到指定的节点，如果已经存在则替换。
        /// </summary>
        /// <returns>是否成功写入</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        /// <summary>
        /// 将指定的键和值写到指定的节点，如果已经存在则替换
        /// </summary>
        /// <param name="lpAppName">节点名称</param>
        /// <param name="lpKeyName">键.如果为null,则删除指定的节点及其所有的项目</param>
        /// <param name="lpString">值.如果为null,则删除指定节点中指定的键</param>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        #endregion

        #region 基本封装
        /// <summary>
        /// 在INI文件中，将指定的键值对写到指定的节点，如果已经存在则替换
        /// </summary>
        public static bool IniWriteItems(string iniFile, string section, string items)
        {
            return WritePrivateProfileSection(section, items, iniFile);
        }

        /// <summary>
        /// 在INI文件中，指定节点写入指定的键及值。如果已经存在，则替换。如果没有则创建。
        /// </summary>
        public static bool IniWriteValue(string iniFile, string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, iniFile);
        }

        /// <summary>
        /// 在INI文件中，删除指定节点中的指定的键。
        /// </summary>
        public static bool IniDeleteKey(string iniFile, string section, string key)
        {
            return WritePrivateProfileString(section, key, null, iniFile);
        }

        /// <summary>
        /// 在INI文件中，删除指定的节点。
        /// </summary>
        public static bool IniDeleteSection(string iniFile, string section)
        {
            return WritePrivateProfileString(section, null, null, iniFile);
        }

        /// <summary>
        /// 在INI文件中，删除指定节点中的所有内容。
        /// </summary>
        public static bool IniEmptySection(string iniFile, string section)
        {
            return WritePrivateProfileSection(section, string.Empty, iniFile);
        }

        /// <summary>
        /// 读取INI文件中指定INI文件中的所有节点名称(Section)
        /// </summary>
        public static string[] IniGetAllSectionNames(string iniFile)
        {
            uint MAX_BUFFER = 32767;
            string[] sections = new string[0];

            //申请内存
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, iniFile);

            if (bytesReturned != 0)
            {
                //读取指定内存的内容
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();
                //每个节点之间用\0分隔,末尾有一个\0
                sections = local.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //释放内存
            Marshal.FreeCoTaskMem(pReturnedString);

            return sections;
        }

        /// <summary>
        /// 获取INI文件中指定节点(Section)中的所有条目(key=value形式)
        /// </summary>
        public static string[] IniGetAllItems(string iniFile, string section)
        {
            //返回值形式为 key=value.例如 Color=Red
            uint MAX_BUFFER = 32767;
            string[] items = new string[0];

            //分配内存
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));

            uint bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, iniFile);

            if (bytesReturned != MAX_BUFFER - 2 || (bytesReturned == 0))
            {

                string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);
                items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            Marshal.FreeCoTaskMem(pReturnedString);

            return items;
        }

        /// <summary>
        /// 获取INI文件中指定节点(Section)中的所有条目的Key列表
        /// </summary>
        public static string[] IniGetAllItemKeys(string iniFile, string section)
        {
            string[] value = new string[0];
            const int size = 1024 * 10;

            char[] chars = new char[size];
            uint bytesReturned = GetPrivateProfileString(section, null, null, chars, size, iniFile);

            if (bytesReturned != 0)
            {
                value = new string(chars).Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            chars = null;

            return value;
        }

        /// <summary>
        /// 读取INI文件中指定KEY的字符串型值
        /// </summary>
        /// <returns>读取到的值</returns>
        public static string IniGetStringValue(string iniFile, string section, string key, string defaultValue)
        {
            string value = defaultValue;
            const int size = 1024 * 10;

            StringBuilder sb = new StringBuilder(size);
            uint bytesReturned = GetPrivateProfileString(section, key, defaultValue, sb, size, iniFile);

            if (bytesReturned != 0) value = sb.ToString();

            sb = null;
            return value;
        }
        #endregion

        #region 进阶封装
        /// <summary>
        /// 将对象转换为ini保存
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <param name="iniPath">路径</param>
        public static bool SaveOrUpdateIniData(object obj, string iniPath)
        {
            bool isSucceed = false;
            PropertyInfo[] pis = obj.GetType().GetProperties();

            foreach (var propertyInfo in pis)
            {
                if (propertyInfo.GetMethod.IsStatic)
                    continue;
                isSucceed = IniWriteValue
                (
                    iniPath, propertyInfo.DeclaringType.Name, propertyInfo.Name, propertyInfo.GetValue(obj) + ""
                ); 
                if(!isSucceed) break;
            }

            return isSucceed;
        }

        /// <summary>
        /// 将ini数据转换为对象
        /// </summary>
        public static object ReadIniData<T>(string iniPath)
        {
            Type type = typeof(T);
            T data = (T)Activator.CreateInstance(type);
            //取得该类的所有Key的列表
            string[] keyValues = IniGetAllItemKeys(iniPath, type.Name);

            //若有继承. 则累加该类的
            if (!string.IsNullOrEmpty(type.BaseType.Name))
            {
                string[] baseValues = IniGetAllItemKeys(iniPath, type.BaseType.Name);
                string[] newValues = new string[keyValues.Length + baseValues.Length];
                keyValues.CopyTo(newValues, 0);
                baseValues.CopyTo(newValues, keyValues.Length);
                keyValues = newValues;
            }

            foreach (string iniGetAllItemKey in keyValues)
            {
                PropertyInfo property = type.GetProperty(iniGetAllItemKey);
                if (property != null)
                {
                    Type objectType = property.PropertyType;

                    string keyValue = IniGetStringValue(iniPath, property.DeclaringType.Name, iniGetAllItemKey, null);
                    if (objectType.IsEnum)
                    {
                        property.SetValue(data, Convert.ChangeType(Enum.Parse(objectType, keyValue, true), objectType));
                    }
                    else
                    {
                        property.SetValue(data, Convert.ChangeType(keyValue, property.PropertyType));
                    }
                }
            }

            return data;
        }
        #endregion
    }
}