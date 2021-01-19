using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Object
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TTo, bool>> ReplaceParameter<TFrom, TTo>(this Expression<Func<TFrom, bool>> target)
        {
            return (Expression<Func<TTo, bool>>)new WhereReplacerVisitor<TFrom, TTo>().Visit(target);
        }
        public static Expression<Func<TTo, object>> ReplaceParameter<TFrom, TTo>(this Expression<Func<TFrom, dynamic>> target)
        {
            return (Expression<Func<TTo, object>>)new WhereReplacerDynamicVisitor<TFrom, TTo>().Visit(target);
        }
        private class WhereReplacerVisitor<TFrom, TTo> : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTo));
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                return Expression.Lambda<Func<TTo, bool>>(Visit(node.Body), _parameter);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if ((node.Member.DeclaringType == typeof(Entity) || node.Member.DeclaringType == typeof(TFrom)) && node.Expression is ParameterExpression)
                {
                    return Expression.Property(_parameter, node.Member.Name);
                }
                return base.VisitMember(node);
            }
        }
        private class WhereReplacerDynamicVisitor<TFrom, TTo> : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTo));
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                return Expression.Lambda<Func<TTo, dynamic>>(Visit(node.Body), _parameter);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if ((node.Member.DeclaringType == typeof(Entity) || node.Member.DeclaringType == typeof(TFrom)) && node.Expression is ParameterExpression)
                {
                    return Expression.Property(_parameter, node.Member.Name);
                }
                return base.VisitMember(node);
            }
        }
        internal class ConvertVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression Parameter;

            public ConvertVisitor(ParameterExpression parameter)
            {
                Parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression item)
            {
                // we just check the parameter to return the new value for them
                if (!item.Name.Equals(Parameter.Name))
                    return item;
                return Parameter;
            }
        }
    }
}
