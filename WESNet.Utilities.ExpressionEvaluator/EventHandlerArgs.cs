using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class ProcessIdentifierHandlerArgs : System.EventArgs
    {
        public Enums.ProcessActions ProcessAction { get; set; }
        public Enums.IdentifierDefinitionTypes IdentifierDefinitionType { get; set; }
        public bool IsRegistered { get; set; }
        public string FullyQualifiedName { get; set; }
        public dynamic Value { get; set; }
        public Type OperandType { get; set; }
        public EvaluatorError Error { get; set; }
    }

    public class ProcessDnnTokenHandlerArgs : System.EventArgs
    {
        public Enums.ProcessActions ProcessAction { get; set; }
        public string ObjectName { get; set; }
        public string PropertyName { get; set; }
        public string Format { get; set; }
        public string ReturnValue { get; set; }
        public EvaluatorError Error { get; set; }
    }

    public class ProcessMethodHandlerArgs : System.EventArgs
    {
        public Enums.ProcessActions ProcessAction { get; set; }
        public string FullyQualifiedName { get; set; }
        public bool IsStatic { get; set; }
        public string MethodName { get; set; }
        public dynamic Instance { get; set; }
        public dynamic[] Parameters { get; set; }
        public MethodDefinition MethodDefinition { get; set; }
        public dynamic ReturnValue { get; set; }
        public Type ReturnType { get; set; }
        public EvaluatorError Error { get; set; }
    }

}
