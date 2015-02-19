using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class EvaluatorException : Exception
    {
        public EvaluatorError EvaluatorError { get; set; }

        public EvaluatorException(string message) : base(message)
        {
            EvaluatorError = null;
        }

        public EvaluatorException(EvaluatorError evaluatorError)
            : base(evaluatorError.ErrorMessage)
        {
            EvaluatorError = evaluatorError;
        }

        public EvaluatorException(string message, EvaluatorError evaluatorError, Exception innerException)
            : base(message, innerException)
        {
            EvaluatorError = evaluatorError;
        }

    }
}
