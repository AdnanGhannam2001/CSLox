namespace CSLox.Parsing;

internal sealed record LoxObject(LoxClass Class)
{
    private readonly Dictionary<string, object> _fields = [];

    public object? Get(string name)
    {
        var field = _fields.GetValueOrDefault(name);
        if (field is not null)
        {
            return field;
        }

        var method = Class.Methods.GetValueOrDefault(name)?.Bind(this);
        if (method is not null)
        {
            return method;
        }

        return Class.SuperClass?.Methods.GetValueOrDefault(name);
    }

    public void Set(string name, object value)
    {
        _fields[name] = value;
    }

    public override string ToString()
    {
        return $"<object {Class}>";
    }
}