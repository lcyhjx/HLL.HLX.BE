using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Abp.Linq.Extensions;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Common.Util
{
    public static class QueryUtil
    {
        /// <summary>
        /// 从IQueryable中获得Sql查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetQuerySql<T>(IQueryable<T> query) where T : class
        {
            const string debugSeperator = "-------------------------------------";
            string sql = string.Empty;

            try
            {
                ObjectQuery<T> objectQuery = query as ObjectQuery<T>;
                if (objectQuery != null)
                {
                    StringBuilder queryString = new StringBuilder();
                    queryString.Append(Environment.NewLine)
                        .AppendLine("QUERY GENERATED...")
                        .AppendLine(debugSeperator)
                        .AppendLine(objectQuery.ToTraceString())
                        .AppendLine("PARAMETERS...")
                        .AppendLine(debugSeperator);
                    foreach (ObjectParameter parameter in objectQuery.Parameters)
                    {
                        queryString.Append(String.Format("{0}({1}) \t- {2}", parameter.Name, parameter.ParameterType, parameter.Value)).Append(Environment.NewLine);
                    }
                    queryString.AppendLine(debugSeperator).Append(Environment.NewLine);

                    sql = queryString.ToString();
                }
            }
            catch (Exception)
            {
                sql = String.Empty;
            }
            return sql;
        }

        /// <summary>
        /// 从ObjectQuery中获得Sql语句
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public static string GetQuerySql(ObjectQuery objectQuery)
        {
            const string debugSeperator = "-------------------------------------";
            string sql = string.Empty;

            try
            {
                if (objectQuery != null)
                {
                    StringBuilder queryString = new StringBuilder();
                    queryString.Append(Environment.NewLine)
                        .AppendLine("QUERY GENERATED...")
                        .AppendLine(debugSeperator)
                        .AppendLine(objectQuery.ToTraceString())
                        .AppendLine("PARAMETERS...")
                        .AppendLine(debugSeperator);
                    foreach (ObjectParameter parameter in objectQuery.Parameters)
                    {
                        queryString.Append(String.Format("{0}({1}) \t- {2}", parameter.Name, parameter.ParameterType, parameter.Value)).Append(Environment.NewLine);
                    }
                    queryString.AppendLine(debugSeperator).Append(Environment.NewLine);

                    sql = queryString.ToString();
                }
            }
            catch (Exception)
            {
                sql = String.Empty;
            }

            return sql;
        }


        /// <summary>
        /// 从指定对象集合获取分页结果集及当前页与总页数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">源集合数据</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public static List<T> GetPagingResult<T>(List<T> sourceList, int pageSize, ref int pageIndex, out int pageCount)
        {
            if (sourceList == null || sourceList.Count == 0)
            {
                pageIndex = -1;
                pageCount = 0;
                return null;
            }

            var totalCount = sourceList.Count;
            pageCount = (totalCount + pageSize - 1) / pageSize;

            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            else if (pageIndex > pageCount - 1)
            {
                pageIndex = pageCount - 1;
            }

            return sourceList.Skip(pageSize * pageIndex).Take(pageSize).ToList();
        }

        /// <summary>
        /// 从查询中获取指定页结果集合及当前页、总页数和总记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询语句</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="totalCount">总数据条数</param>
        /// <returns></returns>
        public static List<T> GetPagingResult<T>(IQueryable<T> query, int pageSize, ref int pageIndex, out int pageCount, out int totalCount)
        {
            if (query == null)
            {
                pageIndex = -1;
                pageCount = 0;
                totalCount = 0;
                return null;
            }

            totalCount = query.Count();
            if (totalCount == 0)
            {
                pageIndex = -1;
                pageCount = 0;
                return null;
            }

            pageCount = (totalCount + pageSize - 1) / pageSize;

            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            else if (pageIndex > pageCount - 1)
            {
                pageIndex = pageCount - 1;
            }

            return query.PageBy(pageSize * pageIndex, pageSize).ToList();
        }
    }
}
