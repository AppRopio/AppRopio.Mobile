﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Log;
using System.Linq;
using System.Threading;

namespace AppRopio.Base.Droid.Services.Log
{
    public class LogService : ILogService
    {
        #region Fields

        private bool _readStarted;
        private bool _writeStarted;

        private const string FILE_NAME = "Application_Log.txt";

        private Queue<byte[]> _logQueue = new Queue<byte[]>();

        private Task _lastWriteTask;

        private CancellationTokenSource _writeCTS;

        private List<byte> _cachedLogBytes;

        #endregion

        #region Constructor

        public LogService()
        {
            _writeCTS = new CancellationTokenSource();
            _cachedLogBytes = new List<byte>();
        }

        #endregion

        #region Private 

        private Task WriteAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    byte[] data = null;

                    lock (_logQueue)
                    {
                        data = _logQueue.Dequeue();
                    }

                    try
                    {
                        System.Diagnostics.Trace.WriteLine(Encoding.UTF8.GetString(data));
                    }
                    catch { }

                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, FILE_NAME);
                    FileStream fStream;

                    if (!File.Exists(filePath))
                    {
                        fStream = File.Create(filePath);

                        var creationData = Encoding.UTF8.GetBytes($"CreationTime: {DateTime.UtcNow}\n");
                        fStream.Write(creationData, 0, creationData.Length);

                        _cachedLogBytes = new List<byte>();
                    }
                    else
                    {
                        var fileInfo = new FileInfo(filePath);
                        if ((fileInfo.CreationTimeUtc - DateTime.UtcNow).Days > 7 || ((fileInfo.Length + data.Length) / (1024 * 1024)) >= 1)
                        {
                            File.Delete(filePath);

                            fStream = File.Create(filePath);

                            var creationData = Encoding.UTF8.GetBytes($"CreationTime: {DateTime.UtcNow}\n");
                            fStream.Write(creationData, 0, creationData.Length);

                            _cachedLogBytes = new List<byte>();
                        }
                        else
                            fStream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                    }

                    fStream.Write(data, 0, data.Length);

                    fStream.Close();

                    _cachedLogBytes.AddRange(data);

                    if (!_readStarted)
                    {
                        lock (_logQueue)
                        {
                            if (_logQueue.Any())
                                WriteAsync();
                            else
                                _writeStarted = false;
                        }
                    }
                    else
                        _writeStarted = false;
                }
                catch
                { }
            }, _writeCTS.Token);
        }

        #endregion

        #region ILogService implementation

        public byte[] CachedLog()
        {
            return _cachedLogBytes?.ToArray();
        }

        public byte[] Read()
        {
            _readStarted = true;

            _writeCTS.Cancel(false);

            var data = new List<byte>();

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            try
            {
                lock (this)
                {
                    if (Directory.Exists(folderPath))
                    {
                        var filePath = Path.Combine(folderPath, FILE_NAME);

                        if (File.Exists(filePath))
                        {
                            var fStream = File.OpenRead(filePath);

                            var buffer = new byte[4 * 1024];

                            while (fStream.Read(buffer, 0, buffer.Length) > 0)
                            {
                                data.AddRange(buffer);
                            }

                            fStream.Close();

                            File.Delete(filePath);

                            _cachedLogBytes = new List<byte>();
                        }
                    }
                }
            }
            catch
            { }

            _readStarted = false;

            return data.ToArray();
        }

        public void Write(byte[] data)
        {
            try
            {
                lock (_logQueue)
                {
                    _logQueue.Enqueue(data);
                }
            }
            catch { }

            if (!_writeStarted && !_readStarted)
            {
                try
                {
                    WriteAsync();
                }
                catch (OperationCanceledException)
                {

                }
            }
        }

        #endregion
    }
}
