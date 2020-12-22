using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FDI.Simple;

namespace FDI.DA
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> source, int page, int row, ref int total)
        {
            total = source.Count();
            var methodCallExpression = Expression.Call(typeof(Queryable), "Skip", new[] { source.ElementType }, source.Expression, Expression.Constant((page - 1) * row));
            source = source.Provider.CreateQuery<T>(methodCallExpression);
            methodCallExpression = Expression.Call(typeof(Queryable), "Take", new[] { source.ElementType }, source.Expression, Expression.Constant(row));
            source = source.Provider.CreateQuery<T>(methodCallExpression);
            return source;
        }
        public static List<SimItem> GetPage(IEnumerable<SimItem> list, int page, int pageSize)
        {
            var listn = list.Skip(page * pageSize).Take(pageSize).ToList();
            return listn;
        }

        public static IQueryable<T> SelectByRequest<T>(this IQueryable<T> source, ParramRequest request, ref int totalRecord)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                source = request.SearchInField.Aggregate(source, (current, m) => current.HasOne(m, request.Keyword));
            }
            if (!string.IsNullOrEmpty(request.FieldSort))
            {
                var propertyName = request.FieldSort;
                var methodName = (!request.TypeSort) ? "OrderByDescending" : "OrderBy";
                source = source.SortBy(propertyName, methodName);
            }
            totalRecord = source.Count();
            if (request.CurrentPage > 0 && request.RowPerPage > 0)
            {
                var methodCallExpression = Expression.Call(typeof(Queryable), "Skip", new[] { source.ElementType }, source.Expression, Expression.Constant((request.CurrentPage - 1) * request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);
                methodCallExpression = Expression.Call(typeof(Queryable), "Take", new[] { source.ElementType }, source.Expression, Expression.Constant(request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);
            }
            return source;
        }
        
        public static IQueryable<T> SelectByRequest<T>(this IQueryable<T> source, ParramRequest request)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                source = request.SearchInField.Aggregate(source, (current, propSearch) => current.HasOne(propSearch, request.Keyword));
            }
            return source;
        }
        public static IQueryable<T> SelectPageByRequest<T>(this IQueryable<T> source, ParramRequest request, ref int totalRecord)
        {
            totalRecord = source.Count();
            if (request.CurrentPage > 0 && request.RowPerPage > 0)
            {
                var methodCallExpression = Expression.Call(typeof(Queryable), "Skip", new[] { source.ElementType }, source.Expression, Expression.Constant((request.CurrentPage - 1) * request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);
                methodCallExpression = Expression.Call(typeof(Queryable), "Take", new[] { source.ElementType }, source.Expression, Expression.Constant(request.RowPerPage));
                source = source.Provider.CreateQuery<T>(methodCallExpression);
            }
            return source;
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName, string methodName)
        {
            var parameter = Expression.Parameter(source.ElementType, String.Empty);
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);
            var methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                          new[] { source.ElementType, property.Type },
                                                          source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<T> HasOne<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            if (source == null || string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(keyword))
            {
                return source;
            }
            keyword = keyword.ToLower();

            var parameter = Expression.Parameter(source.ElementType, String.Empty);
            var property = Expression.Property(parameter, propertyName);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[] { });
            var typeProperty = property.Type.FullName;
            var termConstant = Expression.Constant(keyword, typeof(string));

            MethodCallExpression containsExpression = null;
            if (typeProperty == "System.String")
            {
                var toLowerExpression = Expression.Call(property, toLowerMethod);
                containsExpression = Expression.Call(toLowerExpression, containsMethod, termConstant);
            }
            else if (typeProperty == "System.Int32")
            {
                try
                {
                    var keywordInt = Convert.ToInt32(keyword);
                    termConstant = Expression.Constant(keywordInt, typeof(int));
                    containsMethod = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                    containsExpression = Expression.Call(property, containsMethod, termConstant);
                }
                catch
                {
                    return Enumerable.Empty<T>().AsQueryable();
                }
            }
            else if (typeProperty == "System.Int64")
            {
                try
                {
                    var keywordInt = Convert.ToInt64(keyword);
                    termConstant = Expression.Constant(keywordInt, typeof(long));
                    containsMethod = typeof(long).GetMethod("Equals", new[] { typeof(long) });
                    containsExpression = Expression.Call(property, containsMethod, termConstant);
                }
                catch
                {
                    return Enumerable.Empty<T>().AsQueryable();
                }
            }
            else if (typeProperty == "System.Decimal")
            {
                try
                {
                    var keywordInt = Convert.ToDecimal(keyword);
                    termConstant = Expression.Constant(keywordInt, typeof(decimal));
                    containsMethod = typeof(decimal).GetMethod("Equals", new[] { typeof(decimal) });
                    containsExpression = Expression.Call(property, containsMethod, termConstant);
                }
                catch { source = Enumerable.Empty<T>().AsQueryable(); }
            }
            var predicate = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            var methodCallExpression = Expression.Call(typeof(Queryable), "Where",
                                        new[] { source.ElementType },
                                        source.Expression, Expression.Quote(predicate));
            source = source.Provider.CreateQuery<T>(methodCallExpression);
            return source;
        }

        /// Cách dùng:
        /// list.Where("id", "eq", "12345");
        ///list.Where("id", "lt", "5468");
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string searchProperty, string searchOper, string searchString)
    where TEntity : class
        {
            var type = typeof(TEntity);

            var searchFilter = Expression.Constant(searchString);

            var parameter = Expression.Parameter(type, "p");
            var property = type.GetProperty(searchProperty);
            Expression propertyAccess = Expression.MakeMemberAccess(parameter, property);

            //support int?
            if (property.PropertyType == typeof(int?))
            {
                var valProp = typeof(int?).GetProperty("Value");
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, valProp);

                int? tn = Int32.Parse(searchString);
                searchFilter = Expression.Constant(tn);
            }

            //support decimal?
            if (property.PropertyType == typeof(decimal?))
            {
                var valProp = typeof(decimal?).GetProperty("Value");
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, valProp);

                decimal? tn = Decimal.Parse(searchString);
                searchFilter = Expression.Constant(tn);
            }

            if (propertyAccess.Type == typeof(Int32))
                searchFilter = Expression.Constant(Int32.Parse(searchString));

            if (propertyAccess.Type == typeof(decimal))
                searchFilter = Expression.Constant(Decimal.Parse(searchString));


            var startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var endsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            Expression operation = null;

            switch (searchOper)
            {
                default:
                case "eq":
                    operation = Expression.Equal(propertyAccess, searchFilter);
                    break;
                case "ne":
                    operation = Expression.NotEqual(propertyAccess, searchFilter);
                    break;
                case "lt":
                    operation = Expression.LessThan(propertyAccess, searchFilter);
                    break;
                case "le":
                    operation = Expression.LessThanOrEqual(propertyAccess, searchFilter);
                    break;
                case "gt":
                    operation = Expression.GreaterThan(propertyAccess, searchFilter);
                    break;
                case "ge":
                    operation = Expression.GreaterThanOrEqual(propertyAccess, searchFilter);
                    break;
                case "bw":
                    operation = Expression.Call(propertyAccess, startsWith, searchFilter);
                    break;
                case "bn":
                    operation = Expression.Call(propertyAccess, startsWith, searchFilter);
                    operation = Expression.Not(operation);
                    break;
                case "ew":
                    operation = Expression.Call(propertyAccess, endsWith, searchFilter);
                    break;
                case "en":
                    operation = Expression.Call(propertyAccess, endsWith, searchFilter);
                    operation = Expression.Not(operation);
                    break;
                case "cn":
                    operation = Expression.Call(propertyAccess, contains, searchFilter);
                    break;
                case "nc":
                    operation = Expression.Call(propertyAccess, contains, searchFilter);
                    operation = Expression.Not(operation);
                    break;
            }

            var whereExpression = Expression.Lambda(operation, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), "Where", new[] { source.ElementType }, source.Expression, whereExpression);

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
