using DysoftLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;

namespace RegistryLibrary.Helper
{
    /// <summary>
    /// <see cref="DataTable"/>工具类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 验证是否存在指定表，如果不存在将自动创建
        /// </summary>
        /// <param name="tableName">表名称</param>
        public static void VerificationTable(string tableName)
        {
            CreateTable(tableName);
        }

        /// <summary>
        /// 验证是否存在指定表，如果不存在将自动创建，并自动调整表序列(如果没有将自动创建)
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="primaryKeyName">主键名称, 如果该值不为空将自动更新序列</param>
        public static void VerificationTable(string tableName, string primaryKeyName)
        {
            VerificationTable(tableName);
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                AdjustmentSequence(tableName, primaryKeyName);
            }
        }

        /// <summary>
        /// 验证是否存在指定表，如果不存在将自动创建，并自动调整表序列(如果没有将自动创建)
        /// </summary>
        /// <param name="tableDic">key值为表名称, value值为主键名称, 如果主键名称不为空将自动更新序列</param>
        public static void VerificationTable(Dictionary<string, string> tableDic)
        {
            foreach (string tableName in tableDic.Keys)
            {
                VerificationTable(tableName, tableDic[tableName]);
            }
        }

        /// <summary>
        /// 判断数据库中是否存在指定表
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public static bool TableIsExists(string tableName)
        {
            return OracleDB.GetScalarStr("select count(1) from user_tables where table_name = :tableName",
                new OracleParameter("tableName", tableName?.ToUpper() ?? "")) == "1";
        }

        /// <summary>
        /// 判断数据库中是否存在指定序列
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        /// <returns></returns>
        public static bool SequenceIsExists(string sequenceName)
        {
            return OracleDB.GetScalarStr("select count(1) from user_sequences where sequence_name = :sequenceName",
                new OracleParameter("sequenceName", sequenceName?.ToUpper() ?? "")) == "1";
        }

        /// <summary>
        /// 自动创建指定表
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <exception cref="FileNotFoundException">当指定表的建表语句没有找到时会发生异常</exception>
        public static void CreateTable(string tableName)
        {
            using (FileStream stream = File.OpenRead($"{System.AppDomain.CurrentDomain.BaseDirectory}/TableStructure/{tableName}.sql"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    foreach (string sql in reader.ReadToEnd().Split(';'))
                    {
                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            try
                            {
                                OracleDB.ExeSql(sql);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自动调整序列
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        public static void AdjustmentSequence(string sequenceName)
        {
            if (!SequenceIsExists(sequenceName))
            {
                //如果序列不存在，自动生成序列
                OracleDB.ExeSql(
                    $@"create sequence {sequenceName}
                      minvalue 1
                      maxvalue 9999999
                      start with 1
                      increment by 1
                      cache 20");
            }
        }

        /// <summary>
        /// 自动调整表序列
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="primaryKeyName">主键名称</param>
        public static void AdjustmentSequence(string tableName, string primaryKeyName)
        {
            string sequenceName = $"sequ_{tableName}";
            if (SequenceIsExists(sequenceName))
            {
                OracleDB.ExeSql($"drop sequence {sequenceName}");
            }

            //如果序列不存在，自动生成序列
            //自动调整序列值为主键最大值+1
            OracleDB.ExeSql(
                $@"create sequence {sequenceName}
                    minvalue 1
                    maxvalue 9999999
                    start with {OracleDB.GetScalarStr($"select greatest(nvl(max({primaryKeyName}), 0), 0) + 1 from {tableName}")}
                    increment by 1
                    cache 20");
        }

        /// <summary>
        /// <see cref="DataTable"/>转实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="data">数据源</param>
        /// <returns>转换后的实体列表</returns>
        public static List<T> ToList<T>(this DataTable data)
        {
            if (typeof(T) == typeof(string))
            {
                //如果类型为string， 要进行数据转换
                return data.ToList<JArray>().Select(
                    item => item[data.Columns[0].ColumnName].ToString()).ToList() as List<T>;
            }
            else
            {
                return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(data));
            }
        }

        /// <summary>
        /// <see cref="DataTable"/>转实体
        /// 如果存在数据, 返回第一条, 否则返回空对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="data">数据源</param>
        /// <returns>转换后的实体</returns>
        public static T ToObject<T>(this DataTable data)
        {
            if (data.Rows.Count > 0)
            {
                return data.ToList<T>()[0];
            }
            else { return Activator.CreateInstance<T>(); }
        }
    }
}
