using System.Linq;
using System.Text;

namespace M0LTE.WsjtxUdpLib
{
    internal static class ExtensionMethods
    {
        public static string ToCompactLine(this object o, params string[] argsToSkip)
        {
            var sb = new StringBuilder();
            foreach (var prop in o.GetType().GetProperties())
            {
                if (argsToSkip != null && argsToSkip.Contains(prop.Name))
                {
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }

                sb.Append(prop.Name.Substring(0, 1).ToLower());
                sb.Append(prop.Name.Substring(1));
                sb.Append(":");
                sb.Append(prop.GetValue(o));
            }

            return sb.ToString();
        }
    }
}
