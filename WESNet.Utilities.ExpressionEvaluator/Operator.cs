using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{

    public class Operator : Token
    {
        public static Operator Add() { return new Operator("+", "Add", 3); }
        public static Operator Subtract() { return new Operator("-", "Subtract", 3); }
        public static Operator Multiply() { return new Operator("*", "Multiply", 4); }
        public static Operator Divide() { return new Operator("/", "Divide", 4); }
        public static Operator Mod() { return new Operator("%", "Mod", 4); }
        public static Operator Equal() { return new Operator("==", "Equal", 1); }
        public static Operator NotEqual() { return new Operator("!=", "NotEqual", 1); }
        public static Operator LessThan() { return new Operator("<", "LessThan", 2); }
        public static Operator LessThanOrEqual() { return new Operator("<=", "LessThanOrEqual", 2); }
        public static Operator GreaterThan() { return new Operator(">", "GreaterThan", 3); }
        public static Operator GreaterThanOrEqual() { return new Operator(">=", "GreaterThanOrEqual", 2); }
        public static Operator Or() { return new Operator("||", "Or", 0); }
        public static Operator And() { return new Operator("&&", "And", 0); }
        public static Operator UnaryMinus() { return new Operator("-", "Minus", 5); }
        public static Operator UnaryPlus() { return new Operator("+", "Plus", 5); }
        public static Operator Not() { return new Operator("!", "Not", 5); }
        public static Operator LeftParen() { return new Operator("(", "LeftParen", -1); }
        public static Operator RightParen() { return new Operator(")", "RightParen", 6); }
        public static Operator Comma() { return new Operator(",", "Comma", 6); }
        public static Operator Assignment() { return new Operator("=", "Assignment", -2); }
        //public static Operator FunctionCall(string functionName, int characterPosition) { return new Operator("Fx", functionName, -1, characterPosition); }
        //public static Operator MethodCall(string methodName, int characterPosition) { return new Operator("Mx", methodName, -1, characterPosition); }
        public static Operator PropertyCall(string propertyName, int characterPosition) { return new Operator("Px", propertyName, -1, characterPosition); }
        public static string[] UnaryOperators = new[] { "Minus", "Plus", "Not" };

        public string Symbol
        {
            get
            {
                return TokenText ?? "" ;
            }
            set
            {
                TokenText = value;
            }
        }

        public string OperatorName { get; set; }

        public int Priority { get; set; }

        public bool IsUnaryOperator
        {
            get
            {
                return UnaryOperators.Contains(OperatorName);
            }
        }

        public bool IsPropertyCall
        {
            get
            {
                return Symbol == "Px";
            }
        }

        public Operator() : base()
        {
            TokenType = Enums.TokenTypes.Operator;
        }

        public Operator(string tokenText, int characterPosition) : this(tokenText,"",0,-1) {}

        public Operator(string tokenText, string operatorName, int priority) : this(tokenText, operatorName, priority, -1) {}

        public Operator (string tokenText, string operatorName, int priority, int characterPosition) : base (tokenText,characterPosition)
        {
            TokenType = Enums.TokenTypes.Operator;
            OperatorName = operatorName;
            Priority = priority;
        }

        public override string ToString()
        {
            return base.ToString() + "\r\n" +
                   "Symbol: ".PadLeft(Constants.CaptionWidth) + Symbol + "\r\n" +
                   "OperatorName: ".PadLeft(Constants.CaptionWidth) + OperatorName + "\r\n" +
                   "Priority: ".PadLeft(Constants.CaptionWidth) + Priority.ToString() + "\r\n" +
                   "Is Unary Operator ".PadLeft(Constants.CaptionWidth) + IsUnaryOperator.ToString() + "\r\n" +
                   "Is Property Call ".PadLeft(Constants.CaptionWidth) + IsPropertyCall.ToString() + "\r\n";
        }
    }

}
