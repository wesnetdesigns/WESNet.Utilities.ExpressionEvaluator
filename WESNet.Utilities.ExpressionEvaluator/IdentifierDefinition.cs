using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class IdentifierDefinition
    {
        private Enums.IdentifierDefinitionTypes _IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.IdentifierDefinition;

        private dynamic _Value;

        public Enums.IdentifierDefinitionTypes IdentifierDefinitionType
        {
            get
            {
                return _IdentifierDefinitionType;
            }
            set
            {
                _IdentifierDefinitionType = value;
            }
        }

        public string Namespace { get; set; }

        public string Class { get; set; }

        public string Name { get; set; }
        
        public string TypeName
        {
            get
            {
                return (string.IsNullOrEmpty(Namespace) ? "" : Namespace + ".") +
                    (string.IsNullOrEmpty(Class) ? "" : Class + ".");
            }
        }

        public virtual string FullyQualifiedName
        {
            get
            {
                return TypeName + Name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Namespace = null;
                    Class = null;
                    Name = "";
                }
                else
                {
                    var lastDot = value.LastIndexOf('.') + 1;
                    Name = value.Substring(lastDot);
                    var fullyQualifiedClass = value.Substring(0, lastDot).TrimEnd('.');
                    lastDot = fullyQualifiedClass.LastIndexOf('.') + 1;
                    Class = fullyQualifiedClass.Substring(lastDot);
                    Namespace = fullyQualifiedClass.Substring(0, lastDot).TrimEnd('.');
                }
            }
        }

        public virtual Type ValueType
        {
            get
            {
                if (IdentifierDefinitionType == Enums.IdentifierDefinitionTypes.Class)
                {
                    return Type.GetType(FullyQualifiedName);
                }
                else
                {
                    return Value == null ? null : Value.GetType();
                }
            }
        }

        public bool HasValue { get; set; }

        public dynamic Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != null)
                {
                    var valueType = value.GetType();

                    HasValue = true;
                }
                else
                {
                    HasValue = false;
                }
                
                _Value = value;
            }
        }

        public IdentifierDefinition(string fullName, Enums.IdentifierDefinitionTypes identifierDefinitionType)
        {
            FullyQualifiedName = fullName;
            IdentifierDefinitionType = identifierDefinitionType;
        }

        public IdentifierDefinition(string @namespace, string @class, string name, Enums.IdentifierDefinitionTypes identifierDefinitionType)
        {
            Namespace = @namespace;
            Class = @class;
            Name = name;
            IdentifierDefinitionType = identifierDefinitionType;
        }

        public IdentifierDefinition(string @namespace, string @class, string name, Enums.IdentifierDefinitionTypes identifierDefinitionType, dynamic value)
            : this(@namespace, @class, name, identifierDefinitionType)
        {
            Value = value;
        }

        public IdentifierDefinition(Identifier identifier)
        {
            Namespace = identifier.Namespace;
            Class = identifier.Class;
            Name = identifier.Name;
            if (!IsSupportedType(identifier.OperandType.FullName))
            {
                throw new EvaluatorException("Type " + identifier.OperandType.FullName + " is not supported.");
            }
            FullyQualifiedName = identifier.FullyQualifiedName;
            switch (identifier.TokenType)
            {
                case Enums.TokenTypes.Identifier:
                    IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.IdentifierDefinition;
                    break;
                case Enums.TokenTypes.Class:
                    IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.Class;
                    break;
                case Enums.TokenTypes.Variable:
                    IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.Variable;
                    break;
                case Enums.TokenTypes.DNNToken:
                    IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.DNNToken;
                    break;
                case Enums.TokenTypes.Operator:
                    IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.Method;
                    break;
            }
        }

        private static bool IsSupportedType(string typeName)
        {
            return (Array.IndexOf<string>(Constants.SupportedTypes, typeName) == -1);
        }

        public override string ToString()
        {
            return "Identifier Definition Type: ".PadLeft(Constants.CaptionWidth) + IdentifierDefinitionType.ToString() + "\r\n" +
                   "Namespace: ".PadLeft(Constants.CaptionWidth) + (Namespace ?? "") + "\r\n" +
                   "Class: ".PadLeft(Constants.CaptionWidth) + (Class ?? "") + "\r\n" +
                   "Name: ".PadLeft(Constants.CaptionWidth) + (Name ?? "") + "\r\n" +
                   "Full Name: ".PadLeft(Constants.CaptionWidth) + FullyQualifiedName + "\r\n"  + 
                   "Has Value: ".PadLeft(Constants.CaptionWidth) + HasValue.ToString() + "\r\n" +
                   "Value: ".PadLeft(Constants.CaptionWidth) + (HasValue ? Value.ToString() : "NULL") + "\r\n" +
                   "Value Type: ".PadLeft(Constants.CaptionWidth) + (ValueType == null ? "NULL" : ValueType.FullName);
        }

    }
}
