using Dapper;
using MySql.Data.MySqlClient;
using Kaneko.Dapper.Expressions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Kaneko.Dapper.Enums;
using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Kaneko.Core.Attributes;
using Kaneko.Core.DependencyInjection;

namespace Kaneko.Dapper.Extensions
{
    /// <summary>
    /// Dapper扩展
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="val"></param>
        /// <param name="tableNameFunc"></param>
        /// <returns></returns>
        public static string GetTableName<TEntity>(this string val, Func<string> tableNameFunc = null) where TEntity : IDomainObject
        {
            if (tableNameFunc != null)
                return tableNameFunc.Invoke();

            var t = typeof(TEntity);
            var mTableName = t.GetMainTableName();
            return mTableName;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="entity">实体实例</param>
        /// <param name="tableNameFunc"></param>
        /// <returns></returns>
        public static string GetTableName<TEntity>(this TEntity entity, Func<string> tableNameFunc = null)
        {
            if (tableNameFunc != null)
                return tableNameFunc.Invoke();

            var t = typeof(TEntity);
            var mTableName = t.GetMainTableName();
            return mTableName;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="expression">表达式数据</param>
        /// <param name="tableNameFunc"></param>
        /// <returns></returns>
        public static string GetTableName<TEntity>(this Expression<Func<TEntity, bool>> expression, Func<string> tableNameFunc = null) where TEntity : IDomainObject
        {
            if (tableNameFunc != null)
                return tableNameFunc.Invoke();

            var t = typeof(TEntity);
            var mTableName = t.GetMainTableName();
            return mTableName;
        }

        /// <summary>
        /// 获取自增字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static PropertyInfo GetIdentityField(this Type entity)
        {
            var propertyInfos = entity.GetProperties();
            foreach (var pi in propertyInfos)
            {
                var attribute = pi.GetKanekoAttribute<KanekoIdAttribute>();
                if (attribute != null)
                {
                    return pi;
                }
            }
            return null;
        }

        /// <summary>
        /// 是否自增字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsAutoIdentity(this PropertyInfo property)
        {
            var attribute = property.GetKanekoAttribute<KanekoIdAttribute>();
            if (attribute != null && attribute.AutoIdEntity)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="outSqlAction"></param>
        /// <returns></returns>
        public static async Task<bool> IsExistTableAsync(this IDbConnection connection, string tableName, Action<string> outSqlAction = null)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;
            var dbType = connection.GetDbType();
            var dbName = connection.Database;
            var sql = dbType.ExistTableSql(dbName, tableName);
            var task = await connection.QueryFirstOrDefaultAsync<int>(sql);
            outSqlAction?.Invoke(sql);
            return task > 0;
        }

        /// <summary>
        /// 是否存在字段
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="outSqlAction"></param>
        /// <returns></returns>
        public static async Task<bool> IsExistFieldAsync(this IDbConnection connection, string tableName, string fieldName, Action<string> outSqlAction = null)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;
            var dbType = connection.GetDbType();
            var dbName = connection.Database;
            var sql = dbType.ExistFieldSql(dbName, tableName, fieldName);
            var task = await connection.QueryFirstOrDefaultAsync<int>(sql);
            outSqlAction?.Invoke(sql);
            return task > 0;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="transaction">事务</param>
        /// <param name="returnLastIdentity">是否返回自增的数据</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns>-1 参数为空</returns>
        public static async Task<int> InsertAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            TEntity entity,
            IDbTransaction transaction = null,
            bool returnLastIdentity = false,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var addFields = new List<string>();
            var atFields = new List<string>();
            var dbType = connection.GetDbType();

            var pis = typeof(TEntity).GetProperties();
            var identityPropertyInfo = typeof(TEntity).GetIdentityField();
            foreach (var pi in pis)
            {
                if (identityPropertyInfo?.Name == pi.Name && identityPropertyInfo.IsAutoIdentity())
                    continue;

                addFields.Add($"{pi.GetFieldName().ParamSql(dbType)}");
                atFields.Add($"@{pi.Name}");
            }

            var sql = $"insert into {tableName.ParamSql(dbType)}({string.Join(", ", addFields)}) values({string.Join(", ", atFields)});";

            int task;
            if (identityPropertyInfo != null && returnLastIdentity)
            {
                sql += dbType.SelectLastIdentity();
                task = await connection.ExecuteScalarAsync<int>(sql, entity, transaction);
                if (task > 0)
                    identityPropertyInfo.SetValue(entity, task);
            }
            else
            {
                task = await connection.ExecuteAsync(sql, entity, transaction);
            }
            // 返回sql
            outSqlAction?.Invoke(sql);

            return task;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns>-1 参数为空</returns>
        public static async Task<int> InsertAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            IEnumerable<TEntity> entities,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if ((entities?.Count() ?? 0) <= 0)
                throw new ArgumentNullException(nameof(entities));

            var addFields = new List<string>();
            var atFields = new List<string>();
            var dbType = connection.GetDbType();

            var pis = typeof(TEntity).GetProperties();
            var identityPropertyInfo = typeof(TEntity).GetIdentityField();
            foreach (var pi in pis)
            {
                if (identityPropertyInfo?.Name == pi.Name)
                    continue;

                addFields.Add($"{pi.GetFieldName().ParamSql(dbType)}");
                atFields.Add($"@{pi.Name}");
            }

            var sql = $"insert into {tableName.ParamSql(dbType)}({string.Join(", ", addFields)}) values({string.Join(", ", atFields)});";
            var task = await connection.ExecuteAsync(sql, entities, transaction);
            // 返回sql
            outSqlAction?.Invoke(sql);

            return task;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="whereExpress"></param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns>-1 参数为空</returns>
        public static async Task<int> DeleteAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            Expression<Func<TEntity, bool>> whereExpress,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (whereExpress == null)
                throw new ArgumentNullException(nameof(whereExpress));

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Delete<TEntity>(dbType, tableName).Where(whereExpress);
            var task = await connection.ExecuteAsync(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return task;
        }

        /// <summary>
        /// 对象修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="fields">选择字段</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<bool> SetAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            TEntity entity,
            IEnumerable<string> fields = null,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var setFields = new List<string>();
            var whereFields = new List<string>();
            var dbType = connection.GetDbType();

            var pis = typeof(TEntity).GetProperties();
            foreach (var pi in pis)
            {
                string fieldName = pi.GetFieldName();
                var obs = pi.GetKanekoAttribute<KanekoIdAttribute>();
                if (obs != null)
                {
                    whereFields.Add($"{fieldName.ParamSql(dbType)} = @{pi.Name}");
                }
                else
                {
                    if ((fields?.Count() ?? 0) <= 0 || fields.Contains(fieldName))
                        setFields.Add($"{fieldName.ParamSql(dbType)} = @{pi.Name}");
                }
                ///更换为pi.GetAttribute<KanekoIdAttribute>();
                //var obs = pi.GetCustomAttributes(typeof(KanekoIdAttribute), false);
                //if (obs?.Count() > 0)
                //    whereFields.Add($"{fieldName.ParamSql(dbType)} = @{fieldName}");
                //else
                //{
                //    if ((fields?.Count() ?? 0) <= 0 || fields.Contains(fieldName))
                //        setFields.Add($"{fieldName.ParamSql(dbType)} = @{fieldName}");
                //}
            }
            if (whereFields.Count <= 0)
                throw new Exception($"实体[{nameof(TEntity)}]未设置主键Key属性");
            if (setFields.Count <= 0)
                throw new Exception($"实体[{nameof(TEntity)}]未标记任何更新字段");

            var sql = $"update {tableName.ParamSql(dbType)} set {string.Join(", ", setFields)} where {string.Join(", ", whereFields)}";
            var result = await connection.ExecuteAsync(sql, entity, transaction);
            // 返回sql
            outSqlAction?.Invoke(sql);
            return result > 0;
        }

        /// <summary>
        /// 条件修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection">连接</param>
        /// <param name="tableName">表名</param>
        /// <param name="setExpress">修改内容表达式</param>
        /// <param name="whereExpress">条件表达式</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<bool> SetAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            Expression<Func<object>> setExpress,
            Expression<Func<TEntity, bool>> whereExpress,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (setExpress == null || whereExpress == null)
                throw new ArgumentNullException($"{nameof(setExpress)} / {nameof(whereExpress)}");

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Update<TEntity>(dbType, setExpress, tableName).Where(whereExpress);
            var result = await connection.ExecuteAsync(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return result > 0;
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName">表名</param>
        /// <param name="whereExpress">条件表达式</param>
        /// <param name="fieldExpress">选择字段，默认为*</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<TEntity> GetAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            Expression<Func<TEntity, bool>> whereExpress,
            Expression<Func<TEntity, object>> fieldExpress = null,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (whereExpress == null)
                throw new ArgumentNullException(nameof(whereExpress));

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Select(dbType, fieldExpress, tableName).Where(whereExpress);
            var task = await connection.QueryFirstOrDefaultAsync<TEntity>(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return task;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="whereExpress">条件表达式</param>
        /// <param name="fieldExpress">选择字段，默认为*</param>
        /// <param name="orderByFields">排序字段集合</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            int page,
            int rows,
            Expression<Func<TEntity, bool>> whereExpress,
            Expression<Func<TEntity, object>> fieldExpress = null,
            List<OrderByField> orderByFields = null,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Select(dbType, fieldExpress, tableName);
            if (whereExpress != null)
                sqlExpression.Where(whereExpress);

            var orderBy = string.Empty;
            if ((orderByFields?.Count ?? 0) > 0)
                orderBy = $" {string.Join(", ", orderByFields.Select(oo => oo.Field.ParamSql(dbType) + " " + oo.OrderBy))}";
            sqlExpression.OrderBy(orderBy).Limit(page, rows);

            var task = await connection.QueryAsync<TEntity>(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return task;
        }

        /// <summary>
        /// 获取分页数据 Offset
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="whereExpress">条件表达式</param>
        /// <param name="fieldExpress">选择字段，默认为*</param>
        /// <param name="orderByFields">排序字段集合</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TEntity>> GetOffsetsAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            int offset,
            int size,
            Expression<Func<TEntity, bool>> whereExpress,
            Expression<Func<TEntity, object>> fieldExpress = null,
            List<OrderByField> orderByFields = null,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Select(dbType, fieldExpress, tableName);
            if (whereExpress != null)
                sqlExpression.Where(whereExpress);

            var orderBy = string.Empty;
            if ((orderByFields?.Count ?? 0) > 0)
                orderBy = $" {string.Join(", ", orderByFields.Select(oo => oo.Field.ParamSql(dbType) + " " + oo.OrderBy))}";
            sqlExpression.OrderBy(orderBy).Offset(offset, size);

            var task = await connection.QueryAsync<TEntity>(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return task;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="whereExpress">条件表达式</param>
        /// <param name="transaction">事务</param>
        /// <param name="outSqlAction">返回sql语句</param>
        /// <returns></returns>
        public static async Task<int> CountAsync<TEntity>(this
            IDbConnection connection,
            string tableName,
            Expression<Func<TEntity, bool>> whereExpress,
            IDbTransaction transaction = null,
            Action<string> outSqlAction = null)
            where TEntity : IDomainObject
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var dbType = connection.GetDbType();
            var sqlExpression = SqlExpression.Count<TEntity>(dbType, tableName: tableName).Where(whereExpress);
            var task = await connection.QueryFirstOrDefaultAsync<int>(sqlExpression.Script, sqlExpression.DbParams, transaction);
            // 返回sql
            outSqlAction?.Invoke(sqlExpression.Script);
            return task;
        }

        static readonly ConcurrentDictionary<string, DatabaseType> MSSqlDbType = new ConcurrentDictionary<string, DatabaseType>();

        /// <summary>
        /// 获取db类型
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static DatabaseType GetDbType(this IDbConnection connection)
        {
            if (connection is MySqlConnection)
                return DatabaseType.MySql;
            if (connection is SqlConnection connection1)
            {
                return MSSqlDbType.GetOrAdd(connection.ConnectionString, (connectionString) =>
                {
                    var sqlConnection = connection1;
                    var v = sqlConnection.ServerVersion;
                    int.TryParse(v.Substring(0, v.IndexOf(".")), out int bV);
                    if (bV >= Constants.MSSQLVersion.SQLServer2012Bv)
                        return DatabaseType.GteSqlServer2012;
                    return DatabaseType.SqlServer;
                });
            }
            if (connection is Microsoft.Data.Sqlite.SqliteConnection)
                return DatabaseType.SQLite;

            return DatabaseType.MySql;
        }

        /// <summary>
        /// 获取主表名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static string GetMainTableName(this Type entity)
        {
            var attribute = entity.GetKanekoAttribute<KanekoTableAttribute>();
            string mTableName;
            if (attribute == null)
                mTableName = entity.Name;
            else
                mTableName = attribute.Name;
            return mTableName;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static object GetValueFromExpression<TEntity>(this PropertyInfo propertyInfo, Expression<Func<TEntity, bool>> expression)
        {
            var dictionary = new Dictionary<object, object>();
            ExpressionHelper.Resolve(expression.Body, ref dictionary);
            if ((dictionary?.Count ?? 0) <= 0)
                throw new ArgumentNullException($"Property [{propertyInfo.Name}] 数据为空");

            dictionary.TryGetValue(propertyInfo.Name, out object val);
            return val;
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetFieldName(this PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetKanekoAttribute<KanekoColumnAttribute>();
            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
            {
                return attribute.Name;
            }

            return propertyInfo.Name;
        }

        
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetFieldName(this MemberInfo memberInfo)
        {
            var attribute = memberInfo.GetKanekoAttribute<KanekoColumnAttribute>();
            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
            {
                return attribute.Name;
            }

            return memberInfo.Name;
        }

        /// <summary>
        /// 获取列定义
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetColumnDefinition(this PropertyInfo propertyInfo, DatabaseType databaseType)
        {
            var attribute = propertyInfo.GetKanekoAttribute<KanekoColumnAttribute>();
            string column = propertyInfo.Name, columnDefinition = "varchar(255)";
            if (attribute != null)
            {
                column = string.IsNullOrWhiteSpace(attribute.Name) ? column : attribute.Name;
                columnDefinition = string.IsNullOrWhiteSpace(attribute.ColumnDefinition) ? "varchar(255)" : attribute.ColumnDefinition;
            }
            column = column.ParamSql(databaseType);

            return $" {column} {columnDefinition} ";
        }
    }
}
