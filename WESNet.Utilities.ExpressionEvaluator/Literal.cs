using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace WESNet.Utilities.ExpressionEvaluator
{ 

    public class Literal : Operand, IComparable<Literal>
    {   

        public Literal(string tokenText, int characterPosition) : base(tokenText, characterPosition)
        {
            //This constructor is called when derived class Identifier or DNNToken is consturcted.
        }

        public Literal(string tokenText, string typeName, int characterPosition) : base(tokenText, characterPosition)
        {
            TokenType = Enums.TokenTypes.Literal;

            switch (typeName)
            {
                case "System.Int32":
                    Value = Int32.Parse(tokenText);
                    break;
                case "System.String":
                    Value = tokenText;
                    break;
                case "System.Double":
                    Value = Double.Parse(tokenText);
                    break;
                case "System.Boolean":
                    Value = Boolean.Parse(tokenText);
                    break;
                case "System.DateTime":
                    Value = DateTime.Parse(tokenText);
                    break;
                case "":
                    Value = null;
                    break;
            }
        }

        public Literal(dynamic value)
        {
            TokenType = Enums.TokenTypes.Literal;
            TokenText = (value == null ? "NULL" : value.ToString());
            Value = value;
        }

        public Literal Add (Literal op2)
        {
            return new Literal (Add(this, op2));
        }

        public Literal Subtract (Literal op2)
        {
            return new Literal (Subtract(this, op2));
        }
        
        public Literal Multiply (Literal op2)
        {
            return new Literal (Multiply(this, op2));
        }

        public Literal Divide (Literal op2)
        {
            return new Literal(Divide(this, op2));
        }

        public Literal Mod (Literal op2)
        {
            return new Literal(Mod(this, op2));
        }

        public Literal Minus ()
        {
            return new Literal (Minus(this));
        }

        public Literal Plus ()
        {
            return new Literal (this.TokenText);
        }

        public Literal Not()
        {
            return new Literal (Not(this));
        }

        public Literal Or (Literal op2)
        {
            return new Literal (Or(this, op2));
        }

        public Literal And (Literal op2)
        {
            return new Literal (And(this, op2));
        }
                
        public override string ToString()
        {
            return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                   "Token Text: ".PadLeft(Constants.CaptionWidth) + (TokenText == null ? "NULL" : TokenText) + "\r\n" +
                   "Character Position: ".PadLeft(Constants.CaptionWidth) + (CharacterPosition == -1 ? "NA" : CharacterPosition.ToString()) + "\r\n" +
                   "Value Type: ".PadLeft(Constants.CaptionWidth) + OperandTypeName + "\r\n" +
                   "Value: ".PadLeft(Constants.CaptionWidth) + (Value == null ? "NULL" : Value.ToString());
        }
        
        public Literal Equal (Literal op2)
        {
            return new Literal(CompareTo(op2) == 0);
        }

        public Literal NotEqual (Literal op2)
        {
            return new Literal (CompareTo(op2) != 0);
        }

        public Literal GreaterThan (Literal op2)
        {
            return new Literal(CompareTo(op2) > 0);
        }

        public Literal LessThan(Literal op2)
        {
            return new Literal (CompareTo(op2) < 0);
        }

        public Literal GreaterThanOrEqual(Literal op2)
        {
            return new Literal (CompareTo(op2) >= 0);
        }

        public Literal LessThanOrEqual(Literal op2)
        {
            return new Literal (CompareTo(op2) <= 0);
        }

        public static dynamic Add(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.TokenText == null || op2.TokenText == null) throw new ArgumentNullException("Cannot Add a null operand or null operand value");
            
            if (!AreCompatible(op1, op2)) throw new EvaluatorException("Cannot add incompatible operand types");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x+y;
        }

        public static dynamic Subtract(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot Subtract a null operand or null operand value");

            if (!AreCompatible(op1, op2)) throw new EvaluatorException("Cannot subtract incompatible operand types");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x - y;
        }

        public static dynamic Multiply(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot Multiply a null operand or null operand value");

            if (!AreCompatible(op1, op2)) throw new EvaluatorException("Cannot multiply incompatible operand types");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x * y;
        }

        public static dynamic Divide(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot Divide a null operand or null operand value");

            if (!AreCompatible(op1, op2)) throw new EvaluatorException("Cannot divide incompatible operand types");

            dynamic x = op1.Value;
            dynamic y = op2.Value;

            if (y == 0) throw new EvaluatorException("Cannot divide by 0");

            return x / y;
        }

        public static dynamic Mod(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot Mod a null operand or null operand value");

            if (!AreCompatible(op1, op2)) throw new EvaluatorException("Cannot calculate modulus of incompatible operand types");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x % y;
        }

        public static dynamic Minus(Literal op1)
        {
            if (op1 == null || op1.Value == null) throw new ArgumentNullException("Cannot negate a null operand or null operand value");

            string resultTypeName = op1.OperandType.FullName;

            if (resultTypeName == "System.Int32" || resultTypeName == "System.Double")
            {
                dynamic x = op1.Value;
                return -x;
            }

            throw new ArgumentException("Cannot negate an incompatible operand value type");
        }

        public static bool Not(Literal op1)
        {
            if (op1 == null || op1.Value == null) throw new ArgumentNullException("Cannot not a null operand or null operand value");

            string resultTypeName = op1.OperandType.FullName;

            if (resultTypeName == "System.Boolean")
            {
                dynamic x = op1.Value;
                return !x;
            }

            throw new ArgumentException("Cannot not a non-boolean operand type");
        }

        public static bool Or(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot Or a null operand or null operand value");

            if (op1.GetType().FullName != "System.Boolean" || op2.GetType().FullName != "System.Boolean") throw new EvaluatorException("Cannot or a non-boolean operand type");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x || y;
        }

        public static bool And(Literal op1, Literal op2)
        {
            if (op1 == null || op2 == null || op1.Value == null || op2.Value == null) throw new ArgumentNullException("Cannot and a null operand or null operand value");

            if (op1.GetType().FullName != "System.Boolean" || op2.GetType().FullName != "System.Boolean") throw new EvaluatorException("Cannot and a non-boolean operand type");

            dynamic x = op1.Value;
            dynamic y = op2.Value;
            return x && y;
        }


        public int CompareTo(Literal op2)
        {
            if (op2 == null) throw new ArgumentNullException("op2", "Operand 2 cannot be null");

            if (this.TokenText == null && op2.TokenText == null) return 0;

            if (this.TokenText == null || op2.TokenText == null) throw new ArgumentException("Cannot compare two operands when one has a null value");

            if (!AreCompatible(this, op2)) throw new EvaluatorException("Cannot compare incompatible operand types");

            return Value.CompareTo(op2.Value);
        }
    }
}
