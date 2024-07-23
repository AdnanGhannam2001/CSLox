using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record ClassStatement(Token Name, IList<Statement> Functions) : Statement
{
    public override void Resolve()
    {
        Resolver.Declare(Name.Lexeme);
        Resolver.Define(Name.Lexeme);
    }

    public override void Interpret()
    {
        Interpreter.Environment.Declare(Name.Lexeme, null);
        var @class = new LoxClass(Name.Lexeme);
        Interpreter.Environment.Assign(Name.Lexeme, @class);
    }
}