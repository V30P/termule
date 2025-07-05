using System.Text;
namespace Termule;

public class Logger
{
    readonly List<LogStream> streams = [];

    public LogStream GetStream()
    {
        LogStream stream = new LogStream(this);
        streams.Add(stream);

        return stream;
    }

    public void Log(object message, Component source = null)
    {
        string logMessage = $"{(source != null ? $"{source.path}: " : "")}{message}\n";
        foreach (Stream stream in streams)
        {
            stream.Write(Encoding.Default.GetBytes(logMessage));
        }
    }

    public class LogStream(Logger logger) : Stream
    {
        readonly MemoryStream stream = new MemoryStream();

        public override bool CanRead => stream.CanRead;
        public override bool CanSeek => stream.CanSeek;
        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;
        public override long Position { get => stream.Position; set => stream.Position = value; }

        public override void Flush() => stream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => stream.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => stream.Seek(offset, origin);
        public override void SetLength(long value) => stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => stream.Write(buffer, offset, count);

        protected override void Dispose(bool _)
        {
            logger.streams.Remove(this);
            stream.Dispose();
        }
    }
}