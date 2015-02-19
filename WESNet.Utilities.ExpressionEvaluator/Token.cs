using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
        public class Token
        {
            private Enums.TokenTypes _TokenType = Enums.TokenTypes.Token;

            public Enums.TokenTypes TokenType
            {
                get
                {
                    return _TokenType;
                }
                set
                {
                    _TokenType = value;
                }
            }

            public string TokenText { get; set; }

            public int CharacterPosition { get; set; }

            public Token()
            {
                CharacterPosition = -1;
            }

            public Token(string tokenText) : this()
            {
                TokenText = tokenText;
            }

            public Token(string tokenText, int characterPosition) : this(tokenText)
            {
                CharacterPosition = characterPosition;
            }           

            public override string ToString()
            {
                return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                       "Token Text: ".PadLeft(Constants.CaptionWidth) + (TokenText == null ? "NULL" : TokenText) + "\r\n" +
                       "Character Position: ".PadLeft(Constants.CaptionWidth) + (CharacterPosition == -1 ? "NA" : CharacterPosition.ToString());
            }
        }
}
