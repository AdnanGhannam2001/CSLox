using CSLox.Core.Exceptions;
using CSLox.Scanning.Constants;
using CSLox.Scanning.Enums;
using static CSLox.Scanning.Enums.TokenType;

namespace CSLox.Scanning;

public sealed class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens = [];

    private int _line = 1;
    private int _lineStartsAt = 0;
    private int _start = 0;
    private int _current = 0;

    private bool IsAtEnd => _source.Length <= _current;
    private char? Next => !IsAtEnd ? _source[_current] : null;
    private string CurrentToken => _source[_start.._current];

    public Scanner(string source)
    {
        _source = source;
    }

    public IEnumerable<Token> ScanTokens()
    {
        while (!IsAtEnd)
        {
            _start = _current;
            ScanToken();
        }

        _start = _current;
        AddToken(EOF, null);

        return _tokens;
    }

    private void ScanToken()
    {
        var c = _source[_current++];

        switch (c)
        {
            case '(': { AddToken(LEFT_PAREN);  } break;
            case ')': { AddToken(RIGHT_PAREN); } break;
            case '{': { AddToken(LEFT_BRACE);  } break;
            case '}': { AddToken(RIGHT_BRACE); } break;
            case ',': { AddToken(COMMA);       } break;
            case '.': { AddToken(DOT);         } break;
            case ';': { AddToken(SEMICOLON);   } break;
            case '+': { AddToken(PLUS);        } break;
            case '-': { AddToken(MINUS);       } break;
            case '*': { AddToken(STAR);        } break;
            case '/':
            {
                // Comments
                if (Next == '/')
                {
                    while (!IsAtEnd && _source[_current++] != '\n');
                    NewLine();
                }
                else AddToken(SLASH);
            }
            break;

            case ' ':
            case '\t':
            case '\r':
                _start++;
                break;
            case '\n': { NewLine(); } break;

            case '!': { AddToken(NextMatches('=') ? BANG_EQUAL : BANG);       } break;
            case '=': { AddToken(NextMatches('=') ? EQUAL_EQUAL : EQUAL);     } break;
            case '>': { AddToken(NextMatches('=') ? GREATER_EQUAL : GREATER); } break;
            case '<': { AddToken(NextMatches('=') ? LESS_EQUAL : LESS);       } break;

            case '"': { ScanString(); } break;

            default:
            {
                if (IsDigit(c))
                {
                    ScanNumber();
                    break;
                }
                else if (IsAlpha(c))
                {
                    ScanIdentifer();
                    break;
                }

                throw new ScannerException(CurrentToken, _line, _start, _current);
            }
        }
    }

    private static bool IsDigit(char c) => c >= '0' && c <= '9';
    private static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    private static bool IsAlphaNumeric(char c) => IsDigit(c) || IsAlpha(c);

    private void NewLine()
    {
        _line++;
        _lineStartsAt = _start;
    }

    private bool NextMatches(char c)
    {
        if (IsAtEnd || Next != c) return false;

        _current++;
        return true;
    }

    private void ScanString()
    {
        while (!IsAtEnd && Next != '"')
        {
            if (Next == '\n') _line++;

            _current++;
        }

        if (IsAtEnd) throw new ScannerException(CurrentToken, _line, _start, _current);

        _start++;
        AddToken(STRING, CurrentToken);
        _current++;
    }

    private void ScanNumber()
    {
        while (!IsAtEnd && IsDigit((char) Next!)) _current++;

        if (!IsAtEnd && Next == '.' && ++_current > 0)
        {
            if (!IsAtEnd && IsDigit((char) Next)) while (!IsAtEnd && IsDigit((char) Next!)) _current++;
            else _current--;
        }

        AddToken(NUMBER, double.Parse(CurrentToken));
    }

    private void ScanIdentifer()
    {
        while (!IsAtEnd && IsAlphaNumeric((char) Next!)) _current++;

        if (Keywords.Values.TryGetValue(CurrentToken, out var keyword))
        {
            AddToken(keyword, CurrentToken);
        }
        else
        {
            AddToken(IDENTIFIER, CurrentToken);
        }
    }

    private void AddToken(TokenType type, object? literal = null)
    {
        _tokens.Add(new Token(
                type, CurrentToken, literal, _line, _start - _lineStartsAt, _start, _current - _start)); 
    }
}
