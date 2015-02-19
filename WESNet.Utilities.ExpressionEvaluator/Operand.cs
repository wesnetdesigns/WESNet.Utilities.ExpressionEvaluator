using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Operand : Token
    {
        private dynamic _Value = null;

        public virtual dynamic Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (IsSupportedType(value))
                {
                    _Value = value;
                }
                else
                {
                    throw new EvaluatorException(string.Format("Unable to set Value property because the supplied value ((0}) is not a supported type.", value));
                }
            }
        }

        public virtual bool HasValue
        {
            get
            {
                return Value != null;
            }
        }

        public virtual Type OperandType
        {
            get
            {
                return Value == null ? null : Value.GetType();
            }
        }

        public string OperandTypeName
        {
            get
            {
                return OperandType == null ? "NULL" : OperandType.FullName;
            }
        }

        public Operand() : base()
        {
            TokenType = Enums.TokenTypes.Operand;
        }

        public Operand(string tokenText, int characterPosition)
            : base(tokenText, characterPosition)
        {
            TokenType = Enums.TokenTypes.Operand;
            _Value = null;
        }

        public static bool IsSupportedType(dynamic value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                var valueTypeName = value.GetType().FullName;
                return Evaluator.TypeRegistry.ContainsKey(valueTypeName);
            }
        }

        public static bool AreCompatible(Operand op1, Operand op2)
        {
            var op1TypeName = op1.OperandTypeName;
            var op2TypeName = op2.OperandTypeName;
            return (op1TypeName != "NULL" && op2TypeName != "NULL")
                && (op1TypeName == op2TypeName) || (Evaluator.TypeRegistry[op1TypeName].ConvertsTo.ContainsKey(op2TypeName)
                     && Evaluator.TypeRegistry[op2TypeName].ConvertsTo.ContainsKey(op1TypeName));
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
