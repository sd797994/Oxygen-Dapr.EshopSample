using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Data.Nest
{
    public static class ElasticSearchRepositoryExtension
    {
        public static IElasticSearchRepository<T> Page<T>(this IElasticSearchRepository<T> repo, int pageSize, int pageIndex) where T : class
        {
            var impl = repo as ElasticSearchRepository<T>;
            if (!impl.InitPage)
            {
                impl.searchParams.From(pageIndex * pageSize).Size(pageSize);
                impl.InitPage = true;
            }
            return impl;
        }
        public static IElasticSearchRepository<T> Sort<T>(this IElasticSearchRepository<T> repo, Expression<Func<T, dynamic>> sortfunc, bool desc) where T : class
        {
            var impl = repo as ElasticSearchRepository<T>;
            if (sortfunc != null)
            {
                string sortName;
                Type sortNameType;
                if (sortfunc.Body is UnaryExpression)
                {
                    sortName = CamelCase(((MemberExpression)((UnaryExpression)sortfunc.Body).Operand).Member.Name);
                    sortNameType = (((MemberExpression)((UnaryExpression)sortfunc.Body).Operand).Member as PropertyInfo).PropertyType;
                }
                else
                {
                    sortName = CamelCase(((MemberExpression)sortfunc.Body).Member.Name);
                    sortNameType = (((MemberExpression)sortfunc.Body).Member as PropertyInfo).PropertyType;
                }
                if (desc)
                    impl.sortQueries.Descending($"{sortName}{(sortNameType == typeof(string) ? ".keyword" : "")}");
                else
                    impl.sortQueries.Ascending($"{sortName}{(sortNameType == typeof(string) ? ".keyword" : "")}");
            }
            return impl;
        }
        public static IElasticSearchRepository<T> Query<T>(this IElasticSearchRepository<T> repo, Expression<Func<T, bool>> query) where T : class
        {
            var impl = repo as ElasticSearchRepository<T>;
            if (query != null)
            {
                if (query.Body is BinaryExpression)
                {
                    BinaryExpression be = query.Body as BinaryExpression;
                    var paramName = "";
                    object paramValue = "";
                    bool isDate = false;
                    if (be.Left is UnaryExpression)
                    {
                        paramName = CamelCase(((MemberExpression)((UnaryExpression)be.Left).Operand).Member.Name);
                        if ((((MemberExpression)((UnaryExpression)be.Left).Operand).Member as PropertyInfo).PropertyType == typeof(DateTime) ||
                            (((MemberExpression)((UnaryExpression)be.Left).Operand).Member as PropertyInfo).PropertyType == typeof(DateTime?))
                            isDate = true;
                    }
                    else
                    {
                        paramName = CamelCase(((MemberExpression)be.Left).Member.Name);
                        if ((((MemberExpression)be.Left).Member as PropertyInfo).PropertyType == typeof(DateTime) ||
                            (((MemberExpression)be.Left).Member as PropertyInfo).PropertyType == typeof(DateTime?))
                            isDate = true;
                    }
                    ExpressionType type = be.NodeType;
                    if (be.Right is ConstantExpression)
                    {
                        paramValue = ((ConstantExpression)be.Right).Value;
                    }
                    else if (be.Right is MemberExpression)
                    {
                        paramValue = GetValue((MemberExpression)be.Right);
                    }
                    else if (be.Right is MethodCallExpression)
                    {
                        paramValue = Expression.Lambda(be.Right).Compile().DynamicInvoke();
                    }
                    else
                    {
                        if (((UnaryExpression)be.Right).Operand is not MethodCallExpression)
                            paramValue = GetValue((MemberExpression)((UnaryExpression)be.Right).Operand);
                        else
                            paramValue = Expression.Lambda(be.Right).Compile().DynamicInvoke();
                    }
                    Func<QueryContainerDescriptor<T>, QueryContainer> func = (x) => x.Term(paramName, paramValue);
                    Func<QueryContainerDescriptor<T>, QueryContainer> rangefunc;
                    switch (type)
                    {
                        case ExpressionType.Equal:
                            impl.mustQueries.Add(func);
                            break;
                        case ExpressionType.NotEqual:
                            impl.mustnotQueries.Add(func);
                            break;
                        case ExpressionType.GreaterThan:
                            if (!isDate)
                                rangefunc = (x) => x.Range(r => r.Field(paramName).GreaterThan(Convert.ToDouble(paramValue)));
                            else
                                rangefunc = (x) => x.DateRange(r => r.Field(paramName).GreaterThan((DateTime)paramValue));
                            impl.rangeQueries.Add(rangefunc);
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            if (!isDate)
                                rangefunc = (x) => x.Range(r => r.Field(paramName).GreaterThanOrEquals(Convert.ToDouble(paramValue)));
                            else
                                rangefunc = (x) => x.DateRange(r => r.Field(paramName).GreaterThanOrEquals((DateTime)paramValue));
                            impl.rangeQueries.Add(rangefunc);
                            break;
                        case ExpressionType.LessThan:
                            if (!isDate)
                                rangefunc = (x) => x.Range(r => r.Field(paramName).LessThan(Convert.ToDouble(paramValue)));
                            else
                                rangefunc = (x) => x.DateRange(r => r.Field(paramName).LessThan((DateTime)paramValue));
                            impl.rangeQueries.Add(rangefunc);
                            break;
                        case ExpressionType.LessThanOrEqual:
                            if (!isDate)
                                rangefunc = (x) => x.Range(r => r.Field(paramName).LessThanOrEquals(Convert.ToDouble(paramValue)));
                            else
                                rangefunc = (x) => x.DateRange(r => r.Field(paramName).LessThanOrEquals((DateTime)paramValue));
                            impl.rangeQueries.Add(rangefunc);
                            break;
                    }
                }
                else if (query.Body is MethodCallExpression)//暂时只能处理Contains一种逻辑
                {
                    MethodCallExpression me = query.Body as MethodCallExpression;
                    if (me.Method.Name == "Contains")
                    {
                        if (me.Arguments.Count > 1)//array
                        {
                            var paramName = CamelCase((me.Arguments[1] as MemberExpression).Member.Name);
                            dynamic paramValue = GetValue(me.Arguments[0] as MemberExpression);
                            Func<QueryContainerDescriptor<T>, QueryContainer> rangefunc = (x) => x.Terms(r => r.Field(paramName).Terms(paramValue));
                            impl.mustQueries.Add(rangefunc);
                        }
                        else
                        {
                            if (me.Arguments[0] is MemberExpression)  //list
                            {
                                var paramName = CamelCase((me.Arguments[0] as MemberExpression).Member.Name);
                                dynamic paramValue = Expression.Lambda((query.Body as MethodCallExpression).Object).Compile().DynamicInvoke();
                                Func<QueryContainerDescriptor<T>, QueryContainer> rangefunc = (x) => x.Terms(r => r.Field(paramName).Terms(paramValue));
                                impl.mustQueries.Add(rangefunc);
                            }
                            else if (me.Arguments[0] is ConstantExpression) //string
                            {
                                var paramName = CamelCase(((query.Body as MethodCallExpression).Object as MemberExpression).Member.Name);
                                dynamic paramValue = (me.Arguments[0] as ConstantExpression).Value;
                                Func<QueryContainerDescriptor<T>, QueryContainer> likefunc = (x) => x.Wildcard(r => r.Field(paramName).Value($"*{paramValue}*"));
                                impl.mustQueries.Add(likefunc);
                            }
                        }
                    }
                }
            }
            return impl;
        }

        #region 私有方法
        static object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }
        static string CamelCase(string sourceStr)
        {
            if (sourceStr.Length == 1)
                return sourceStr.AsSpan()[0].ToString().ToLower();
            var result = sourceStr.AsSpan()[0].ToString().ToLower() + sourceStr.AsSpan()[1..].ToString();
            if (result == "id")
                return "_id";
            return result;
        }
        #endregion
    }
}
