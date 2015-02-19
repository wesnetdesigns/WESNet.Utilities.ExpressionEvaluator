using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Enums
    {
        public enum TokenTypes
        {
            Token = 0,
            Operator,
            MethodCall,
            MethodFrame,
            Operand,
            Literal,
            Identifier,
            Class,
            Variable,
            DNNToken
        }

        public enum IdentifierDefinitionTypes
        {
            IdentifierDefinition = 0,
            Class,
            Variable,
            DNNToken,
            Method
        }

        public enum ProcessActions
        {
            Register = 0,
            ResolveType,
            ResolveMethodSignature,
            Store,
            Load,
            Set,
            Get,
            Invoke
        }

    }
}
