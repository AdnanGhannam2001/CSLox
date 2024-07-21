using CSLox.Parsing.Interpreting;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record FunctionStatement(Token Name, IList<Token> Parameters, IList<Statement> Body) : Statement
{
    public override void Interpret()
    {
        var function = new LoxFunction(this, Interpreter.Environment);
        Interpreter.Environment.Decalare(Name.Lexeme, function);
    }
}