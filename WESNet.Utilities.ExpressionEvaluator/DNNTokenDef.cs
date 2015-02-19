using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class DNNTokenDefinition : IdentifierDefinition
    {
        private static string variablePattern = @"^\s*(?<object>.*(?=:))?(:?(?<property>[^|=]+))(\|(?<format>.+?(\\[|=].*?)*))?([|=](?<default>.*))?$";

        private static string typePattern = @"^(?<bool>true|false)|" +
                                            @"#(?<datetime>.*?)#|" +
                                            @"(?<double>[0-9]+\.[0-9]*)|" +
                                            @"(?<integer>\d+)|" +
                                            @"(?:"")(?<string>.*?)(?:"")\s*$";

        private static Regex variableRegex = new Regex(variablePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex typeRegex = new Regex(typePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public string ObjectName { get; set; }
        public string PropertyName { get; set; }
        public string Format { get; set; }
        public string Default { get; set; }

        public bool IsResolved { get; set; }

        public override string FullyQualifiedName
        {
            get
            {
                return (ObjectName +":") + PropertyName;
            }
            set
            {
                var names = value.Split(':');
                if (names.Length == 1)
                {
                    ObjectName = null;
                    PropertyName = names[0];
                }
                else if (names.Length == 2)
                {
                    ObjectName = names[0];
                    PropertyName = names[1];
                }
                else
                {
                    throw new EvaluatorException("Invalid full name found in token [" + value + "]");
                }
            }
        }

        public DNNTokenDefinition (string tokenValue) : base (tokenValue, Enums.IdentifierDefinitionTypes.DNNToken)
        {
            var match = variableRegex.Match((string)tokenValue);
            if (match.Success)
            {
                ObjectName = match.Groups["object"] == null ? null : match.Groups["object"].Value;
                PropertyName = match.Groups["property"] == null ? null : match.Groups["property"].Value;
                Format = match.Groups["format"] == null ? null : Regex.Replace(match.Groups["format"].Value, @"(\\)([|=])", "$2");
                Default = match.Groups["default"] == null ? null : match.Groups["default"].Value;       
            }
        }

        private T GetValue<T>(string literal)
        {
            object value;
            
            try
            {
                Type type = GetType(literal, out value);
                return (T)Convert.ChangeType(value, type);
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        private Type GetType(string literal, out object value)
        {   
            var matches = typeRegex.Matches(literal);
       
            Type type;

            foreach (Match match in matches)
            {
                int i = 0;
                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    bool success = group.Success && i > 0;      // ignore capture index 0 and 1 (general and WhiteSpace)
                    if (success)
                    {
                        string groupName = typeRegex.GroupNameFromNumber(i);

                        switch (groupName)
                        {
                            case "bool":
                                value = bool.Parse(matchValue);
                                type = typeof(System.Boolean);
                                break;
                            case "datetime":
                                value = DateTime.Parse(matchValue);
                                type = typeof(System.DateTime);
                                break;
                            case "double":
                                value = double.Parse(matchValue);
                                type = typeof(System.Double);
                                break;
                            case "integer":
                                value = int.Parse(matchValue);
                                type = typeof(System.Int32);
                                break;
                            case "string":
                                value = matchValue;
                                type = typeof(System.String);
                                break;
                            default:
                                value = matchValue;
                                type = typeof(System.String);
                                break;
                        }
                        return type;
                    }
                }
            }
            value = null;
            return typeof(System.Object);
        }

        public bool Resolve(object dataSource)
        {
            var resolved = false;
            if (dataSource != null)
            {


            }
            return resolved;
        }

        public override string ToString()
        {
            return "         Name: " + Name.ToString() + "\n\r" +
                   "   ObjectName: " + (ObjectName ?? "N/A") + "\n\r" +
                   " PropertyName: " + (PropertyName ?? "N/A") + "\n\r" +
                   "       Format: " + (Format ?? "N/A") + "\n\r" +
                   "      Default: " + (Default ?? "N/A") + "\n\r" +
                   "        Value: " + (IsResolved ? Value.ToString() : "N/A") + "\n\r" +
                   "   Value Type: " + (IsResolved && ValueType != null ? ValueType.FullName : "N/A");
        }

    }
}
