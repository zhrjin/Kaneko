using Kaneko.Dapper.Enums;
using Kaneko.Dapper.Expressions;

namespace Kaneko.Dapper.Extensions
{
    /// <summary>
    /// Sql别名
    /// </summary>
    public static class SqlAliasExtensions
    {
        /// <summary>
        /// 参数前缀
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ParamPrefix(this DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return "@";
                case DatabaseType.MySql:
                    return "?";
                case DatabaseType.SQLite:
                    return "@";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取添加左右标记 防止有关键字作为字段名/表名
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="sqlGenerate"></param>
        /// <returns></returns>
        public static string ParamSql(this string columnName, SqlGenerate sqlGenerate)
        {
            return columnName.ParamSql(sqlGenerate?.DatabaseType);
        }

        /// <summary>
        /// 获取添加左右标记 防止有关键字作为字段名/表名
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ParamSql(this string columnName, DatabaseType? dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    if (columnName.StartsWith("["))
                        return columnName;
                    return $"[{columnName}]";
                case DatabaseType.MySql:
                    if (columnName.StartsWith("`"))
                        return columnName;
                    return $"`{columnName}`";
                case DatabaseType.SQLite:
                    if (columnName.StartsWith("`"))
                        return columnName;
                    return $"`{columnName}`";
                default:
                    return columnName;
            }
        }

        /// <summary>
        /// 获取最后一次Insert
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string SelectLastIdentity(this DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return " select @@Identity";
                case DatabaseType.MySql:
                    return " select LAST_INSERT_ID();";
                case DatabaseType.SQLite:
                    return " select last_insert_rowid();";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string ExistTableSql(this DatabaseType dbType, string dbName, string tableName)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $"select count(1) from sys.tables where name='{tableName}' and type = 'u'";
                case DatabaseType.MySql:
                    return $"select count(1) from information_schema.tables where table_schema = '{dbName}' and table_name = '{tableName}'";
                case DatabaseType.SQLite:
                    return $"select count(1) from sqlite_master where type = 'table' and name='{tableName}'";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// 是否存在字段
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string ExistFieldSql(this DatabaseType dbType, string dbName, string tableName, string fieldName)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $"select count(1) from sys.columns where object_id = object_id('{tableName}') and name='{fieldName}'";
                case DatabaseType.MySql:
                    return $"select count(1) from information_schema.columns where table_schema = '{dbName}' and table_name  = '{tableName}' and column_name = '{fieldName}'";
                case DatabaseType.SQLite:
                    return $"select * from sqlite_master where name='{tableName}' and sql like '%{fieldName}%';";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// 创建字段语句
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string AddColumnsSql(this DatabaseType dbType, string tableName, string columnDefinitione)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $"alter table {tableName.ParamSql(dbType)} add {columnDefinitione}";
                case DatabaseType.MySql:
                    return $"alter table {tableName.ParamSql(dbType)} add column {columnDefinitione}";
                case DatabaseType.SQLite:
                    return $"alter table {tableName.ParamSql(dbType)} add {columnDefinitione}";
                default:
                    return $"alter table {tableName.ParamSql(dbType)} add {columnDefinitione}";
            }
        }

        /// <summary>
        /// 修改字段语句
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="tableName"></param>
        /// <param name="columnDefinitione"></param>
        /// <returns></returns>
        public static string AlterColumnsSql(this DatabaseType dbType, string tableName, string columnDefinitione)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $"alter table {tableName.ParamSql(dbType)} alter column {columnDefinitione}";
                case DatabaseType.MySql:
                    return $"alter table {tableName.ParamSql(dbType)} modify {columnDefinitione}";
                case DatabaseType.SQLite:
                    return $"alter table {tableName.ParamSql(dbType)} alter column {columnDefinitione}";
                default:
                    return $"alter table {tableName.ParamSql(dbType)} alter column {columnDefinitione}";
            }
        }

        /// <summary>
        /// 查询表结构脚本
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string GetSchemaColumnsQueryScript(this DatabaseType dbType, string tableName)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $@"
                        SELECT
                            upper(c.COLUMN_NAME) as Name, 
	                        case c.IS_NULLABLE
                                when 'YES' then 'null'
                                else 'not null'
                            end as Nullable, 
	                        upper(c.DATA_TYPE) as DataType, 
	                        cast(ISNULL(c.CHARACTER_OCTET_LENGTH, 0) as numeric) as Size, 
	                        upper(c.TABLE_NAME) as TableName,
	                        case OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey')
                            when 1 then 'primary key' else '' end as PrimaryKey
                        FROM information_schema.columns c
                        left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE p
                        on c.TABLE_NAME = p.TABLE_NAME
                        and c.COLUMN_NAME = p.COLUMN_NAME
                        where upper(c.TABLE_NAME) = '{tableName.ToUpper()}'
                        order by c.TABLE_NAME,c.ordinal_position";
                case DatabaseType.MySql:
                    return "";
                case DatabaseType.SQLite:
                    return "";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 创建主键脚本
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyFieldName"></param>
        /// <returns></returns>
        public static string CreatePrimaryKeyScript(this DatabaseType dbType, string tableName, string primaryKeyFieldName)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return $@"ALTER TABLE {tableName} ADD CONSTRAINT PK_{tableName.ToUpper()}_{primaryKeyFieldName} PRIMARY KEY({primaryKeyFieldName})";
                case DatabaseType.MySql:
                    return "";
                case DatabaseType.SQLite:
                    return "";
                default:
                    return "";
            }
        }
    }
}
