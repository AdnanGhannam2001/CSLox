using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Grammar;
using CSLox.Scanning;
using CSLox.Scanning.Enums;
using static CSLox.Scanning.Enums.TokenType;

namespace CSLox.Parsing;

public sealed class Parser
{
    private readonly string _content;
    private readonly IEnumerable<Token> _tokens;
    private int _current = 0;
    private Token CurrentToken => _tokens.ElementAt(_current);
    private bool IsAtEnd => CurrentToken.Type == EOF;

    public Parser(string content, IEnumerable<Token> tokens)
    {
        _content = content;
        _tokens = tokens;
    }

    public IEnumerable<Statement> Parse()
    {
        var statements = new List<Statement>();
        while (!IsAtEnd)
        {
            statements.Add(Declaration());
        }

        return statements;
    }

    private Statement Declaration()
    {
        if (MatchesAny(CLASS)) return ClassDeclaration();
        if (MatchesAny(FUN)) return FunctionDeclaration();
        if (MatchesAny(VAR)) return VarDeclaration();

        return Statement();
    }

    private Statement ClassDeclaration()
    {
        var name = Consume(IDENTIFIER);

        Variable? superClass = null;
        if (MatchesAny(LESS))
        {
            superClass = new (Consume(IDENTIFIER));
        }

        Consume(LEFT_BRACE);

        var functions = new List<FunctionStatement>();
        while (TokenTypeEquals(IDENTIFIER))
        {
            functions.Add(FunctionDeclaration());
        }
        Consume(RIGHT_BRACE);

        return new ClassStatement(name, superClass, functions);
    }

    private FunctionStatement FunctionDeclaration()
    {
        var identifier = Consume(IDENTIFIER);
        var parameters = Parameters();
        Consume(LEFT_BRACE);
        var body = Block();

        return new FunctionStatement(identifier, parameters, body);
    }

    private IList<Token> Parameters()
    {
        Consume(LEFT_PAREN);
        var parameters = new List<Token>();
        do 
        {
            if (parameters.Count >= Grammar.Call.MAX_ARGUMENTS)
            {
                throw new RuntimeException($"You can't define a function with too many parameters (max = {Grammar.Call.MAX_ARGUMENTS})");
            }

            if (!TokenTypeEquals(RIGHT_PAREN))
            {
                parameters.Add(Consume(IDENTIFIER));
            }
        }
        while (MatchesAny(COMMA));
        Consume(RIGHT_PAREN); // Consumes ')'

        return parameters;
    }

    private Statement VarDeclaration()
    {
        var identifier = Consume(IDENTIFIER);
        var initExpr = MatchesAny(EQUAL) ? Expression() : null;
        Consume(SEMICOLON);

        return new VarDeclaration(identifier, initExpr);
    }

    private Statement Statement()
    {
        if (MatchesAny(FOR)) return ForStatement();
        if (MatchesAny(IF)) return IfStatement();
        if (MatchesAny(PRINT)) return PrintStatement();
        if (MatchesAny(RETURN)) return ReturnStatement();
        if (MatchesAny(WHILE)) return WhileStatement();
        if (MatchesAny(LEFT_BRACE)) return new Block(Block());

        return ExpressionStatement();
    }

    private Statement PrintStatement()
    {
        var expr = Expression();
        Consume(SEMICOLON);

        return new PrintStatement(expr);
    }

    private Statement ReturnStatement()
    {
        Expr? expr = null;
        if (!MatchesAny(SEMICOLON))
        {
            expr = Expression();
            Consume(SEMICOLON);
        }

        return new ReturnStatement(expr);
    }

    private IList<Statement> Block()
    {
        var statements = new List<Statement>();
        while (!MatchesAny(RIGHT_BRACE))
        {
            if (IsAtEnd)
            {
                throw new RuntimeException("Expected '}'");
            }

            statements.Add(Declaration());
        }

        return statements;
    }

    private Statement ForStatement()
    {
        Consume(LEFT_PAREN);
        Statement? init = null;

        if (!MatchesAny(SEMICOLON))
        {
            init = MatchesAny(VAR) ? VarDeclaration() : ExpressionStatement();
        }

        Expr? condition = null;
        if (!MatchesAny(SEMICOLON))
        {
            condition = Expression();
            Consume(SEMICOLON);
        }

        Expr? sideEffect = null;
        if (!MatchesAny(RIGHT_PAREN))
        {
            sideEffect = Expression();
            Consume(RIGHT_PAREN);
        }

        var body = Statement();

        return new ForStatement(init, condition, sideEffect, body);
    }

    private Statement IfStatement()
    {
        Consume(LEFT_PAREN);
        var condition = Expression();
        Consume(RIGHT_PAREN);

        var then = Statement();

        Statement? @else = null;
        if (MatchesAny(ELSE))
        {
            @else = Statement();
        }

        return new IfStatement(condition, then, @else);
    }

    private Statement WhileStatement()
    {
        Consume(LEFT_PAREN);
        var condition = Expression();
        Consume(RIGHT_PAREN);

        var body = Statement();

        return new WhileStatement(condition, body);
    }

    private Statement ExpressionStatement()
    {
        var expr = Expression();
        Consume(SEMICOLON);
        return new ExpressionStatement(expr);
    }

    private Expr Expression()
    {
        return Assignment();
    }

    private Expr Assignment()
    {
        var expr = Or();

        if (MatchesAny(EQUAL))
        {
            var assignment = Assignment();

            if (expr is Variable variable)
            {
                return new Assignment(variable.Identifier, assignment);
            }
            else if (expr is Get getExpr)
            {
                return new Set(getExpr.Object, getExpr.Field, assignment);
            }

            throw new RuntimeException("Invalid assignment");
        }

        return expr;
    }

    private Expr Or()
    {
        var expr = And();

        while (MatchesAny(OR))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Equality();
            expr = new Logical(expr, op, right);
        }

        return expr;
    }

    private Expr And()
    {
        var expr = Equality();

        while (MatchesAny(AND))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Equality();
            expr = new Logical(expr, op, right);
        }

        return expr;
    }

    private Expr Equality()
    {
        var expr = Comparison();

        while (MatchesAny(BANG_EQUAL, EQUAL_EQUAL))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Comparison();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Comparison()
    {
        var expr = Term();

        while (MatchesAny(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Term();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Term()
    {
        var expr = Factor();

        while (MatchesAny(PLUS, MINUS))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Factor();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Factor()
    {
        var expr = Unary();

        while (MatchesAny(STAR, SLASH))
        {
            var op = _tokens.ElementAt(_current - 1);
            var right = Unary();
            expr = new Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Unary()
    {
        if (MatchesAny(BANG, MINUS))
        {
            var op = _tokens.ElementAt(_current - 1);
            var exp = Unary();
            return new Unary(op, exp);
        }

        return Call();
    }

    private Expr Call()
    {
        var expr = Primary();

        while (true)
        {
            if (MatchesAny(LEFT_PAREN))
            {
                var arguments = Arguments();
                expr = new Call(expr, arguments);
            }
            else if (MatchesAny(DOT))
            {
                var field = Consume(IDENTIFIER);
                expr = new Get(expr, field);
            }
            else
            {
                break;
            }
        }

        return expr;
    }

    private IList<Expr> Arguments()
    {
        var arguments = new List<Expr>();
        do
        {
            if (arguments.Count >= Grammar.Call.MAX_ARGUMENTS)
            {
                throw new RuntimeException($"Can't have that much argument to a function (max = {Grammar.Call.MAX_ARGUMENTS})");
            }

            if (!TokenTypeEquals(RIGHT_PAREN))
            {
                arguments.Add(Expression());
            }
        }
        while (MatchesAny(COMMA));
        Consume(RIGHT_PAREN); // Consumes ')'

        return arguments;
    }

    private Expr Primary()
    {
        if (MatchesAny(TRUE)) return new Literal(true);
        if (MatchesAny(FALSE)) return new Literal(false);
        if (MatchesAny(NIL)) return new Literal(null);
        if (MatchesAny(NUMBER) || MatchesAny(STRING)) return new Literal(_tokens.ElementAt(_current - 1).Literal!);
        if (MatchesAny(LEFT_PAREN))
        {
            var expr = Expression();
            Consume(RIGHT_PAREN);
            return new Grouping(expr);
        }
        if (TokenTypeEquals(SUPER))
        {
            var keyword = Consume(SUPER);
            Consume(DOT);
            var method = Consume(IDENTIFIER);
            return new Super(keyword, method);
        }
        if (TokenTypeEquals(THIS)) return new This(Consume(THIS));
        if (MatchesAny(IDENTIFIER)) return new Variable(_tokens.ElementAt(_current - 1));

        throw new UnexpectedTokenException(_content, CurrentToken, "expression");
    }

    private bool MatchesAny(params TokenType[] types)
    {
        if (types.Any(TokenTypeEquals))
        {
            _current++;
            return true;
        }

        return false;
    }

    private bool TokenTypeEquals(TokenType type)
    {
        if (IsAtEnd) return false;

        return CurrentToken.Type == type;
    }

    private Token Consume(TokenType type, string? expected = null)
    {
        if (MatchesAny(type)) return _tokens.ElementAt(_current - 1);

        throw new UnexpectedTokenException(_content, CurrentToken, expected ?? type.ToString());
    }
}
