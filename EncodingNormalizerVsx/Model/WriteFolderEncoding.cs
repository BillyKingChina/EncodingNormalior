using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingNormalizerVsx.Model
{
    public class WriteFolderEncoding
    {
        /// <summary>
        ///     �����е��ļ�д����淶
        /// </summary>
        public void WriteSitpulationEncoding(IEnumerable<IEncodingScrutatorFile> encodingScrutatorFolder,
            EncodingScrutatorProgress progress, Encoding encoding)
        {
            foreach (var temp in encodingScrutatorFolder.Where(temp => temp.Check))
            {
                if (temp is Model.EncodingScrutatorFile)
                {
                    var file = (Model.EncodingScrutatorFile)temp;
                    progress.ReportWriteSitpulationFile(file);
                    try
                    {
                        file.WriteSitpulationEncoding(encoding);
                        file.Check = false;
                    }
                    catch (Exception e)
                    {
                        progress.ReportExcept(e);
                    }
                }
                else if (temp is Model.EncodingScrutatorFolder)
                {
                    WriteSitpulationEncoding(((Model.EncodingScrutatorFolder)temp).Folder, progress, encoding);
                }
            }
        }
    }
}