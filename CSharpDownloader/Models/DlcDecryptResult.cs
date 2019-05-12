using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpDownloader.Models
{
    public class DlcDecryptResult
    {
        public bool Success { get; set; }
        public DownloadContainer Data { get; set; }

    }
}
