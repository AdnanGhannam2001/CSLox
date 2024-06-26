using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record VarDeclaration(Token Name, Expr? Initializer = null) : Statement
{
    public override void Interpret()
    {
        Parser.s_environment.Decalare(Name.Lexeme, Initializer?.Interpret());
    }
}
