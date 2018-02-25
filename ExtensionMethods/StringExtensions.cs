using System;
using System.Text;

namespace BlogML2Ghost.Core.ExtensionMethods
{
    public static class StringExtensions
    {
        // Copied from http://www.danharman.net/2011/07/18/seo-slugification-in-dotnet-aka-unicode-to-ascii-aka-diacritic-stripping/
        // Substitution table copied from https://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696
        public static string AsSlug(this string value, int maxLength)
        {
            if (value == null) return "";

            var normalised = value.Normalize(NormalizationForm.FormKD);

            int len = normalised.Length;
            bool prevDash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = normalised[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        prevDash = true;
                    }
                }
                else
                {
                    string swap = RemapInternationalCharToAscii(c);
                    if (swap != null)
                    {
                        if (prevDash)
                        {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(swap);
                    }
                }

                if (sb.Length == maxLength) break;
            }

            return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }


}