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
            Regex replace = //new Regex("[.$^{\\[(|)*+?\\\\]");
                _regex;
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


        /// <summary>
        /// ��ȡͨ���������
        /// </summary>
        /// <param name="wildcarStr"></param>
        /// <param name="ignoreCase">�Ƿ���Դ�Сд</param>
        /// <returns></returns>
        public static Regex GetWildcardRegex(string wildcarStr, bool ignoreCase)
        {
            //if (ignoreCase)
            //{
            //    return new Regex(GetWildcardRegexString(wildcarStr));
            //}
            //return new Regex(GetWildcardRegexString(wildcarStr), RegexOptions.IgnoreCase);

            return new Regex(GetWildcardRegexString(wildcarStr), ignoreCase? RegexOptions.IgnoreCase:RegexOptions.None);
        }


        private static Regex _regex = new Regex("[.$^{\\[(|)*+?\\\\]", RegexOptions.Compiled);
    }
}