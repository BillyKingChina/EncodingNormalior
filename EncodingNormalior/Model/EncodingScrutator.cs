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
            var headByte = ReadFileHeadbyte(stream);

            //���ļ���ȡ����
            var encoding = AutoEncoding(headByte);
            stream.Position = 0;

            // uft8��ǩ��
            if (encoding.Equals(Encoding.ASCII))//GBK utf8
            {
                if (IsGBK(stream))
                {
                    encoding = Encoding.GetEncoding("GBK");
                }
            }

            stream.Dispose();
            return encoding;
        }
        /// <summary>
        /// �����ļ��ǲ���GBK����
        /// </summary>
        /// <param name="stream">�ļ�</param>
        /// <returns>true ��GBK���룬false����GBK����</returns>
        private static bool IsGBK(Stream stream)
        {
            long length = 0;
            bool isGBK = false;//������е�byte���ǲ�����127��ô��ascii����ʱ��ʲô����
            var buffer = new byte[1024];
            var n = 0;
            while ((n = stream.Read(buffer, 0, 1024)) > 0)
            {
                for (var i = 0; i < n; i++)
                {
                    var temp = buffer[i];
                    if (temp < 128)
                    {
                        length++;
                        continue;
                    }
                    if (i + 1 == n)
                    {
                        break;
                    }
                    var temp2 = buffer[i + 1];//http://en.wikipedia.org/wiki/GBK
                    if ((temp >= 0xA1 && temp <= 0xA9 && temp2 >= 0xA1 && temp2 <= 0xFE) ||
                        (temp >= 0xB0 && temp <= 0xF7 && temp2 >= 0xA1 && temp2 <= 0xFE) ||
                        (temp >= 0x81 && temp <= 0xA0 && temp2 >= 0x40 && temp2 <= 0xFE && temp2 != 0x7F) ||
                        (temp >= 0xAA && temp <= 0xFE && temp2 >= 0x40 && temp2 <= 0xA0 && temp2 != 0x7F) ||
                        (temp >= 0xA8 && temp <= 0xA9 && temp2 >= 0x40 && temp2 <= 0xA0 && temp2 != 0x7F) ||
                        (temp >= 0xAA && temp <= 0xAF && temp2 >= 0xA1 && temp2 <= 0xFE) ||
                        (temp >= 0xF8 && temp <= 0xFE && temp2 >= 0xA1 && temp2 <= 0xFE) ||
                        (temp >= 0xA1 && temp <= 0xA7 && temp2 >= 0x40 && temp2 <= 0xA0 && temp2 != 0x7F))
                    {
                        length += 2;
                        i++;
                        isGBK = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            stream.Position = 0;
            if (!isGBK)//���û�����Ļ�GBK����ascii���ַ����ж�ASCII
            {
                return false;
            }
            return length == stream.Length;
        }

        /// <summary>
        ///     ��ȡ�ļ���ͷ4��byte
        /// </summary>
        /// <param name="stream">�ļ���</param>
        /// <param name="headAmount">��ȡ����</param>
        /// <returns>�ļ�ͷ4��byte</returns>
        private byte[] ReadFileHeadbyte(Stream stream, int headAmount = 4)
        {
            //var headAmount = 4;
            var buffer = new byte[headAmount];
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


            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
                return Encoding.UTF7; //85 116 102 55    //utf7 aa 97 97 0 0
            //utf7 ���� = 43 102 120 90

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                return Encoding.UTF8; //��ǩ�� 117 116 102 56
            // 130 151 160 231
            if (bom[0] == 0xff && bom[1] == 0xfe)
                return Encoding.Unicode; //UTF-16LE

            if (bom[0] == 0xfe && bom[1] == 0xff)
                return Encoding.BigEndianUnicode; //UTF-16BE

            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

            return Encoding.ASCII; //�������ASCII������GBK
        }
    }
}