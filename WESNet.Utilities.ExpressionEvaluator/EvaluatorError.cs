using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class EvaluatorError
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int CharacterPosition { get; set; }
        public int LastTokenIndex { get; set; }
        public Token LastToken { get; set; }

        public EvaluatorError()
        {
            CharacterPosition = -1;
            LastTokenIndex = -1;
        }

        public EvaluatorError(int errorCode, string errorMessage) : this()
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public EvaluatorError(int errorCode, string errorMessage, int characterPosition, int lastTokenPosition, Token lastToken)
            : this(errorCode, errorMessage)
        {
            CharacterPosition = characterPosition;
            LastTokenIndex = lastTokenPosition;
            LastToken = lastToken;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Error Code: {0}\r\n", ErrorCode);
            sb.AppendFormat("Error Message: {0}\r\n", ErrorMessage);
            if (CharacterPosition != -1) sb.AppendFormat("Character Position: {0}\r\n", CharacterPosition);
            if (LastTokenIndex != -1) sb.AppendFormat("Last Token Index: {0}\r\n", LastTokenIndex);
            if (LastToken != null) sb.AppendFormat("Last Token: {0}\r\n", LastToken.ToString());
            return sb.ToString();
        }
    }
}
