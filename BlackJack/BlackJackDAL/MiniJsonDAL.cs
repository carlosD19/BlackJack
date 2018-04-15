using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BlackJackDAL
{
    public class Json
    {
        private static NumberFormatInfo numberFormat = (new CultureInfo("en-US")).NumberFormat;

        /// <summary>
        /// Parses the string json into a value.
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false.</returns>
        public object Deserialize(string json)
        {
            return Parser.Parse(json);
        }

        private sealed class Parser : IDisposable
        {
            private const string WhiteSpace = " \t\n\r";
            private const string WordBreak = " \t\n\r{}[],:\"";

            private StringReader json;

            private Parser(string jsonString)
            {
                this.json = new StringReader(jsonString);
            }

            private enum TOKEN
            {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            }

            private char PeekChar
            {
                get
                {
                    return Convert.ToChar(this.json.Peek());
                }
            }

            private char NextChar
            {
                get
                {
                    return Convert.ToChar(this.json.Read());
                }
            }

            private string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();

                    while (WordBreak.IndexOf(this.PeekChar) == -1)
                    {
                        word.Append(this.NextChar);

                        if (this.json.Peek() == -1)
                        {
                            break;
                        }
                    }

                    return word.ToString();
                }
            }

            private TOKEN NextToken
            {
                get
                {
                    this.EatWhitespace();

                    if (this.json.Peek() == -1)
                    {
                        return TOKEN.NONE;
                    }

                    char c = this.PeekChar;
                    switch (c)
                    {
                        case '{':
                            return TOKEN.CURLY_OPEN;
                        case '}':
                            this.json.Read();
                            return TOKEN.CURLY_CLOSE;
                        case '[':
                            return TOKEN.SQUARED_OPEN;
                        case ']':
                            this.json.Read();
                            return TOKEN.SQUARED_CLOSE;
                        case ',':
                            this.json.Read();
                            return TOKEN.COMMA;
                        case '"':
                            return TOKEN.STRING;
                        case ':':
                            return TOKEN.COLON;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '-':
                            return TOKEN.NUMBER;
                    }

                    string word = this.NextWord;

                    switch (word)
                    {
                        case "false":
                            return TOKEN.FALSE;
                        case "true":
                            return TOKEN.TRUE;
                        case "null":
                            return TOKEN.NULL;
                    }

                    return TOKEN.NONE;
                }
            }

            public static object Parse(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseValue();
                }
            }

            public void Dispose()
            {
                this.json.Dispose();
                this.json = null;
            }

            private Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();

                this.json.Read();

                while (true)
                {
                    switch (this.NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            // name
                            string name = this.ParseString();
                            if (name == null)
                            {
                                return null;
                            }

                            // :
                            if (this.NextToken != TOKEN.COLON)
                            {
                                return null;
                            }

                            // ditch the colon
                            this.json.Read();

                            // value
                            table[name] = this.ParseValue();
                            break;
                    }
                }
            }

            private List<object> ParseArray()
            {
                List<object> array = new List<object>();

                // ditch opening bracket
                this.json.Read();

                // [
                var parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = this.NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = this.ParseByToken(nextToken);

                            array.Add(value);
                            break;
                    }
                }

                return array;
            }

            private object ParseValue()
            {
                TOKEN nextToken = this.NextToken;
                return this.ParseByToken(nextToken);
            }

            private object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return this.ParseString();
                    case TOKEN.NUMBER:
                        return this.ParseNumber();
                    case TOKEN.CURLY_OPEN:
                        return this.ParseObject();
                    case TOKEN.SQUARED_OPEN:
                        return this.ParseArray();
                    case TOKEN.TRUE:
                        return true;
                    case TOKEN.FALSE:
                        return false;
                    case TOKEN.NULL:
                        return null;
                    default:
                        return null;
                }
            }

            private string ParseString()
            {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                this.json.Read();

                bool parsing = true;
                while (parsing)
                {
                    if (this.json.Peek() == -1)
                    {
                        parsing = false;
                        break;
                    }

                    c = this.NextChar;
                    switch (c)
                    {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (this.json.Peek() == -1)
                            {
                                parsing = false;
                                break;
                            }

                            c = this.NextChar;
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new StringBuilder();

                                    for (int i = 0; i < 4; i++)
                                    {
                                        hex.Append(this.NextChar);
                                    }

                                    s.Append((char)Convert.ToInt32(hex.ToString(), 16));
                                    break;
                            }

                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }

                return s.ToString();
            }

            private object ParseNumber()
            {
                string number = this.NextWord;

                if (number.IndexOf('.') == -1)
                {
                    return long.Parse(number, numberFormat);
                }

                return double.Parse(number, numberFormat);
            }

            private void EatWhitespace()
            {
                while (WhiteSpace.IndexOf(this.PeekChar) != -1)
                {
                    this.json.Read();

                    if (this.json.Peek() == -1)
                    {
                        break;
                    }
                }
            }
        }
    }
}
