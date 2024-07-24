using CSLox.Core.Exceptions;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record This(Token Keyword) : Expr
{
    public override void Resolve()
    {
        if (Resolver.CurrentScopeType != Enums.ScopeType.Method)
        {
            throw new LoxException("Invalid use of 'this'", "Can't use 'this' keyword outside of a class");
        }

        Resolver.ResolveLocalVariable(this, Keyword);
    }

    public override object Interpret()
    {
        return Interpreter.GetVariable("this", this);
    }

    internal override (int counter, string content) Draw()
    {
        // TODO
        throw new NotImplementedException();
    }
}