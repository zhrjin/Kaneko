using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kaneko.Core.Extensions
{
    public static class SearchDTOExtensions
    {
        public static T DeserializeObject<T>(this SearchDTO search) where T : IDataTransferObject
        {
            T dTO = JsonConvert.DeserializeObject<T>(search.QueryJson);
            Pagination pagination = JsonConvert.DeserializeObject<Pagination>(search.Pagination);

            dTO.PageIndex = pagination.Page;
            dTO.PageSize = pagination.Rows;

            string orderField = pagination.Sidx;
            FieldSortType sord = (string.IsNullOrWhiteSpace(pagination.Sord) || pagination.Sord.Trim().ToLower() == "asc") ? FieldSortType.Asc : FieldSortType.Desc;
            List<OrderByField> orderByFields = new List<OrderByField>();

            string[] orders = orderField.Split(',');

            foreach (string order in orders)
            {
                string orderPart = order;
                orderPart = Regex.Replace(orderPart, @"\s+", " ");
                string[] orderArry = orderPart.Split(' ');
                string orField = orderArry[0];
                FieldSortType isAsc;
                if (orderArry.Length == 2)
                {
                    isAsc = orderArry[1].ToUpper() == "ASC" ? FieldSortType.Asc : FieldSortType.Desc;
                }
                else
                {
                    isAsc = sord;
                }
                orderByFields.Add(new OrderByField(orField, isAsc));
            }

            dTO.Order = orderByFields.ToArray();
            return dTO;
        }
    }
}
