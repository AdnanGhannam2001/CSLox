using CSLox.Parsing.Exceptions;

namespace CSLox.Parsing.Interfaces;

public interface ICallable
{
    public void CheckArity(int arity, int passedArguments)
    {
        if (passedArguments < arity)
        {
            throw new RuntimeException($"Too few arguments, got: '{passedArguments}' expected: '{arity}'");
        }
        else if (passedArguments > arity)
        {
            throw new RuntimeException($"Too many arguments, got: '{passedArguments}' expected: '{arity}'");
        }
    }

    object? Invoke(IList<object> arguments);
}