using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BmiRechner
{
    public static class NavigationHelper
    {
        public static Uri BuildUri(string pageName)
        {
            return new Uri(pageName, UriKind.Relative);
        }

        public static Uri BuildUri(string pageName, string parameterName, string parameterValue)
        {
            string navigationUri = string.Format("{0}?{1}={2}", pageName, parameterName, parameterValue);
            string escapedUri = Uri.EscapeUriString(navigationUri);
            return new Uri(escapedUri, UriKind.Relative);
        }

        public static Uri BuildUri(string pageName, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                // Keine Parameter angegeben
                return new Uri(pageName, UriKind.Relative);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(pageName + "?");

            string[] keys = parameters.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                string value = parameters[key].ToString();

                sb.AppendFormat("{0}={1}", key, value);

                if (i != keys.Length - 1)
                {
                    // Es gibt noch nachfolgende Elemente - & anhängen
                    sb.Append("&");
                }
            }

            string escapedUri = Uri.EscapeUriString(sb.ToString());
            return new Uri(escapedUri, UriKind.Relative);
        }
    }
}
