using CSLox.Parsing.Interpreting;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record VarDeclaration(Token Name, Expr? Initializer = null) : Statement
{
    public override void Interpret()
    {
        Interpreter.Environment.Decalare(Name.Lexeme, Initializer?.Interpret());
    }
}
