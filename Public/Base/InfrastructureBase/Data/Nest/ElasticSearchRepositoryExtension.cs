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
                impl.searchParams.From((pageIndex - 1) * pageSize).Size(pageSize);
                impl.InitPage = true;
            }
            return impl;
        }
        public static IElasticSearchRepository<T> Sort<T>(this IElasticSearchRepository<T> repo, Expression<Func<T, dynamic>> sortfunc, bool desc) where T : class
        {
            var impl = repo as ElasticSearchRepository<T>;
            if (sortfunc != null)
            {
                var memberprop = GetMember(sortfunc.Body);
                var sortName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                if (desc)
                    impl.sortQueries.Descending(sortName);
                else
                    impl.sortQueries.Ascending(sortName);
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
                    var memberprop = GetMember(be.Left);
                    paramName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                    if (memberprop.propertyInfo.PropertyType == typeof(DateTime) || memberprop.propertyInfo.PropertyType == typeof(DateTime?))
                        isDate = true;
                    ExpressionType type = be.NodeType;
                    paramValue = GetValue(be.Right);
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
                        string paramName;
                        dynamic paramValue;
                        if (me.Arguments.Count > 1)//array
                        {
                            var memberprop = GetMember(me.Arguments[1]);
                            paramName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                            paramValue = GetValue(me.Arguments[0] as MemberExpression);
                            Func<QueryContainerDescriptor<T>, QueryContainer> rangefunc = (x) => x.Terms(r => r.Field(paramName).Terms(paramValue));
                            impl.mustQueries.Add(rangefunc);
                        }
                        else
                        {
                            if (me.Arguments[0] is MemberExpression) //property
                            {
                                if ((me.Object as MemberExpression).Member is PropertyInfo)
                                {
                                    var memberprop = GetMember(query.Body);
                                    paramName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                                    paramValue = GetValue(me.Arguments[0]);
                                    Func<QueryContainerDescriptor<T>, QueryContainer> containsfunc = (x) => x.Wildcard(r => r.Field(paramName).Value($"*{paramValue}*"));
                                    impl.mustQueries.Add(containsfunc);
                                }
                                else  //list
                                {
                                    var memberprop = GetMember(me.Arguments[0]);
                                    paramName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                                    paramValue = Expression.Lambda((query.Body as MethodCallExpression).Object).Compile().DynamicInvoke();
                                    Func<QueryContainerDescriptor<T>, QueryContainer> rangefunc = (x) => x.Terms(r => r.Field(paramName).Terms(paramValue));
                                    impl.mustQueries.Add(rangefunc);
                                }
                            }
                            else if (me.Arguments[0] is ConstantExpression) //string
                            {
                                var memberprop = GetMember(query.Body);
                                paramName = CamelCase(memberprop.memberInfo.Name, memberprop.propertyInfo.PropertyType);
                                paramValue = GetValue(me.Arguments[0]);
                                Func<QueryContainerDescriptor<T>, QueryContainer> containsfunc = (x) => x.Wildcard(r => r.Field(paramName).Value($"*{paramValue}*"));
                                impl.mustQueries.Add(containsfunc);
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
        static object GetValue(Expression expression)
        {
            if (expression is ConstantExpression)
                return GetValue(expression as ConstantExpression);
            else if (expression is MemberExpression)
                return GetValue(expression as MemberExpression);
            else if (expression is UnaryExpression)
                return GetValue(expression as UnaryExpression);
            else
                return Expression.Lambda(expression).Compile().DynamicInvoke();
        }
        static object GetValue(ConstantExpression methodCall)
        {
            return methodCall.Value;
        }
        static object GetValue(UnaryExpression unary)
        {
            if (unary.Operand is MemberExpression)
                return GetValue(unary.Operand as MemberExpression);
            else
                return GetValue(unary as Expression);
        }

        static string CamelCase(string sourceStr, Type type = null)
        {
            if (sourceStr.Length == 1)
                return sourceStr.AsSpan()[0].ToString().ToLower();
            var result = sourceStr.AsSpan()[0].ToString().ToLower() + sourceStr.AsSpan()[1..].ToString() + (type == typeof(string) ? ".keyword" : "");
            if (result == "id")
                return "_id";
            return result;
        }
        static (MemberInfo memberInfo, PropertyInfo propertyInfo) GetMember(Expression func)
        {
            MemberInfo memberInfo = default;
            PropertyInfo propertyInfo;
            if(func is UnaryExpression)
            {
                func = (func as UnaryExpression).Operand;
            }
            if (func is MemberExpression)
            {
                memberInfo = (func as MemberExpression).Member;
            }
            else if (func is MethodCallExpression)
            {
                memberInfo = ((func as MethodCallExpression).Object as MemberExpression).Member;
            }
            propertyInfo = memberInfo as PropertyInfo;
            return (memberInfo, propertyInfo);
        }
        #endregion
    }
}
