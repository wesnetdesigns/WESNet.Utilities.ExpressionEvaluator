using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class MethodDefinition : IdentifierDefinition
    {
        private List<ParameterDefinition> _ParameterList = new List<ParameterDefinition>();

        public bool IsStatic { get; set; }

        public Type ReturnType { get; set; }

        public string Parameters { get; set; }

        public List<ParameterDefinition> ParameterList
        {
            get
            {
                return _ParameterList;
            }
        }

        public MethodDefinition(string fullname) : base(fullname, Enums.IdentifierDefinitionTypes.Method) {}

        public MethodDefinition(string fullname, params ParameterDefinition[] parameters) : base(fullname,Enums.IdentifierDefinitionTypes.Method)
        {
            ParameterList.AddRange(parameters);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (ReturnType != null) sb.Append(ReturnType.FullName + " ");
            sb.Append(FullyQualifiedName + "(");
            foreach (ParameterDefinition p in ParameterList)
            {
                sb.Append(p.ToString());
                sb.Append(", ");
            }
            if (sb.Length > Name.Length + 3) sb.Length -= 2;
            sb.Append(")");
            return sb.ToString();
        }

        public dynamic Invoke (dynamic instance, params dynamic[] args)
        {
            Type type = null; ;
            MethodInfo methodInfo = null;

            var parameterTypes = ParameterList.Select(p=>p.ParameterType).ToArray();
            var argTypes = args.Select(a=>a.GetType()).ToArray();

            if (argTypes.Length != parameterTypes.Length)
            {
                throw new EvaluatorException (new EvaluatorError(350, string.Format("Incorrect number of arguments ({0}) passed to method {1}.", args.Length, ToString())));
            }

            for(var i=0; i<args.Length; i++)
            {
                if (argTypes[i].FullName != parameterTypes[i].FullName)
                {
                    throw new EvaluatorException(new EvaluatorError(351, String.Format("Type mismatch ({0}) in parameter {1} passed to method {2}.", argTypes[i].FullName, ParameterList[i].ParameterName, ToString())));
                }
            }

            if (string.IsNullOrEmpty(TypeName))
            {
                foreach (string typeName in Constants.Usings)
                {
                    type = Type.GetType(typeName);
                    if (type != null)
                    {
                        methodInfo = type.GetMethod(Name, parameterTypes);
                        if (methodInfo != null)
                        {
                            FullyQualifiedName = typeName + '.' + Name;
                            break;
                        }
                    }
                }
            }
            else
            {
                type = Type.GetType(TypeName.TrimEnd('.'));
                methodInfo = type.GetMethod(Name, parameterTypes);
            }

            if (type != null && methodInfo != null)
            {
                try
                {
                    if (instance == null && !methodInfo.IsStatic)
                    {
                        instance = Activator.CreateInstance(type);
                    }

                    return methodInfo.Invoke(instance, args);
                }
                catch (Exception exc)
                {
                    throw new EvaluatorException(new EvaluatorError(351, "Unable to invoke method " + Name + ". Error: " + exc.Message));
                }
            }
            return null;
        }
    }
}
