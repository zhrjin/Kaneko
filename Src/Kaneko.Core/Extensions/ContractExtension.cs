using Kaneko.Core.Contract;
using System;

namespace Kaneko.Core.Extensions
{
    public static class ContractExtension
    {
        #region SqlServerWithOperatorBaseDO
        public static SqlServerWithOperatorBaseDO<T> Create<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName)
        {
            @this.Create(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static SqlServerWithOperatorBaseDO<T> Create<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.CreateBy = userId;
            @this.CreateByName = userName;
            @this.CreateDate = dateTime;
            @this.Modity(userId, userName, dateTime);
            return @this;
        }

        public static SqlServerWithOperatorBaseDO<T> Modity<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName)
        {
            @this.Modity(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static SqlServerWithOperatorBaseDO<T> Modity<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.Version++;
            return @this;
        }

        public static SqlServerWithOperatorBaseDO<T> Delete<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName)
        {
            @this.Delete(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static SqlServerWithOperatorBaseDO<T> Delete<T>(this SqlServerWithOperatorBaseDO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.IsDel = 1;
            @this.Version++;
            return @this;
        }
        #endregion

        #region BsseState
        public static BaseState<T> Create<T>(this BaseState<T> @this, string userId, string userName)
        {
            @this.Create(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseState<T> Create<T>(this BaseState<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.CreateBy = userId;
            @this.CreateByName = userName;
            @this.CreateDate = dateTime;
            @this.Modity(userId, userName, dateTime);
            return @this;
        }

        public static BaseState<T> Modity<T>(this BaseState<T> @this, string userId, string userName)
        {
            @this.Modity(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseState<T> Modity<T>(this BaseState<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.Version++;
            return @this;
        }

        public static BaseState<T> Delete<T>(this BaseState<T> @this, string userId, string userName)
        {
            @this.Delete(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseState<T> Delete<T>(this BaseState<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.IsDel = 1;
            @this.Version++;
            return @this;
        }
        #endregion

        #region BaseVO
        public static BaseVO<T> Create<T>(this BaseVO<T> @this, string userId, string userName)
        {
            @this.Create(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseVO<T> Create<T>(this BaseVO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.CreateBy = userId;
            @this.CreateByName = userName;
            @this.CreateDate = dateTime;
            @this.Modity(userId, userName, dateTime);
            return @this;
        }

        public static BaseVO<T> Modity<T>(this BaseVO<T> @this, string userId, string userName)
        {
            @this.Modity(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseVO<T> Modity<T>(this BaseVO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.Version++;
            return @this;
        }

        public static BaseVO<T> Delete<T>(this BaseVO<T> @this, string userId, string userName)
        {
            @this.Delete(userId, userName, System.DateTime.Now);
            return @this;
        }

        public static BaseVO<T> Delete<T>(this BaseVO<T> @this, string userId, string userName, DateTime dateTime)
        {
            @this.ModityBy = userId;
            @this.ModityByName = userName;
            @this.ModityDate = dateTime;
            @this.IsDel = 1;
            @this.Version++;
            return @this;
        }
        #endregion
    }
}
