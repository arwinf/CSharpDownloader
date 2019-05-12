using CSharpDownloader.Dlc;
using CSharpDownloader.Downloader;
using CSharpDownloader.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace CSharpDownloader
{
    class Program
    {
        private static IConfiguration _config { get; set; }
        private static DownloadManager _downloadManager { get; set; }

        static void Main(string[] args)
        {
            _config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            _downloadManager = new DownloadManager(_config);
            
            FileSystemWatcher fsWatcher = new FileSystemWatcher(_config["DlcFolder"], "*.dlc");

            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.Created += (object sender, FileSystemEventArgs e) => 
            {
                var result = DlcProcessor.Handle(e.FullPath).Result;

                if (result.Success)
                    _downloadManager.Add(result.Data);
            };

            while (true)
            {
            }

        }
    }
}
