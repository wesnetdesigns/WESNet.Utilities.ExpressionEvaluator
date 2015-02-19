using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Variable : Literal
    {
        public event EventHandler<ProcessIdentifierHandlerArgs> ProcessIdentifier;

        public override Enums.TokenTypes TokenType { get { return Enums.TokenTypes.Variable; } }

        public string Namespace { get; set; }

        public string IdentifierName { get; set; }

        public bool IsDeclared { get; set; }

        public bool IsLoaded { get; set; }

        public override object Value
        {
            get
            {
                if (!IsLoaded)
                {
                    try
                    {
                        base.Value = LoadValue();
                        IsLoaded = true;
                    }
                    catch
                    {
                        base.Value = null;
                        IsLoaded = false;
                    }        
                }
                return base.Value;
            }
            set
            {
                StoreValue(value);
                base.Value = value;
            }
        }

        public Variable(string tokenText, int characterPosition) : base(tokenText, characterPosition)
        {
            IdentifierName = tokenText;
        }

        public override string ToString()
        {
            return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                   "Token Text: ".PadLeft(Constants.CaptionWidth) + (TokenText == null ? "NULL" : TokenText) + "\r\n" +
                   "Namespace: ".PadLeft(Constants.CaptionWidth) + (Namespace == null ? "" : Namespace) + "\r\n" +
                   "Identifier Name: ".PadLeft(Constants.CaptionWidth) + IdentifierName + "\r\n" +
                   "Is Declared: ".PadLeft(Constants.CaptionWidth) + IsDeclared.ToString() + "\r\n" +
                   "Character Position: ".PadLeft(Constants.CaptionWidth) + (CharacterPosition == -1 ? "NA" : CharacterPosition.ToString()) + "\r\n" +
                   "Value Type: ".PadLeft(Constants.CaptionWidth) + ValueTypeName + "\r\n" +
                   "Value Type Index: ".PadLeft(Constants.CaptionWidth) + ValueTypeIndex.ToString() + "\r\n" +
                   "Value: ".PadLeft(Constants.CaptionWidth) + (Value == null ? "NULL" : Value.ToString());
        }

        public virtual dynamic LoadValue()
        {
            if (ProcessIdentifier != null)
            {
                var eventArgs = new ProcessIdentifierHandlerArgs() { ProcessAction = Enums.ProcessActions.Load, FullName = IdentifierName, VariableType = ValueType };
                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error == null)
                {
                    return eventArgs.Value;
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

        public virtual void StoreValue(dynamic value)
        {
            if (ProcessIdentifier != null)
            {            
                var eventArgs = new ProcessIdentifierHandlerArgs() { ProcessAction = Enums.ProcessActions.Store, FullName = IdentifierName, VariableType = ValueType, Value = value };
                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error != null)
                {
                    throw new EvaluatorException(eventArgs.Error);
                }
            }
        }

        public void DeclareVariable()
        {
            if (ProcessIdentifier != null)
            {
                var eventArgs = new ProcessIdentifierHandlerArgs() { ProcessAction = Enums.ProcessActions.Register, FullName = IdentifierName, VariableType = ValueType };
                ProcessIdentifier(this, eventArgs);
                if (eventArgs.Error != null)
                {
                    throw new EvaluatorException(eventArgs.Error);
                }
            }
        }
    }
}
