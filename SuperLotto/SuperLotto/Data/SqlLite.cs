using SQLite;
using SuperLotto.Model;
using SuperLotto.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SuperLotto.Data
{
    public class SqlLite
    {
        //默认数据库地址
        
        public static string ConnectStr => Setting.ConfigDirectory + @"\data.db";

        public static void CreateTable<T>() => CreateTable<T>(ConnectStr);

        /// <summary>
        /// 创建表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectStr">连接地址</param>
        public static void CreateTable<T>(string connectStr)
        {
            Directory.CreateDirectory(Setting.ConfigDirectory);
            using (var db = new SQLiteConnection(connectStr))
            {
                db.CreateTable<T>();
            }
        }

        public static int Save<T>(object obj) => Save<T>(obj, ConnectStr);

        /// <summary>
        /// 保存对象到库
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="connectStr">连接地址</param>
        /// <returns></returns>
        public static int Save<T>(object obj, string connectStr)
        {
            if (!File.Exists(connectStr))
                CreateTable<T>(connectStr);
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.Insert(obj);
            }
        }

        public static int SaveOrUpdate<T>(object obj) => SaveOrUpdate<T>(obj, ConnectStr);

        /// <summary>
        /// 保存或者修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="connectStr">连接地址</param>
        /// <returns></returns>
        public static int SaveOrUpdate<T>(object obj, string connectStr)
        {
            if (!File.Exists(connectStr))
                CreateTable<T>(connectStr);
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.InsertOrReplace(obj);
            }
        }

        /// <summary>
        /// 插入所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="connectStr">连接地址</param>
        /// <returns></returns>
        public static int InsertAll<T>(List<T> dataList, string connectStr)
        {
            if (!File.Exists(connectStr))
                CreateTable<T>(connectStr);
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.InsertAll(dataList);
            }
        }

        /// <summary>
        /// 修改该对象数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="connectStr">连接地址</param>
        /// <returns></returns>
        public static int Update(object obj, string connectStr)
        {
            if (!File.Exists(connectStr)) return -1;
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.Update(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="connectStr"></param>
        /// <returns></returns>
        public static int Delete(object obj, string connectStr)
        {
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.Delete(obj);
            }
        }

        public static object Table<T>() => Table<T>(ConnectStr);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectStr"></param>
        /// <returns></returns>
        public static object Table<T>(string connectStr)
        {
            if (!File.Exists(connectStr)) return null;
            using (var db = new SQLiteConnection(connectStr))
            {
                switch (typeof(T).Name)
                {
                    case "ExceptionLog": 
                        return db.Table<ExceptionLog>().Reverse().ToList();
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="connectStr"></param>
        /// <returns></returns>
        public static object Get<T>(object id, string connectStr)
        {
            if (!File.Exists(connectStr)) return null;
            var db = new SQLiteConnection(connectStr);

            switch (typeof(T).Name)
            {
                case "ExceptionLog": 
                    return db.Get<ExceptionLog>(pk: id);
                default:
                    return null;
            }
        }

        public static int Execute(string query, params object[] args) => Execute(query, ConnectStr, args);

        /// <summary>
        /// 执行Sql语句(比如 Update、Delete、Insert)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectStr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Execute(string query, string connectStr, params object[] args)
        {
            if (!File.Exists(connectStr)) return -1;
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.Execute(query, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectStr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int CountColumn(string query, string connectStr, params object[] args)
        {
            if (!File.Exists(connectStr)) return -1;
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.ExecuteScalar<int>(query, args);
            }
        }

        public static object Query<T>(string query, params object[] args) => Query<T>(query, ConnectStr, args);

        /// <summary>
        /// 查询Sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="connectStr"></param>
        /// <returns></returns>
        public static object Query<T>(string query, string connectStr, params object[] args)
        {
            if (!File.Exists(connectStr)) return null;
            using (var db = new SQLiteConnection(connectStr))
            {
                switch (typeof(T).Name)
                {
                    case "ExceptionLog": 
                        return db.Query<ExceptionLog>(query, args);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// 校验表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectStr">连接地址</param>
        public static bool VerificationTable(string tableName, string connectStr)
        {
            if (!File.Exists(connectStr)) return false;
            using (var db = new SQLiteConnection(connectStr))
            {
                string VerificationTableIsExtis = " select count(*) from sqlite_master where type = 'table' and name = ? ";
                return db.ExecuteScalar<int>(VerificationTableIsExtis, tableName) > 0;
            }
        }

        /// <summary>
        /// 执行带有参数的命令文本
        /// </summary>
        /// <param name="querySql">查询语句</param>
        /// <param name="connectStr">连接地址</param>
        /// <param name="args">参数</param>
        public static T ExecuteScalar<T>(string querySql, string connectStr, params object[] args)
        {
            if (!File.Exists(connectStr)) return default;
            using (var db = new SQLiteConnection(connectStr))
            {
                return db.ExecuteScalar<T>(querySql, args);
            }
        }

        public static int DeleteAll<T>() => DeleteAll<T> (ConnectStr);

        /// <summary>
        /// 删除指定表所有数据
        /// </summary>
        /// <param name="connectStr">连接地址</param>
        public static int DeleteAll<T>(string connectStr)
        {
            if (!File.Exists(connectStr)) return -1;
            using (var db = new SQLiteConnection(connectStr))
            {
                switch (typeof(T).Name)
                {
                    case "ExceptionLog":
                        return db.DeleteAll<ExceptionLog>();
                    default:
                        return -1;
                }
            }
        }
    }
}
