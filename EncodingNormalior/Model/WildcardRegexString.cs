using System.Text.RegularExpressions;

namespace EncodingNormalior.Model
{
    public static class WildcardRegexString
    {
        /// <summary>
        /// ͨ���ת����
        /// </summary>
        /// <param name="wildcardStr"></param>
        /// <returns></returns>
        public static string GetWildcardRegexString(string wildcardStr)
        {
            Regex replace = new Regex("[.$^{\\[(|)*+?\\\\]");
            return replace.Replace(wildcardStr,
                       delegate (Match m)
                       {
                           switch (m.Value)
                           {
                               case "?":
                                   return ".?";
                               case "*":
                                   return ".*";
                               default:
                                   return "\\" + m.Value;
                           }
                       }) + "$";
        }
    }
}