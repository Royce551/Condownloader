using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condownloader.Configuration
{
    public class ConfigurationFile
    {
        public string DownloadURLS { get; set; } = string.Empty;
        public string DownloadFileName { get; set; } = string.Empty;
        public bool DownloadAudioOnly { get; set; } = false;
        public string ConvertFormat { get; set; } = ".mp4";
    }
}
