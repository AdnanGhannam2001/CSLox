using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Super(Token Keyword, Token Method) : Expr
{
    public override void Resolve()
    {
        Resolver.ResolveLocalVariable(this, Keyword);
    }

    public override object Interpret()
    {
        var distance = Interpreter.ResolveMap.GetValueOrDefault(this);
        var superClass = (LoxClass) Interpreter.Environment.GetVariableValue(Keyword.Lexeme, distance);
        var @object = (LoxObject) Interpreter.Environment.GetVariableValue("this", distance - 1);
        var method = superClass.Methods.GetValueOrDefault(Method.Lexeme)
            ??  throw new RuntimeException($"Method '{Method.Lexeme}' is not defined on super class");

        return method.Bind(@object);
    }

    internal override (int counter, string content) Draw()
    {
        throw new NotImplementedException();
    }
}