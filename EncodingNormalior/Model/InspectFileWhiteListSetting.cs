using System.Collections.Generic;

namespace EncodingNormalior.Model
{
    /// <summary>
    ///     �ļ�������������
    /// </summary>
    public class InspectFileWhiteListSetting : ISetting
    {
        /// <summary>
        ///     ���û��ȡ������
        /// </summary>
        public List<string> WhiteList { set; get; } = new List<string>();
    }
}