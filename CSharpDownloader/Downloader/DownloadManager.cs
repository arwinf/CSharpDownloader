using CSharpDownloader.Models;
using Microsoft.Extensions.Configuration;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDownloader.Downloader
{
    public class DownloadManager
    {
        private readonly ProgressBarOptions _overallProgressBarOptions = new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.DarkYellow,
            ProgressCharacter = '─',
            ProgressBarOnBottom  = true,
            CollapseWhenFinished = false
        };

        private readonly ProgressBarOptions _allDlcProgressBarOptions = new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.DarkYellow,
            ProgressCharacter = '─',
            ProgressBarOnBottom = true,
            CollapseWhenFinished = false
        };

        private IConfiguration _config;
        private ObservableCollection<DownloadContainer> _dlcCollection;

        public DownloadManager(IConfiguration config)
        {
            _config = config;

            _dlcCollection = new ObservableCollection<DownloadContainer>();
            _dlcCollection.CollectionChanged += DlcCollectionChanged;
        }

        private void InitOrRefreshConsoleUi()
        {
            using (var overallProgressBar = new ProgressBar(100, "Status of all downloads", _overallProgressBarOptions))
            {
                Parallel.For(0, _dlcCollection.Count, (i) =>
                {
                    using (var downloadContainerProgressBar = overallProgressBar.Spawn(100, "__TEST", _allDlcProgressBarOptions))
                    {
                        downloadContainerProgressBar.Tick(1);
                        foreach (var downloadItem in _dlcCollection[i].DownloadItems)

                        {
                            using (var downloadItemProgress = downloadContainerProgressBar.Spawn(100, downloadItem.Url, _allDlcProgressBarOptions))
                            {
                                downloadContainerProgressBar.Tick(20);
                            }
                        }
                    }
                });
            }
        }

        public bool ItemsAvailable => _dlcCollection.Any(dc => dc.DownloadItems.Any(di => !di.Complete));
            
        public void Add(DownloadContainer dlc)
        {
            _dlcCollection.Add(dlc);
        }
            
        public void Start()
        {
            InitOrRefreshConsoleUi();
        }

        public void Stop()
        {

        }

        private void DlcCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InitOrRefreshConsoleUi();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

    }
}
