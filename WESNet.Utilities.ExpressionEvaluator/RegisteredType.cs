using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class RegisteredType
    {
        private Dictionary<string, MethodInfo> _ConvertsTo = new Dictionary<string, MethodInfo>();

        public string FullName { get; set; }
        public Dictionary<string, MethodInfo> ConvertsTo
        {
            get
            {
                return _ConvertsTo;
            }
        }

        public RegisteredType(string fullName, params string[] convertsToFilter)
        {
            FullName = fullName;

            try
            {
                var fromType = Type.GetType(fullName);
                var convertToMethods = typeof(System.Convert).GetMethods();
                for (int j = 0; j < convertToMethods.Length; j++)
                {
                    var method = convertToMethods[j];
                    if (convertsToFilter == null || convertsToFilter.Contains(method.ReturnType.FullName))
                    {
                        if (method.IsStatic && method.GetParameters().Where(p => p.ParameterType.FullName == fullName).Count() > 0)
                        {
                            var returnTypeName = method.ReturnType.FullName;
                            if (!ConvertsTo.ContainsKey(returnTypeName))
                            {
                                ConvertsTo.Add(returnTypeName, method);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                var message = exc.Message;
            }
        }

        public bool CanConvertTo(string convertToTypeName)
        {
            return ConvertsTo.ContainsKey(convertToTypeName);
        }

        public T ConvertTo<T>(dynamic value)
        {
            var convertToTypeName = typeof(T).FullName;
            if (CanConvertTo(convertToTypeName))
            {
                var methodInfo = ConvertsTo[convertToTypeName];
                object[] parameters = new object[] { value };
                try
                {
                    return (T)(methodInfo.Invoke(null, parameters));
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}
