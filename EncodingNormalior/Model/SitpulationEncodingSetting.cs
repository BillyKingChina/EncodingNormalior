using System.Text;

namespace EncodingNormalior.Model
{
    /// <summary>
    ///     �û����õĹ涨����
    /// </summary>
    public class SitpulationEncodingSetting : ISetting
    {
        /// <summary>
        ///     ���û��ȡ�û��涨�ı��룬Ĭ�ϱ���Ϊ <see cref="Encoding.UTF8" />
        /// </summary>
        public Encoding SitpulationEncoding { set; get; } = Encoding.UTF8;
    }
}