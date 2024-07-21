using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record FunctionStatement(Token Name, IList<Token> Parameters, IList<Statement> Body) : Statement
{
    public override void Interpret()
    {
        var function = new LoxFunction(this, Parser.s_environment);
        Parser.s_environment.Decalare(Name.Lexeme, function);
    }
}