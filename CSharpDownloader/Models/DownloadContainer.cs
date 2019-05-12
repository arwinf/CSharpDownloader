using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDownloader.Models
{
    public class DownloadContainer 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Links
        {
            set
            {
                if (DownloadItems == null)
                    DownloadItems = new List<DownloadItem>();

                foreach (var item in value)
                    DownloadItems.Add(new DownloadItem(item));
            }
        }

        public decimal Size => DownloadItems.Sum(di => di.Size);
        public List<DownloadItem> DownloadItems { get; set; }
    }
}
