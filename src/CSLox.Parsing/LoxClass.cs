using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing;

internal sealed class LoxClass : ICallable
{
    private readonly string _name;

    public LoxClass(string name) => _name = name;

    public object? Invoke(IList<object> arguments)
    {
        // TODO
        // ((ICallable) this).CheckArity(, arguments.Count);
        return new LoxObject(this);
    }

    public override string ToString()
    {
        return $"<class {_name}>";
    }

    public override int GetHashCode() => base.GetHashCode() ^ 11;
} 