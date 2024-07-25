using CSLox.Parsing.Interfaces;
using CSLox.Parsing.Resolving;

namespace CSLox.Parsing;

internal sealed class LoxClass : ICallable
{
    private readonly string _name;
    internal readonly LoxClass? SuperClass;

    internal readonly Dictionary<string, LoxFunction> Methods;

    public LoxClass(string name, LoxClass? superClass, Dictionary<string, LoxFunction> methods)
    {
        _name = name;
        SuperClass = superClass;
        Methods = methods;
    }

    public object? Invoke(IList<object> arguments)
    {
        var @object = new LoxObject(this);

        if (@object.Get("init") is LoxFunction constructor)
        {
            Resolver.CurrentScopeType = Enums.ScopeType.Constructor;
            constructor.Invoke(arguments);
        }
        else
        {
            ((ICallable) this).CheckArity(0, arguments.Count);
        }

        return @object;
    }

    public override string ToString()
    {
        return $"<class {_name}>";
    }

    public override int GetHashCode() => base.GetHashCode() ^ 11;
} 