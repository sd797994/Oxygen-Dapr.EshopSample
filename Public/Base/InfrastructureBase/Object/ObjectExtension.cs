using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace InfrastructureBase.Object
{
    public static class ObjectExtension
    {
        public static Tout CopyTo<Tin, Tout>(this Tin model) where Tin : class where Tout : class
        {
            return ExtensionMapper<Tin, Tout>.Map(model);
        }
        public static IEnumerable<Tout> CopyTo<Tin, Tout>(this IEnumerable<Tin> model) where Tin : class where Tout : class
        {
            foreach (var item in model)
            {
                yield return ExtensionMapper<Tin, Tout>.Map(item);
            }
        }
        public static object GetPropertyValue<T>(this T t, string name)
        {
            Type type = t.GetType();
            PropertyInfo p = type.GetProperty(name);
            if (p == null)
            {
                return null;
            }
            ParameterExpression param_obj = Expression.Parameter(typeof(T));
            UnaryExpression body;
            if (typeof(T).BaseType == null)
            {
                body = Expression.Convert(Expression.Property(Expression.Convert(param_obj, type), name), typeof(object));
            }
            else
            {
                body = Expression.Convert(Expression.Property(param_obj, p), typeof(object));
            }
            var result = Expression.Lambda<Func<T, object>>(body, param_obj);
            var getValue = result.Compile();
            return getValue(t);
        }
    }
}
