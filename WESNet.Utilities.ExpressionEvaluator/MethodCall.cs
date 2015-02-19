using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    class MethodCall : Operator
    {
        public event EventHandler<ProcessMethodHandlerArgs> ProcessMethod;

        public bool IsRegistered { get; set; }

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

        public Type ReturnType
        {
            get
            {
                var type = Type.GetType(TypeName);
                return type;
            }
        }

        public dynamic ReturnValue { get; set; }

        public bool IsStatic { get; set; } 
        
        public MethodCall() : base()
        {
            TokenType = Enums.TokenTypes.MethodCall;
            IsStatic = true;
        }

        public MethodCall(string tokenText, int characterPosition, bool isStatic) : base(tokenText, characterPosition)
        {
            TokenType = Enums.TokenTypes.MethodCall;
            var lastDot = tokenText.LastIndexOf('.') + 1;
            Name = tokenText.Substring(lastDot);
            var fullyQualifiedClass = tokenText.Substring(0, lastDot).TrimEnd('.');
            TypeName = fullyQualifiedClass;
            IsStatic = isStatic;
        }

        public override string ToString()
        {
            return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                   "Namespace: ".PadLeft(Constants.CaptionWidth) + (Namespace ?? "") + "\r\n" +
                   "Class: ".PadLeft(Constants.CaptionWidth) + (Class ?? "") + "\r\n" +
                   "Method Name: ".PadLeft(Constants.CaptionWidth) + (Name ?? "") + "\r\n" +
                   "Full Name: ".PadLeft(Constants.CaptionWidth) + FullyQualifiedName + "\r\n" +
                   "Is Static: ".PadLeft(Constants.CaptionWidth) + IsStatic.ToString() + "\r\n" +
                   "Is Registered: ".PadLeft(Constants.CaptionWidth) + IsRegistered.ToString() + "\r\n" +
                   "Return Type: ".PadLeft(Constants.CaptionWidth) + (ReturnType == null ? "" : ReturnType.FullName);
        }

        public dynamic InvokeMethod(Stack<Operand> operandStack)
        {
            if (ProcessMethod != null)
            {
                ProcessMethodHandlerArgs eventArgs;
                var parameters = new List<dynamic>();
                Identifier identifier = null;
                dynamic instance = null;

                while (operandStack.Count > 0 && operandStack.Peek().TokenType != Enums.TokenTypes.MethodFrame )
                {
                    var operand = (Literal)(operandStack.Pop());
                    parameters.Add(operand.Value);
                }

                if (operandStack.Pop().TokenType != Enums.TokenTypes.MethodFrame)
                {
                    throw new EvaluatorException("Operand Stack Method Frame Not Found");
                }

                parameters.Reverse();

                if (operandStack.Count > 0)
                {
                    var instanceOperand = operandStack.Pop();

                    if (instanceOperand.TokenType == Enums.TokenTypes.Class)
                    {
                        identifier = (Identifier)instanceOperand;
                        TypeName = identifier.FullyQualifiedName;
                        instance = null;
                    }
                    else if (!IsStatic)
                    {
                        switch (instanceOperand.TokenType)
                        {
                            case Enums.TokenTypes.Literal:
                                instance = ((Literal)instanceOperand).Value;
                                break;
                            case Enums.TokenTypes.Variable:
                                identifier = (Identifier)instanceOperand;
                                instance = identifier.LoadValue();
                                break;
                            case Enums.TokenTypes.DNNToken:
                                var dnnToken = (DNNToken)instanceOperand;
                                instance = dnnToken.Value;
                                break;
                        }
                        TypeName = instance == null ? "" : instance.GetType().FullName;
                    }
                }

                var parameterArray = parameters.ToArray();
                
                eventArgs = new ProcessMethodHandlerArgs()
                {
                    ProcessAction = Enums.ProcessActions.ResolveMethodSignature,
                    FullyQualifiedName = this.FullyQualifiedName,
                    IsStatic = this.IsStatic,
                    Instance = instance,
                    Parameters = parameterArray
                };
                ProcessMethod(this, eventArgs);

                if (eventArgs.Error == null)
                {
                    eventArgs.ProcessAction = Enums.ProcessActions.Invoke;
                    ProcessMethod(this, eventArgs);
                    if (eventArgs.Error == null)
                    {
                        return eventArgs.ReturnValue;
                    }
                }
                throw new EvaluatorException("Error invoking method " + eventArgs.FullyQualifiedName + ": " + eventArgs.Error.ErrorMessage);
            }
            return null;
        }
    }
}
