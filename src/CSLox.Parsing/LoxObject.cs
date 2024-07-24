namespace CSLox.Parsing;

internal sealed record LoxObject(LoxClass Class)
{
    private readonly Dictionary<string, object> _fields = [];

    public object? Get(string name)
    {
        var field = _fields.GetValueOrDefault(name);

        return field is not null
            ? field
            : (Class.Methods.GetValueOrDefault(name)?.Bind(this));
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