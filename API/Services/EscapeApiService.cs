using System.Text;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public class EscapeApiService : IEscapeApiService
    {
        public EscapeResult Escape(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\'':
                        sb.Append("\\'");
                        break;
                    default:
                        if (c >= 32 && c <= 126)
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            sb.Append($"\\u{(int)c:X4}");
                        }
                        break;
                }
            }

            return new EscapeResult
            {
                Input = input,
                Output = sb.ToString(),
                Operation = "escape"
            };
        }

        public EscapeResult Unescape(string input)
        {
            var sb = new StringBuilder();
            var i = 0;
            while (i < input.Length)
            {
                if (input[i] == '\\' && i + 1 < input.Length)
                {
                    var next = input[i + 1];
                    switch (next)
                    {
                        case 'a':
                            sb.Append('\a');
                            i += 2;
                            continue;
                        case 'b':
                            sb.Append('\b');
                            i += 2;
                            continue;
                        case 'f':
                            sb.Append('\f');
                            i += 2;
                            continue;
                        case 'n':
                            sb.Append('\n');
                            i += 2;
                            continue;
                        case 'r':
                            sb.Append('\r');
                            i += 2;
                            continue;
                        case 't':
                            sb.Append('\t');
                            i += 2;
                            continue;
                        case 'v':
                            sb.Append('\v');
                            i += 2;
                            continue;
                        case '\\':
                            sb.Append('\\');
                            i += 2;
                            continue;
                        case '"':
                            sb.Append('\"');
                            i += 2;
                            continue;
                        case '\'':
                            sb.Append('\'');
                            i += 2;
                            continue;
                        case 'u':
                            if (i + 5 < input.Length)
                            {
                                var hex = input.Substring(i + 2, 4);
                                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var value))
                                {
                                    sb.Append((char)value);
                                    i += 6;
                                    continue;
                                }
                            }
                            break;
                    }
                }
                sb.Append(input[i]);
                i++;
            }

            return new EscapeResult
            {
                Input = input,
                Output = sb.ToString(),
                Operation = "unescape"
            };
        }
    }
}
