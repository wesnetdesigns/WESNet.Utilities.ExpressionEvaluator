using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Namespace : Token
    {
        private List<string> _Namespaces = new List<string>();
        
        public override Enums.TokenTypes TokenType { get { return Enums.TokenTypes.Namespace; } }

        public List<string> Namespaces
        {
            get
            {
                return _Namespaces;
            }
        }

        public string CompleteNamespace
        {
            get
            {
                return String.Join(".", _Namespaces);
            }
        }

        public Namespace(string tokenText, int characterPosition)
            : base(tokenText, characterPosition)
        {
            var namespaces = tokenText.Split('.');
            _Namespaces.AddRange(namespaces);
        }

        public override string ToString()
        {
            return "Token Type: ".PadLeft(Constants.CaptionWidth) + TokenType.ToString() + "\r\n" +
                   "Token Text: ".PadLeft(Constants.CaptionWidth) + (TokenText == null ? "NULL" : TokenText) + "\r\n" +
                   "Character Position: ".PadLeft(Constants.CaptionWidth) + (CharacterPosition == -1 ? "NA" : CharacterPosition.ToString()) + "\r\n" +
                   "Namespace: ".PadLeft(Constants.CaptionWidth) + (_Namespaces.Count == 0 ? "" : CompleteNamespace);
        }

        public bool IsRegistered
        {
            get
            {
                throw new NotImplementedException("Namespace Registration is not implemented");
            }
        }

        public void RegisterNamespace()
        {
            throw new NotImplementedException("Namespace Registration is not implemented");
        }
        
    }
}
