using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record FunctionStatement(Token Name, IList<Token> Parameters, IList<Statement> Body) : Statement
{
    public override void Resolve()
    {
        var type = Resolver.CurrentScopeType;
        if (type != Enums.ScopeType.Method) Resolver.CurrentScopeType = Enums.ScopeType.Function;
        Resolver.Declare(Name.Lexeme);
        Resolver.Define(Name.Lexeme);

        Resolver.BeginScope();
            foreach (var param in Parameters)
            {
                Resolver.Declare(param.Lexeme);
                Resolver.Define(param.Lexeme);
            }
            Resolver.Resolve(Body);
        Resolver.EndScope();
        Resolver.CurrentScopeType = type;
    }

    public override void Interpret()
    {
        var function = new LoxFunction(this, Interpreter.Environment);
        Interpreter.Environment.Declare(Name.Lexeme, function);
    }
}