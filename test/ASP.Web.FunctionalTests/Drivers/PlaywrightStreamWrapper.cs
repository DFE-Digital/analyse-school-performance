namespace ASP.Web.FunctionalTests.Drivers
{
    public class PlaywrightStreamWrapper : Stream
    {
        private readonly Stream _stream;

        internal PlaywrightStreamWrapper(Stream stream)
        {
            _stream = stream;
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            Task<int> task =
                _stream.ReadAsync(buffer, offset, count);
            task.Wait();

            return task.Result;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => _stream.ReadAsync(buffer, offset, count, cancellationToken);

        public override void Close() => _stream.Close();

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        public override void SetLength(long value) => _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
    }
}
