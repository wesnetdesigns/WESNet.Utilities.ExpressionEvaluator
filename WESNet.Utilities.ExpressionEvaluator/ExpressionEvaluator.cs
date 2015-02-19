using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace WESNet.Utilities.ExpressionEvaluator
{
    public class Evaluator
    {
        private static string tokenizerPattern = @"(?<whitespace>\s+)|" +
                                                 @"(?<bool>true|false)|" +
                                                 @"#(?<datetime>.*?)#|" +
                                                 @"(?<double>\d+\.\d+|\d+(?:[dD]))|" +
                                                 @"(?<integer>\d+)|" +
                                                 @"(?:"")(?<string>.*?)(?:"")|" +
                                                 @"(?<op>==|!=|<=|>=|\|\||&&|[+\-*/!><%(),=])|" +
                                                 @"((?:[.])(?<method>[a-zA-Z]\w*)\s*\()|" +
                                                 @"(?<function>[a-zA-Z]\w*)\s*\(|" +
                                                 @"(?<identifier>([a-zA-Z]\w*(\.[a-zA-Z]\w*)*(?=\.[a-zA-Z]\w*))|([a-zA-Z]\w*))|" +                                             
                                                 @"((?:[.])(?<property>[a-zA-Z]\w*)\b)|" +
                                                 @"(?:\[)(?<dnntoken>[^\[\]]+?)(?:\])|" +
                                                 @"(?<invalid>[^\s]+)";

        private static Regex tokenizer = new Regex(tokenizerPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Dictionary<string, RegisteredType> _TypeRegistry = new Dictionary<string, RegisteredType>();
        private Dictionary<string, IdentifierDefinition> _IdentifierRegistry = new Dictionary<string, IdentifierDefinition>();
        private Dictionary<string, List<MethodDefinition>> _MethodRegistry = new Dictionary<string, List<MethodDefinition>>();
        private Queue<Token> _RPNTokens = new Queue<Token>();
        private Stack<Operand> _OperandStack = new Stack<Operand>();
        private Stack<Operator> _OperatorStack = new Stack<Operator>();
        private List<EvaluatorError> _EvaluatorErrors = new List<EvaluatorError>();

        private bool _unaryState;
        private bool _fxParamsState;
        private bool _isAssignmentStatement;

        public EventHandler UpdateProgress;

        public Queue<Token> RPNTokens
        {
            get
            {
                return _RPNTokens;
            }
        }

        public Stack<Operand> OperandStack
        {
            get
            {
                return _OperandStack;
            }
        }

        public Stack<Operator> OperatorStack
        {
            get
            {
                return _OperatorStack;
            }
        }

        public static Dictionary<string, RegisteredType> TypeRegistry
        {
            get
            {
                return _TypeRegistry;
            }
        }

        public Dictionary<string, IdentifierDefinition> IdentifierRegistry
        {
            get
            {
                return _IdentifierRegistry;
            }
        }

        public Dictionary<string, List<MethodDefinition>> MethodRegistry
        {
            get
            {
                return _MethodRegistry;
            }
        }

        public List<EvaluatorError> EvaluatorErrors
        {
            get
            {
                return _EvaluatorErrors;
            }
        }

        public event EventHandler<ProcessDnnTokenHandlerArgs> ProcessDNNToken;

        static Evaluator()
        {
            var convertsToFilter = new string[] { "System.Boolean", "System.DateTime", "System.Double", "System.Int32", "System.Boolean" };
            TypeRegistry.Add("System.Boolean", new RegisteredType("System.Boolean", convertsToFilter));
            TypeRegistry.Add("System.DateTime", new RegisteredType("System.DateTime", convertsToFilter));
            TypeRegistry.Add("System.Double", new RegisteredType("System.Double", convertsToFilter));
            TypeRegistry.Add("System.Int32", new RegisteredType("System.Int32", convertsToFilter));
            TypeRegistry.Add("System.String", new RegisteredType("System.String", convertsToFilter));            
        }

        public Evaluator()      
        {
            IdentifierRegistry.Add("System.Math", new IdentifierDefinition("System", "Math", "", Enums.IdentifierDefinitionTypes.Class));
            IdentifierRegistry.Add("System.DateTime", new IdentifierDefinition("System", "DateTime", "", Enums.IdentifierDefinitionTypes.Class));
            IdentifierRegistry.Add("System.String", new IdentifierDefinition("System", "String", "", Enums.IdentifierDefinitionTypes.Class));
            IdentifierRegistry.Add("System.Boolean", new IdentifierDefinition("System", "Boolean", "", Enums.IdentifierDefinitionTypes.Class));

            RegisterIntrinsicMethods();
        }

        public Literal Evaluate(string expression)
        {
            EvaluatorErrors.Clear();
            RPNTokens.Clear();
            OperatorStack.Clear();
            OperandStack.Clear();

            _unaryState = true;
            _fxParamsState = false;
            _isAssignmentStatement = false;

            try
            {

                if (TokenizeExpression(expression).ErrorCode != 0) return null;

                if (UpdateProgress != null) UpdateProgress(this, new EventArgs());

                EvaluatePostfix();
                if (OperandStack.Count == 1)
                {
                    return (Literal)OperandStack.Peek();
                }
                else
                {
                    return null;
                }
            }
            catch (EvaluatorException exc)
            {
                var evaluatorError = exc.EvaluatorError;
                if (evaluatorError == null)
                {
                    evaluatorError = new EvaluatorError(400, exc.Message);
                }
                EvaluatorErrors.Add(evaluatorError);
            }
            catch (Exception exc)
            {
                var evaluatorError = new EvaluatorError(500, exc.Message);
                EvaluatorErrors.Add(evaluatorError);

            }
            return null;    
        }

        private EvaluatorError TokenizeExpression(string expression)
        {
            int characterPosition = 0;
            int lastTokenPosition = 0;
            Token lastToken = null;
            EvaluatorError error;
            Operator op;
            
            if (string.IsNullOrEmpty(expression))
            {
                error = new EvaluatorError(1, "Expression to be evaluated is empty or null", 0, 0, null);
                EvaluatorErrors.Add(error);
                return error;
            }

            MatchCollection matches = tokenizer.Matches(expression);

            foreach (Match match in matches)
            {
                int i = 0;
                foreach (Group group in match.Groups)
                {

                    string matchValue = group.Value;
                    bool success = group.Success && i > 0;      // ignore capture index 0 and 1 (general and WhiteSpace)
                    if (success)
                    {
                        string groupName = tokenizer.GroupNameFromNumber(i);

                        Token token = null;

                        switch (groupName)
                        {
                            case "whitespace":
                                break;
                            case "bool":
                                token = new Literal(matchValue, "System.Boolean", characterPosition);
                                break;
                            case "datetime":
                                token = new Literal(matchValue, "System.DateTime", characterPosition);
                                break;
                            case "double":
                                token = new Literal(matchValue, "System.Double", characterPosition);
                                break;
                            case "integer":
                                token = new Literal(matchValue, "System.Int32", characterPosition);
                                break;
                            case "string":
                                token = new Literal(matchValue, "System.String", characterPosition);
                                break;
                            case "identifier":
                                token = new Identifier(matchValue, characterPosition);
                                ((Identifier)token).ProcessIdentifier += Evaluator_ProcessIdentifier;
                                break;
                            case "dnntoken":
                                token = new DNNToken(matchValue, characterPosition);
                                ((DNNToken)token).ProcessDNNToken += ProcessDNNToken;
                                break;
                            case "function":
                            case "method":
                                token = new MethodCall(matchValue, characterPosition, groupName == "function");
                                ((MethodCall)token).ProcessMethod += Evaluator_ProcessMethod;
                                RPNTokens.Enqueue(new Operand() { TokenType = Enums.TokenTypes.MethodFrame });
                                _fxParamsState = true;
                                break;
                            case "property":
                                token = Operator.PropertyCall(matchValue, characterPosition);
                                break;
                            case "op":
                                switch (matchValue)
                                {
                                    case "+":
                                        token = _unaryState ? Operator.UnaryPlus() : Operator.Add();
                                        break;
                                    case "-":
                                        token = _unaryState ? Operator.UnaryMinus() : Operator.Subtract();
                                        break;
                                    case "*":
                                        token = Operator.Multiply();
                                        break;
                                    case "/":
                                        token = Operator.Divide();
                                        break;
                                    case "&&":
                                        token = Operator.And();
                                        break;
                                    case "||":
                                        token = Operator.Or();
                                        break;
                                    case "!":
                                        token = Operator.Not();
                                        break;
                                    case ">":
                                        token = Operator.GreaterThan();
                                        break;
                                    case "<":
                                        token = Operator.LessThan();
                                        break;
                                    case "%":
                                        token = Operator.Mod();
                                        break;
                                    case "==":
                                        token = Operator.Equal();
                                        break;
                                    case "!=":
                                        token = Operator.NotEqual();
                                        break;
                                    case ">=":
                                        token = Operator.GreaterThanOrEqual();
                                        break;
                                    case "<=":
                                        token = Operator.LessThanOrEqual();
                                        break;
                                    case "(":
                                        token = Operator.LeftParen();
                                        break;
                                    case ")":
                                        token = Operator.RightParen();
                                        break;
                                    case ",":
                                        token = Operator.Comma();
                                        break;
                                    case "=":
                                        token = Operator.Assignment();
                                        _isAssignmentStatement = true;
                                        break;
                                    default:
                                        error = new EvaluatorError(2, "Unknown or not implemented operator '" + matchValue + "'",
                                                                             characterPosition, lastTokenPosition, lastToken);
                                        EvaluatorErrors.Add(error);
                                        break;
                                }
                                token.CharacterPosition = characterPosition;
                                break;
                            case "invalid":
                                error = new EvaluatorError(3, "Unable to parse expression operand or unknown or not implemented operator '" + matchValue + "'",
                                                                   characterPosition, lastTokenPosition, lastToken);
                                EvaluatorErrors.Add(error);
                                break;
                        }

                        if (token == null)
                        {
                            characterPosition += matchValue.Length;
                        }
                        else
                        {
                            switch (token.TokenType)
                            { 
                                case Enums.TokenTypes.Literal:
                                    RPNTokens.Enqueue(token);
                                    _unaryState = false;
                                    break;
                                case Enums.TokenTypes.Identifier:
                                    ((Identifier)token).ResolveTokenType();
                                    RPNTokens.Enqueue(token);
                                    _unaryState = false;
                                    break;
                                case Enums.TokenTypes.DNNToken:
                                    RPNTokens.Enqueue(token);
                                    _unaryState = false;
                                    break;
                                case Enums.TokenTypes.Operator:
                                case Enums.TokenTypes.MethodCall:
                                    op = (Operator)token;
                                    if (op.OperatorName == "LeftParen" || op.TokenType == Enums.TokenTypes.MethodCall)
                                    {
                                        OperatorStack.Push(op);
                                        _unaryState = true;
                                    }
                                    else if (op.OperatorName == "RightParen" || op.OperatorName == "Comma")
                                    {
                                        if (op.OperatorName == "RightParen")
                                        {
                                            while (OperatorStack.Count > 0)
                                            {
                                                op = OperatorStack.Pop();
                                                if (op.OperatorName == "LeftParen") break;
                                                RPNTokens.Enqueue(op);
                                                if (op.TokenType == Enums.TokenTypes.MethodCall) break;
                                            }
                                        }
                                        else
                                        {
                                            _unaryState = true;
                                            while (OperatorStack.Count > 0)
                                            {
                                                op = OperatorStack.Pop();
                                                if (op.TokenType == Enums.TokenTypes.MethodCall)
                                                {
                                                    OperatorStack.Push(op);
                                                    break;
                                                }
                                                RPNTokens.Enqueue(op);
                                            }
                                        }

                                        if (OperatorStack.Count > 0 && !(op.OperatorName == "LeftParen" || op.TokenType == Enums.TokenTypes.MethodCall))
                                        {
                                            var evaluatorError = new EvaluatorError(4, "Mismatched parenthesis", characterPosition, lastTokenPosition, lastToken);
                                            EvaluatorErrors.Add(evaluatorError);
                                        }
                                    }
                                    else
                                    {
                                        _unaryState = true;
                                        if (IsHigherPriority(op))
                                        {
                                            OperatorStack.Push(op);
                                        }
                                        else
                                        {
                                            while (OperatorStack.Count > 0)
                                            {
                                                RPNTokens.Enqueue(OperatorStack.Pop());
                                                if (IsHigherPriority(op))
                                                {
                                                    OperatorStack.Push(op);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            characterPosition += matchValue.Length;
                            lastTokenPosition = RPNTokens.Count - 1;
                            lastToken = token;
                        }
                    }
                    i++;
                }
            }

            while (OperatorStack.Count > 0)
            {
                var token = OperatorStack.Pop();
                RPNTokens.Enqueue(token);
                lastTokenPosition = RPNTokens.Count - 1;
                lastToken = token;
            }

            if (EvaluatorErrors.Count == 0)
            {
                return new EvaluatorError();
            }
            else
            {
                return EvaluatorErrors[EvaluatorErrors.Count - 1];
            }
        }

        private void EvaluatePostfix()
        {
            Identifier identifier;
            if (RPNTokens.Count > 0)
            {
                foreach (Token token in RPNTokens)
                {
                    switch (token.TokenType)
                    {
                        case Enums.TokenTypes.Class:
                            OperandStack.Push((Identifier)token);
                            break;
                        case Enums.TokenTypes.Literal:
                            OperandStack.Push((Literal)token);
                            break;
                        case Enums.TokenTypes.Variable:
                            identifier = (Identifier)token;

                            if (!identifier.IsRegistered)
                            {
                                if (_isAssignmentStatement)
                                {
                                    if (RPNTokens.First() == token)
                                    {
                                        identifier.RegisterIdentifier();
                                        OperandStack.Push(identifier);
                                    }
                                    else
                                    {
                                        throw new EvaluatorException("When used in an assignment statement, variable " + identifier.FullyQualifiedName + " must be the only element to the left of the assignment operator.");
                                    }
                                }
                                else
                                {
                                    throw new EvaluatorException("Variable " + identifier.FullyQualifiedName + " is not defined.");
                                }
                            }
                            else
                            {
                                OperandStack.Push(new Literal(identifier.LoadValue()));
                            }
                            break;
                        case Enums.TokenTypes.DNNToken:
                            OperandStack.Push(new Literal(((DNNToken)token).LoadValue()));
                            break;
                        case Enums.TokenTypes.Operator:
                            Operator op = (Operator)token;
                            if (op.OperatorName == "Assignment" && OperandStack.Count == 2)
                            {
                                var result = (Literal)(OperandStack.Pop());
                                identifier = (Identifier)(OperandStack.Pop());
                                identifier.Value = result.Value;
                                identifier.StoreValue();
                                OperandStack.Push(result);
                            }
                            else if (op.IsUnaryOperator && OperandStack.Count > 0)
                            {
                                switch (op.OperatorName)
                                {
                                    case "Minus":
                                        OperandStack.Push(((Literal)OperandStack.Pop()).Minus());
                                        break;
                                    case "Plus":
                                        OperandStack.Push(((Literal)OperandStack.Pop()).Plus());
                                        break;
                                    case "Not":
                                        OperandStack.Push(((Literal)OperandStack.Pop()).Not());
                                        break;
                                }
                                break;
                            }
                            else if (OperandStack.Count >= 2)
                            {
                                var op2 = (Literal)OperandStack.Pop();
                                var op1 = (Literal)OperandStack.Pop();
                                switch (op.OperatorName)
                                {
                                    case "Add":
                                        OperandStack.Push(op1.Add(op2));
                                        break;
                                    case "Subtract":
                                        OperandStack.Push(op1.Subtract(op2));
                                        break;
                                    case "Multiply":
                                        OperandStack.Push(op1.Multiply(op2));
                                        break;
                                    case "Divide":
                                        OperandStack.Push(op1.Divide(op2));
                                        break;
                                    case "Mod":
                                        OperandStack.Push(op1.Mod(op2));
                                        break;
                                    case "Equal":
                                        OperandStack.Push(op1.Equal(op2));
                                        break;
                                    case "NotEqual":
                                        OperandStack.Push(op1.NotEqual(op2));
                                        break;
                                    case "GreaterThan":
                                        OperandStack.Push(op1.GreaterThan(op2));
                                        break;
                                    case "LessThan":
                                        OperandStack.Push(op1.LessThan(op2));
                                        break;
                                    case "GreaterThanOrEqual":
                                        OperandStack.Push(op1.GreaterThanOrEqual(op2));
                                        break;
                                    case "LessThanOrEqual":
                                        OperandStack.Push(op1.LessThanOrEqual(op2));
                                        break;
                                    case "Or":
                                        OperandStack.Push(op1.Or(op2));
                                        break;
                                    case "And":
                                        OperandStack.Push(op1.And(op2));
                                        break;
                                }
                            }
                            break;
                        case Enums.TokenTypes.MethodFrame:
                            OperandStack.Push((Operand)token);
                            break;
                        case Enums.TokenTypes.MethodCall:
                            var methodCall = (MethodCall)token;
                            OperandStack.Push(new Literal(methodCall.InvokeMethod(OperandStack)));
                            break;
                    }
                }
            }
        }
        
        private bool IsHigherPriority(Operator op)
        {
            return (OperatorStack.Count == 0) || (op.Priority > OperatorStack.Peek().Priority);
        }

        private void RegisterIntrinsicMethods()
        {
            var lastFullName = "";
            var fullName = "";
            var methodDefinitions = new List<MethodDefinition>();
            MethodDefinition methodDefinition = null;

            foreach (string usingName in Constants.Usings)
            {
                var type = Type.GetType(usingName);
                if (type != null)
                {
                    foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (!(methodInfo.IsGenericMethod || methodInfo.IsAbstract || methodInfo.IsSpecialName))
                        {
                            fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
                            if (fullName != lastFullName && methodDefinitions.Count > 0)
                            {
                                AddMethodToRegistry(lastFullName, methodDefinitions);
                                methodDefinitions = new List<MethodDefinition>();
                            }
                            lastFullName = fullName;
                            var parameterList = new List<ParameterDefinition>();
                            foreach (ParameterInfo parameter in methodInfo.GetParameters())
                            {
                                var parameterDefinition = new ParameterDefinition(parameter.ParameterType.FullName, parameter.Name);
                                parameterList.Add(parameterDefinition);
                            }
                            methodDefinition = new MethodDefinition(fullName, parameterList.ToArray());
                            methodDefinition.ReturnType = methodInfo.ReturnType;
                            methodDefinitions.Add(methodDefinition);              
                        }      
                    }
                    if (fullName != lastFullName && methodDefinitions.Count > 0)
                    {
                        AddMethodToRegistry(fullName, methodDefinitions);
                        methodDefinitions = new List<MethodDefinition>();
                        lastFullName = fullName;
                    }
                }
            }
        }

        private void AddMethodToRegistry(string fullName, List<MethodDefinition> methodDefinitions)
        {
            if (MethodRegistry.ContainsKey(fullName))
            {
                MethodRegistry[fullName].AddRange(methodDefinitions);
            }
            else
            {
                MethodRegistry.Add(fullName, methodDefinitions);
            }
        }



        private void Evaluator_ProcessIdentifier(object sender, ProcessIdentifierHandlerArgs e)
        {
            var identifierDefinition = IdentifierRegistry.ContainsKey(e.FullyQualifiedName) ? IdentifierRegistry[e.FullyQualifiedName] : null;
            switch (e.ProcessAction)
            {
                case Enums.ProcessActions.ResolveType:
                    if (identifierDefinition == null)
                    {
                        e.IdentifierDefinitionType = Enums.IdentifierDefinitionTypes.Variable;
                        e.IsRegistered = false;
                    }
                    else
                    {
                        e.IdentifierDefinitionType = identifierDefinition.IdentifierDefinitionType;
                        e.IsRegistered = true;
                    }
                    break;
                case Enums.ProcessActions.Register:
                    if (identifierDefinition == null)
                    {
                        try
                        {
                            identifierDefinition = new IdentifierDefinition(e.FullyQualifiedName, e.IdentifierDefinitionType);
                            IdentifierRegistry.Add(e.FullyQualifiedName, identifierDefinition);
                            e.Error = null;
                        }
                        catch (EvaluatorException exc)
                        {
                            e.Error = new EvaluatorError(100, exc.InnerException.Message);
                        }
                    }
                    else
                    {
                        e.Error = new EvaluatorError(101, "Attempted declaration of already declared variable " + e.FullyQualifiedName);
                    }
                    break;
                case Enums.ProcessActions.Store:
                    if (identifierDefinition == null)
                    {
                        e.Error = new EvaluatorError(102, "Variable " + e.FullyQualifiedName + " is undefined");
                    }
                    else
                    {
                        identifierDefinition.Value = e.Value;
                        e.Error = null;
                    }
                    break;
                case Enums.ProcessActions.Load:
                    if (identifierDefinition == null)
                    {
                        e.Error = new EvaluatorError(103, "Variable " + e.FullyQualifiedName + " is undefined");
                    }
                    else
                    {
                        e.Value = identifierDefinition.Value;
                        e.Error = null;
                    }
                    break;
                default:
                    e.Error = new EvaluatorError(104, "Invalid process variable action (" + e.ProcessAction.ToString() + ")");
                    break;
            }
        }

        private void Evaluator_ProcessMethod(object sender, ProcessMethodHandlerArgs e)
        {
            var methodDefinitions = MethodRegistry.ContainsKey(e.FullyQualifiedName) ? MethodRegistry[e.FullyQualifiedName] : null;
            
            switch (e.ProcessAction)
            {
                case Enums.ProcessActions.ResolveMethodSignature:
                    if (methodDefinitions == null)
                    {
                        e.Error = new EvaluatorError(202, "Method " + e.FullyQualifiedName + " is undefined");
                    }
                    else
                    {
                        try
                        {
                            foreach (MethodDefinition methodDefinition in methodDefinitions)
                            {
                                var parameterTypesMatch = true;
                                if (e.Parameters.Length == methodDefinition.ParameterList.Count)
                                {
                                    var i = 0;
                                    while (parameterTypesMatch && i < e.Parameters.Length)
                                    {
                                        if (e.Parameters[i].GetType().FullName != methodDefinition.ParameterList[i].ParameterTypeName)
                                        {
                                            parameterTypesMatch = false;
                                            break;
                                        }
                                        i++;
                                    }
                                    if (parameterTypesMatch)
                                    {
                                        e.MethodDefinition = methodDefinition;
                                        e.ReturnType = methodDefinition.ReturnType;
                                        e.Error = null;
                                        return;
                                    }
                                }
                            }
                            e.Error = new EvaluatorError(203, "No method with matching signature found for " + e.FullyQualifiedName);
                            e.ReturnValue = null;
                        }
                        catch (Exception exc)
                        {
                            e.Error = new EvaluatorError(204, "Error evaluating method " + e.FullyQualifiedName + ". Inner Exception: " + exc.Message);
                            e.ReturnValue = null;
                        }
                    }
                    break;
                case Enums.ProcessActions.Register:
                    if (methodDefinitions == null)
                    {
                        throw new NotImplementedException("Method declaration not implemented");
                    }
                    else
                    {
                        e.Error = new EvaluatorError(201, "Attempted declaration of already declared method " + e.MethodName);
                    }
                    break;
                case Enums.ProcessActions.Invoke:
                    if (e.MethodDefinition == null)
                    {
                        e.Error = new EvaluatorError(202, "Method " + e.FullyQualifiedName + " is undefined");
                    }
                    else
                    {
                        try
                        {
                            e.ReturnValue = e.MethodDefinition.Invoke(e.Instance, e.Parameters);
                            e.Error = null;
                            return;
                        }
                        catch (Exception exc)
                        {
                            e.Error = new EvaluatorError(204, "Error evaluating method " + e.FullyQualifiedName + ". Inner Exception: " + exc.Message);
                            e.ReturnValue = null;
                        }
                    }
                    break;
                default:
                    e.Error = new EvaluatorError(205, "Invalid process method action (" + e.ProcessAction.ToString() + ")");
                    break;
            }
        }
    }
}
