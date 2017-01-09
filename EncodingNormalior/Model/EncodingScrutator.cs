using System;
using System.IO;
using System.Text;

namespace EncodingNormalior.Model
{
    /// <summary>
    ///     ������
    /// </summary>
    public class EncodingScrutator
    {
        /// <summary>
        ///     ����ļ�����
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <returns>�ļ�����</returns>
        public Encoding InspectFileEncoding(FileInfo file)
        {
            //����
            Stream stream = file.OpenRead();
            byte[] headByte = ReadFileHeadbyte(stream);

            //���ļ���ȡ����
            Encoding encoding= AutoEncoding(headByte);

            //gbk utf7 uft8��ǩ��
            if (encoding.Equals(Encoding.ASCII))
            {
                if (IsGBK(stream))
                {
                    return Encoding.GetEncoding("GBK");
                }
            }


            stream.Dispose();
            //return Encoding.Default;
            return encoding;
        }

        private bool IsGBK(Stream stream)
        {
            return true;
        }

        /// <summary>
        /// ��ȡ�ļ���ͷ4��byte
        /// </summary>
        /// <param name="stream">�ļ���</param>
        /// <returns>�ļ�ͷ4��byte</returns>
        private byte[] ReadFileHeadbyte(Stream stream)
        {
            int headAmount = 4;
            byte[] buffer = new byte[headAmount];
            stream.Read(buffer, 0, headAmount);
            stream.Position = 0;
            return buffer;
        }



        private static Encoding AutoEncoding(byte[] bom)
        {
            if (bom.Length != 4)
            {
                throw new ArgumentException("EncodingScrutator.AutoEncoding ������С������4");
            }

            // Analyze the BOM

            //gbk 71 66 75 32
            //gbk ���� 177 224 194 235
            //gbk aa  97 97 0 0
            //gbk aa�� 97 97 206 196
            //gbk aa���� 97 97 206 196 200 253

            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
                return Encoding.UTF7;//85 116 102 55    //utf7 aa 97 97 0 0
            //utf7 ���� = 43 102 120 90

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                return Encoding.UTF8;//��ǩ�� 117 116 102 56
                                          // 130 151 160 231
            if (bom[0] == 0xff && bom[1] == 0xfe)
                return Encoding.Unicode; //UTF-16LE

            if (bom[0] == 0xfe && bom[1] == 0xff)
                return Encoding.BigEndianUnicode; //UTF-16BE

            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

            return Encoding.ASCII;//ascii 116 104 105 115
        }
    }
}