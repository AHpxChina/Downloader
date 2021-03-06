﻿using System;
using System.IO;

namespace Downloader
{
    public class DownloadConfiguration : ICloneable
    {
        private int _bufferBlockSize;
        private int _chunkCount;
        private long _maximumBytesPerSecond;

        public DownloadConfiguration()
        {
            MaxTryAgainOnFailover = int.MaxValue; // the maximum number of times to fail.
            ParallelDownload = false; // download parts of file as parallel or not
            ChunkCount = 1; // file parts to download
            Timeout = 1000; // timeout (millisecond) per stream block reader
            OnTheFlyDownload = true; // caching in-memory mode
            BufferBlockSize = 1024; // usually, hosts support max to 8000 bytes
            MaximumBytesPerSecond = ThrottledStream.Infinite; // No-limitation in download speed
            RequestConfiguration = new RequestConfiguration(); // Default requests configuration
            TempDirectory = Path.GetTempPath(); // Default chunks path
            CheckDiskSizeBeforeDownload = true; // check disk size for temp and file path
        }

        /// <summary>
        ///     Stream buffer size which is used for size of blocks
        /// </summary>
        public int BufferBlockSize
        {
            get => (int)Math.Min(MaximumSpeedPerChunk, _bufferBlockSize);
            set => _bufferBlockSize = value;
        }

        /// <summary>
        ///     Check disk available size for download file before starting the download.
        /// </summary>
        public bool CheckDiskSizeBeforeDownload { get; set; }

        /// <summary>
        ///     File chunking parts count
        /// </summary>
        public int ChunkCount
        {
            get => _chunkCount;
            set => _chunkCount = Math.Max(1, value);
        }

        /// <summary>
        ///     The maximum bytes per second that can be transferred through the base stream.
        /// </summary>
        public long MaximumBytesPerSecond
        {
            get => _maximumBytesPerSecond;
            set => _maximumBytesPerSecond = value <= 0 ? long.MaxValue : value;
        }

        /// <summary>
        ///     The maximum bytes per second that can be transferred through the base stream at each chunk downloader.
        ///     This Property is ReadOnly.
        /// </summary>
        public long MaximumSpeedPerChunk => ParallelDownload ? MaximumBytesPerSecond / ChunkCount : MaximumBytesPerSecond;

        /// <summary>
        ///     How many time try again to download on failed
        /// </summary>
        public int MaxTryAgainOnFailover { get; set; }

        /// <summary>
        ///     download file without caching chunks in disk. In the other words,
        ///     all chunks stored in memory.
        /// </summary>
        public bool OnTheFlyDownload { get; set; }

        /// <summary>
        ///     Download file chunks as Parallel or Serial?
        /// </summary>
        public bool ParallelDownload { get; set; }

        /// <summary>
        ///     Custom body of your requests
        /// </summary>
        public RequestConfiguration RequestConfiguration { get; set; }

        /// <summary>
        ///     Chunk files storage path when the OnTheFlyDownload is false.
        /// </summary>
        public string TempDirectory { get; set; }

        /// <summary>
        ///     Chunk files extension, the default value is ".dsc" which is the acronym of "Downloader Service Chunks" file
        /// </summary>
        public string TempFilesExtension { get; set; } = ".dsc";

        /// <summary>
        ///     Download timeout per stream file blocks
        /// </summary>
        public int Timeout { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}