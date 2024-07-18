using CSLox.Parsing.Exceptions;

namespace CSLox.Parsing;

public sealed partial class Parser
{
    internal static Environment s_environment = new();
    
    internal class Environment
    {
        private readonly Dictionary<string, object?> _declarations = [];
        public Environment? m_environment;

        public Environment() { }

        public Environment(Environment environment)
        {
            m_environment = environment;
        }

        public void Decalare(string name, object? value)
        {
            if (_declarations.ContainsKey(name))
            {
                _declarations[name] = value;
                return;
            }

            _declarations.Add(name, value);
        }

        public void Assign(string name, object value)
        {
            if (_declarations.ContainsKey(name))
            {
                _declarations[name] = value;
                return;
            }

            if (m_environment is not null)
            {
                m_environment.Assign(name, value);
                return;
            }

            throw new RuntimeException($"Undefined variable named: {name}");
        }

        public object? GetVariableValue(string name)
        {
            if (_declarations.TryGetValue(name, out var value)) return value;

            if (m_environment is not null) return m_environment.GetVariableValue(name);

            throw new RuntimeException($"Undefined variable named: '{name}'");
        }
    }
}