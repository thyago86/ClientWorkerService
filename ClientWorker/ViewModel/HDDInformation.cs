using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClientWorker.ViewModel
{
    public class HDDInformation
    {
        public string Name { get; set; }
        public DriveType DriveType { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long TotalSize { get; set; }
    }
}
