using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class ParameterDefinition
    {
        private static string parameterRegexExpression = @"(?<type>[a-zA-Z]+\w*(\.[a-zA-Z]+\w*)*)\s+(?<name>[a-zA-Z]+\w*)";
        private static Regex parameterRegex = new Regex(parameterRegexExpression, RegexOptions.Compiled);

        public Type ParameterType { get; private set; }

        public string ParameterTypeName { get; private set; }

        public string ParameterName { get; private set; }

        public ParameterDefinition(string tokenValue)
        {
            var matches = parameterRegex.Matches(tokenValue);
            foreach (Match match in matches)
            {
                int i = 0;
                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    bool success = group.Success && i > 0;      // ignore capture index 0 and 1 (general and WhiteSpace)
                    if (success)
                    {
                        string groupName = parameterRegex.GroupNameFromNumber(i);
                        switch (groupName)
                        {
                            case "type":
                                ParameterTypeName = group.Value;
                                ParameterType = Type.GetType(group.Value);
                                break;
                            case "name":
                                ParameterName = group.Value;
                                break;
                        }
                    }
                    i++;
                }
            }
        }

        public ParameterDefinition(string parameterTypeName, string parameterName)
        {
            ParameterTypeName = parameterTypeName;
            ParameterType = Type.GetType(parameterTypeName);
            ParameterName = ParameterName;
        }

        public override string ToString()
        {
            return ParameterTypeName + " " + ParameterName;
        }
    }
}
