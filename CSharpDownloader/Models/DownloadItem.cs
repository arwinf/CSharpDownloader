using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpDownloader.Models
{
    public class DownloadItem
    {
        public DownloadItem(string link)
        {
            Url = link;
        }

        public string FileName { get; set; }
        public string Url { get; set; } 
        public decimal Progress { get; set; }
        public bool Complete => this.Progress == (decimal)100;
        public decimal Size { get; set; }

    }
}
