namespace CSLox.Parsing;

internal sealed record LoxObject(LoxClass Class)
{
    private readonly Dictionary<string, object> _fields = [];

    public object? GetField(string name)
    {
        return _fields.GetValueOrDefault(name);
    }

    public void SetField(string name, object value)
    {
        _fields[name] = value;
    }

    public override string ToString()
    {
        return $"<object {Class}>";
    }
}