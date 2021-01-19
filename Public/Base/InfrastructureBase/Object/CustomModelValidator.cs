using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InfrastructureBase.Object
{
    public class CustomModelValidator
    {
        public static void Valid(object obj)
        {
            var asEnumerable = obj as IEnumerable;
            if (asEnumerable != null)
            {
                foreach (var enumObj in asEnumerable)
                {
                    var validResults = new CustomModelValidator().Valid(enumObj);
                    if (validResults.Count > 0)
                    {
                        throw new InfrastructureException(validResults[0].ErrorMessage);
                    }
                }
            }
            else
            {
                var validResults = new CustomModelValidator().Valid(obj);
                if (validResults.Count > 0)
                {
                    throw new InfrastructureException(validResults[0].ErrorMessage);
                }
            }
        }
        internal List<ValidationResult> Valid<T>(T instance)
        {
            var validator = new DataAnnotationsValidator();
            var validationResults = new List<ValidationResult>();
            validator.TryValidateObjectRecursive(instance, validationResults);
            return validationResults;
        }
        internal class DataAnnotationsValidator
        {
            public bool TryValidateObject<T>(T obj, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null)
            {
                return Validator.TryValidateObject(obj, new ValidationContext(obj, null, validationContextItems), results, true);
            }

            public bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results, IDictionary<object, object> validationContextItems = null)
            {
                return TryValidateObjectRecursive(obj, results, new HashSet<object>(), validationContextItems);
            }

            private bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results, ISet<object> validatedObjects, IDictionary<object, object> validationContextItems = null)
            {
                if (validatedObjects.Contains(obj))
                {
                    return true;
                }

                validatedObjects.Add(obj);
                bool result = TryValidateObject(obj, results, validationContextItems);

                var properties = obj.GetType().GetProperties().Where(prop => prop.CanRead
                    && prop.GetIndexParameters().Length == 0).ToList();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType) continue;

                    var value = obj.GetPropertyValue(property.Name); 

                    if (value == null) continue;

                    var asEnumerable = value as IEnumerable;
                    if (asEnumerable != null)
                    {
                        foreach (var enumObj in asEnumerable)
                        {
                            if (enumObj != null)
                            {
                                var nestedResults = new List<ValidationResult>();
                                if (!TryValidateObjectRecursive(enumObj, nestedResults, validatedObjects, validationContextItems))
                                {
                                    result = false;
                                    foreach (var validationResult in nestedResults)
                                    {
                                        PropertyInfo property1 = property;
                                        results.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                                    }
                                };
                            }
                        }
                    }
                    else
                    {
                        var nestedResults = new List<ValidationResult>();
                        if (!TryValidateObjectRecursive(value, nestedResults, validatedObjects, validationContextItems))
                        {
                            result = false;
                            foreach (var validationResult in nestedResults)
                            {
                                PropertyInfo property1 = property;
                                results.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                            }
                        };
                    }
                }
                return result;
            }
        }
    }
}
