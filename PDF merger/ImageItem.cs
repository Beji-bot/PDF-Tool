using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_merger
{
    public class ImageItem
    {
        public string FilePath { get; set; }

        public string FileName => Path.GetFileName(FilePath);

        public string FileExtension => Path.GetExtension(FilePath).TrimStart('.').ToUpperInvariant();
    }
}
