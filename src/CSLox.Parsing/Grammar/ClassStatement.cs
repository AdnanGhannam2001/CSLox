using CSLox.Core.Exceptions;
using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record ClassStatement(Token Name, Variable? SuperClass, IList<FunctionStatement> Methods) : Statement
{
    public override void Resolve()
    {
        Resolver.Declare(Name.Lexeme);
        Resolver.Define(Name.Lexeme);

        if (Name.Lexeme == SuperClass?.Identifier.Lexeme)
        {
            throw new LoxException("A class can't inherit from itself");
        }


        if (SuperClass is not null)
        {
            SuperClass.Resolve();

            Resolver.BeginScope();
            Resolver.Declare("super");
            Resolver.Define("super");
        }

        Resolver.BeginScope();
            Resolver.Declare("this");
            Resolver.Define("this");
            foreach (var method in Methods)
            {
                Resolver.CurrentScopeType = Enums.ScopeType.Method;
                method.Resolve();
            }
        Resolver.EndScope();

        if (SuperClass is not null)
        {
            Resolver.EndScope();
        }
    }

    public override void Interpret()
    {
        var superClass = SuperClass?.Interpret();
        if (superClass is not null && superClass is not LoxClass)
        {
            throw new RuntimeException("Super class should be a class");
        }

        Interpreter.Environment.Declare(Name.Lexeme, null);

        if (superClass is not null)
        {
            Interpreter.Environment = new (Interpreter.Environment);
            Interpreter.Environment.Declare("super", superClass);
        }
        
        var methods = new Dictionary<string, LoxFunction>();
        foreach (var function in Methods)
        {
            var method = new LoxFunction(function, Interpreter.Environment);
            methods.Add(function.Name.Lexeme, method);
        }
        
        var @class = new LoxClass(Name.Lexeme, (LoxClass?) superClass, methods);

        if (superClass is not null)
        {
            Interpreter.Environment = Interpreter.Environment.m_environment!;
        }

        Interpreter.Environment.Assign(Name.Lexeme, @class);
    }
}