using System;

namespace Model
{
    public class MediaItem
    {
        public string URL { get; set; }
        public MediaFileType FileType { get; set; }
        public MediaFileFormat FileFormat { get; set; }
        public long FileSize { get; set; }
        public TimeSpan Duration { get; set; }
    }
}