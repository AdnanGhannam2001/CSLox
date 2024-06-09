using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Grammar;
using CSLox.Scanning;
using CSLox.Scanning.Enums;
using static CSLox.Scanning.Enums.TokenType;

namespace CSLox.Parsing;

public sealed class Parser
{
    private readonly IEnumerable<Token> _tokens;
    private int _current = 0;
    private Token CurrentToken => _tokens.ElementAt(_current);
    private bool IsAtEnd => CurrentToken.Type == EOF;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
    }

    public Expr Parse()
    {
        return Expression();
    }

    private Expr Expression()
    {
        return Equality();
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

        return Primary();
    }

    private Expr Primary()
    {
        if (MatchesAny(TRUE)) return new Literal(true);
        if (MatchesAny(FALSE)) return new Literal(false);
        if (MatchesAny(NUMBER) || MatchesAny(STRING)) return new Literal(_tokens.ElementAt(_current - 1).Lexeme);

        if (MatchesAny(LEFT_PAREN))
        {
            var expr = Expression();
            Consume(RIGHT_PAREN);
            return new Grouping(expr);
        }

        throw new UnexpectedTokenException(CurrentToken, "Excpected expression");
    }

    private bool MatchesAny(params TokenType[] types)
    {
        if (types.Any(TypeEquals))
        {
            _current++;
            return true;
        }

        return false;
    }

    private bool TypeEquals(TokenType type)
    {
        if (IsAtEnd) return false;

        return CurrentToken.Type == type;
    }

    private Token Consume(TokenType type)
    {
        if (MatchesAny(type)) return _tokens.ElementAt(_current - 1);

        throw new UnexpectedTokenException(CurrentToken, $"Excpected {type} instead");
    }
}
