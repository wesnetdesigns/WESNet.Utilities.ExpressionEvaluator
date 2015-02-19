using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class DNNToken : Operand
    {
        private static string variablePattern = @"^\s*(?<object>.*?(?=:))?(:?(?<property>[^|=]+?))(\|(?<format>.+?(\\[|=].*?)*))?([|=](?<default>.*))?$";

        private static string typePattern = @"^(?<bool>true|false)|" +
                                            @"#(?<datetime>.*?)#|" +
                                            @"(?<double>[0-9]+\.[0-9]*)|" +
                                            @"(?<integer>\d+)|" +
                                            @"(?:"")(?<string>.*?)(?:"")\s*$";

        private static Regex variableRegex = new Regex(variablePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex typeRegex = new Regex(typePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public event EventHandler<ProcessDnnTokenHandlerArgs> ProcessDNNToken;

        public string ObjectName { get; set; }
        public string PropertyName { get; set; }
        public string FullName { get; set; }
        public string Format { get; set; }
        public dynamic DefaultValue { get; set; }

        public bool IsResolved
        {
            get
            {
                return base.HasValue;
            }
        }
        
        public DNNToken(string tokenText, int characterPosition) : base(tokenText, characterPosition)
        {
            TokenType = Enums.TokenTypes.DNNToken;
            var match = variableRegex.Match(tokenText);
            if (match.Success)
            {
                ObjectName = match.Groups["object"] == null ? null : match.Groups["object"].Value;
                PropertyName = match.Groups["property"] == null ? null : match.Groups["property"].Value;
                FullName = (ObjectName != null ? ObjectName + ":" : "") + PropertyName;
                Format = match.Groups["format"] == null ? null : Regex.Replace(match.Groups["format"].Value, @"(\\)([|=])", "$2");
                var defaultValue = match.Groups["default"] == null ? null : match.Groups["default"].Value;
                DefaultValue = ParseValue(defaultValue);
            }
        }

        public virtual dynamic LoadValue()
        {
            if (ProcessDNNToken != null)
            {
                var eventArgs = new ProcessDnnTokenHandlerArgs()
                {
                    ProcessAction = Enums.ProcessActions.Load,
                    ObjectName = this.ObjectName,
                    PropertyName = this.PropertyName,
                    Format = this.Format
                };

                ProcessDNNToken(this, eventArgs);
                if (eventArgs.Error == null)
                {
                    Value = ParseValue(eventArgs.ReturnValue);
                    return Value;
                }
                else
                {
                    if (DefaultValue == null)
                    {
                        return TokenText;
                    }
                    else
                    {
                        return DefaultValue;
                    }
                }
            }
            else
            {
                return DefaultValue;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\r\n" +
                   "ObjectName: ".PadLeft(Constants.CaptionWidth) + (ObjectName ?? "N/A") + "\r\n" +
                   "PropertyName: ".PadLeft(Constants.CaptionWidth) + (PropertyName ?? "N/A") + "\r\n" +
                   "Format: ".PadLeft(Constants.CaptionWidth) + (Format ?? "N/A") + "\r\n" +
                   "Default Value: ".PadLeft(Constants.CaptionWidth) + (DefaultValue == null ? "N/A": DefaultValue.ToString()) + "\r\n" +
                   "Value: ".PadLeft(Constants.CaptionWidth) + (IsResolved ? Value.ToString() : "N/A") + "\r\n" +
                   "Value Type: ".PadLeft(Constants.CaptionWidth) + (IsResolved && OperandType != null ? OperandType.FullName : "N/A");
        }

        private dynamic ParseValue(string value)
        {
            if (value != null)
            {
                var match = typeRegex.Match(value);
                if (match.Success)
                {
                    for (var i=2; i< match.Groups.Count; i++)
                    {
                        var group = match.Groups[i];
                        if (group.Success)
                        {
                            string groupName = typeRegex.GroupNameFromNumber(i);
                            switch (groupName)
                            {
                                case "bool":
                                    return System.Boolean.Parse(group.Value);
                                case "datetime":
                                    return System.DateTime.Parse(group.Value);
                                case "double":
                                    return System.Double.Parse(group.Value);
                                case "integer":
                                    return System.Int32.Parse(group.Value);
                                case "string":
                                     return group.Value;
                                default:
                                    return value;
                            }
                        }
                    }
                    return value;
                }
            }
            return value;
        }      
    }
}
