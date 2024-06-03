using System.Text;

namespace CSLox.Core.Exceptions;

public class ScannerException : LoxException
{
    public ScannerException(string lexeme, int line, int start, int end) : base("Syntax Error")
    {

        Details = $"Unexpected '{lexeme}' at line: {line}, columns: {start}-{end}";
    }
}