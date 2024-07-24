using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record ClassStatement(Token Name, IList<FunctionStatement> Methods) : Statement
{
    public override void Resolve()
    {
        Resolver.Declare(Name.Lexeme);
        Resolver.Define(Name.Lexeme);

        Resolver.BeginScope();
            Resolver.Declare("this");
            Resolver.Define("this");
            foreach (var method in Methods)
            {
                Resolver.CurrentScopeType = Enums.ScopeType.Method;
                method.Resolve();
            }
        Resolver.EndScope();
    }

    public override void Interpret()
    {
        Interpreter.Environment.Declare(Name.Lexeme, null);
        
        var methods = new Dictionary<string, LoxFunction>();
        foreach (var function in Methods)
        {
            var method = new LoxFunction(function, Interpreter.Environment);
            methods.Add(function.Name.Lexeme, method);
        }
        
        var @class = new LoxClass(Name.Lexeme, methods);
        Interpreter.Environment.Assign(Name.Lexeme, @class);
    }
}