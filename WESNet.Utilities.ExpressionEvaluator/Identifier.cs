using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Identifier : Operand
    {
        public event EventHandler<ProcessIdentifierHandlerArgs> ProcessIdentifier;

        public bool IsRegistered { get; set; }

        public bool IsResolved
        {
            get
            {
                return (TokenType != Enums.TokenTypes.Identifier);
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
                    (string.IsNullOrEmpty(Class) ? "" : Class);
            }
            set
            {
                var lastDot = value.LastIndexOf('.') + 1;
                Class = value.Substring(lastDot);
                Namespace = value.Substring(0, lastDot).TrimEnd('.');
            }
        }

        public string FullyQualifiedName
        {
            get
            {
                return TypeName == "" ? Name : TypeName + "." + Name;
            }
        }

        public override Type OperandType
        {
            get
            {
                if (TokenType == Enums.TokenTypes.Class)
                {
                    return Type.GetType(FullyQualifiedName);
                }
                else
                {
                    return base.OperandType;
                }
            }
        }

        public Identifier() : base()
        {
            TokenType = Enums.TokenTypes.Identifier;
        }

        public Identifier(string tokenText, int characterPosition)
            : base(tokenText, characterPosition)
        {
            TokenType = Enums.TokenTypes.Identifier;
            var lastDot = tokenText.LastIndexOf('.') + 1;
            Name = tokenText.Substring(lastDot);
            var fullyQualifiedClass = tokenText.Substring(0, lastDot).TrimEnd('.');
            TypeName = fullyQualifiedClass;
        }

        public override string ToString()
        {
            return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                   "Namespace: ".PadLeft(Constants.CaptionWidth) + (Namespace ?? "") + "\r\n" +
                   "Class: ".PadLeft(Constants.CaptionWidth) + (Class ?? "") + "\r\n" +
                   "Name: ".PadLeft(Constants.CaptionWidth) + (Name ?? "") + "\r\n" +
                   "Full Name: ".PadLeft(Constants.CaptionWidth) + FullyQualifiedName + "\r\n" +
                   "Is Registered: ".PadLeft(Constants.CaptionWidth) + IsRegistered.ToString() + "\r\n" +
                   "Is Resolved: ".PadLeft(Constants.CaptionWidth) + IsResolved.ToString() + "\r\n" +
                   "Identifier Type: ".PadLeft(Constants.CaptionWidth) + (OperandType == null ? "" : OperandType.FullName) + "\r\n" +
                   "Has Value: ".PadLeft(Constants.CaptionWidth) + HasValue.ToString() + "\r\n" +
                   "Value: ".PadLeft(Constants.CaptionWidth) + (HasValue ? Value.ToString() : "NULL");

        }

        public void ResolveTokenType()
        {
            if (ProcessIdentifier != null)
            {
                var eventArgs = new ProcessIdentifierHandlerArgs()
                {
                    ProcessAction = Enums.ProcessActions.ResolveType,
                    FullyQualifiedName = this.FullyQualifiedName
                };
                if (ProcessIdentifier != null)
                {
                    ProcessIdentifier(this, eventArgs);
                    if (eventArgs.Error == null)
                    {
                        var identifierDefinitionType = eventArgs.IdentifierDefinitionType;
                        switch (identifierDefinitionType)
                        {
                            case Enums.IdentifierDefinitionTypes.Variable:
                                TokenType = Enums.TokenTypes.Variable;
                                break;
                            case Enums.IdentifierDefinitionTypes.Class:
                                TokenType = Enums.TokenTypes.Class;
                                break;
                            case Enums.IdentifierDefinitionTypes.DNNToken:
                                TokenType = Enums.TokenTypes.DNNToken;
                                break;
                            case Enums.IdentifierDefinitionTypes.Method:
                                TokenType = Enums.TokenTypes.MethodCall;
                                break;
                        }
                        IsRegistered = eventArgs.IsRegistered;                 
                    }
                    else
                    {
                        throw new EvaluatorException(eventArgs.Error);
                    }
                }
            }
        }

        public virtual dynamic LoadValue()
        {
            if (ProcessIdentifier != null)
            {
                var eventArgs = new ProcessIdentifierHandlerArgs()
                {
                    ProcessAction = Enums.ProcessActions.Load,
                    FullyQualifiedName = this.FullyQualifiedName
                };

                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error == null)
                {
                    Value = eventArgs.Value;
                    return Value;
                }
                else
                {
                    throw new EvaluatorException(eventArgs.Error);
                }
            }
            else
            {
                return null;
            }
        }

        public virtual void StoreValue()
        {
            if (ProcessIdentifier != null)
            {
                var eventArgs = new ProcessIdentifierHandlerArgs()
                {
                  ProcessAction = Enums.ProcessActions.Store,
                  FullyQualifiedName = this.FullyQualifiedName,
                  Value = this.Value
                };
                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error != null)
                {
                    throw new EvaluatorException(eventArgs.Error);
                }
            }
        }

        public void RegisterIdentifier()
        {
            if (ProcessIdentifier != null)
            {
                var identifierDefinitionType = Enums.IdentifierDefinitionTypes.IdentifierDefinition;
                switch (TokenType)
                {
                    case Enums.TokenTypes.Variable:
                        identifierDefinitionType = Enums.IdentifierDefinitionTypes.Variable;
                        break;
                    case Enums.TokenTypes.Class:
                        identifierDefinitionType = Enums.IdentifierDefinitionTypes.Class;
                        break;
                    case Enums.TokenTypes.DNNToken:
                        identifierDefinitionType = Enums.IdentifierDefinitionTypes.DNNToken;
                        break;
                    case Enums.TokenTypes.Operator:
                        identifierDefinitionType = Enums.IdentifierDefinitionTypes.Method;
                        break;
                }
                var eventArgs = new ProcessIdentifierHandlerArgs()
                {
                    ProcessAction = Enums.ProcessActions.Register,
                    FullyQualifiedName = this.FullyQualifiedName,
                    IdentifierDefinitionType = identifierDefinitionType
                };
                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error != null)
                {
                    throw new EvaluatorException(eventArgs.Error);
                }
            }
        }
    }
}

