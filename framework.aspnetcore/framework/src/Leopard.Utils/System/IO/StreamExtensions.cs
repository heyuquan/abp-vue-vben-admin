using Leopard.Utils;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static int TryReadAll(this Stream stream, byte[] buffer, int offset, int count)
        {
            return TryReadAll(stream, buffer.AsSpan(offset, count));
        }

        public static int TryReadAll(this Stream stream, Span<byte> buffer)
        {
            var totalRead = 0;
            while (!buffer.IsEmpty)
            {
                var read = stream.Read(buffer);
                if (read == 0)
                    return totalRead;

                totalRead += read;
                buffer = buffer[read..];
            }

            return totalRead;
        }

        public static Task<int> TryReadAllAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return TryReadAllAsync(stream, buffer.AsMemory(offset, count), cancellationToken);
        }

        public static async Task<int> TryReadAllAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var totalRead = 0;
            while (!buffer.IsEmpty)
            {
                var read = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
                if (read == 0)
                    return totalRead;

                totalRead += read;
                buffer = buffer[read..];
            }

            return totalRead;
        }

        public static byte[] ReadToEnd(this Stream stream)
        {
            if (stream.CanSeek)
            {
                var length = stream.Length - stream.Position;
                if (length == 0)
                    return Array.Empty<byte>();

                var buffer = new byte[length];
                var actualLength = TryReadAll(stream, buffer, 0, buffer.Length);
                Array.Resize(ref buffer, actualLength);
                return buffer;
            }

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ReadToEndAsync(this Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream.CanSeek)
            {
                var length = stream.Length - stream.Position;
                if (length == 0)
                    return Array.Empty<byte>();

                var buffer = new byte[length];
                var actualLength = await TryReadAllAsync(stream, buffer, cancellationToken).ConfigureAwait(false);
                Array.Resize(ref buffer, actualLength);
                return buffer;
            }

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            return memoryStream.ToArray();
        }

        public static MemoryStream WriteString(this MemoryStream stream, string value, bool seekToBegin = true)
        {
            Checked.NotNull(stream, nameof(stream));
            Checked.NotNull(value, nameof(value));

            using var writer = new StreamWriter(stream, Encoding.Unicode, 1024, true);

            writer.Write(value);
            writer.Flush();

            if (seekToBegin)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return stream;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StreamReader ToStreamReader(this Stream stream, bool leaveOpen)
        {
            return new StreamReader(stream, Encoding.UTF8, true, 0x400, leaveOpen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StreamReader ToStreamReader(this Stream stream, Encoding encoding, bool detectEncoding, int bufferSize, bool leaveOpen)
        {
            return new StreamReader(stream, encoding, detectEncoding, bufferSize, leaveOpen);
        }

        public static bool ContentsEqual(this Stream src, Stream other, bool? forceLengthCompare = null)
        {
            Checked.NotNull(src, nameof(src));
            Checked.NotNull(other, nameof(other));

            if (src == other)
            {
                // This is not merely an optimization, as incrementing one stream's position
                // should not affect the position of the other.
                return true;
            }

            // This is not 100% correct, as a stream can be non-seekable but still have a known
            // length (but hopefully the opposite can never happen). I don't know how to check
            // if the length is available without throwing an exception if it's not.
            if ((!forceLengthCompare.HasValue && src.CanSeek && other.CanSeek) || (forceLengthCompare == true))
            {
                if (src.Length != other.Length)
                {
                    return false;
                }
            }

            const int intSize = sizeof(long);
            const int bufferSize = 1024 * intSize; // 2048;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                int len1 = src.Read(buffer1, 0, bufferSize);
                int len2 = other.Read(buffer2, 0, bufferSize);

                if (len1 != len2)
                    return false;

                if (len1 == 0)
                    return true;

                int iterations = (int)Math.Ceiling((double)len1 / sizeof(long));

                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * intSize) != BitConverter.ToInt64(buffer2, i * intSize))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static async Task<bool> ContentsEqualAsync(this Stream src, Stream other, bool? forceLengthCompare = null)
        {
            Checked.NotNull(src, nameof(src));
            Checked.NotNull(other, nameof(other));

            if (src == other)
            {
                // This is not merely an optimization, as incrementing one stream's position
                // should not affect the position of the other.
                return true;
            }

            // This is not 100% correct, as a stream can be non-seekable but still have a known
            // length (but hopefully the opposite can never happen). I don't know how to check
            // if the length is available without throwing an exception if it's not.
            if ((!forceLengthCompare.HasValue && src.CanSeek && other.CanSeek) || (forceLengthCompare == true))
            {
                if (src.Length != other.Length)
                {
                    return false;
                }
            }

            const int intSize = sizeof(long);
            const int bufferSize = 1024 * intSize; // 2048;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                int len1 = await src.ReadAsync(buffer1.AsMemory(0, bufferSize));
                int len2 = await other.ReadAsync(buffer2.AsMemory(0, bufferSize));

                if (len1 != len2)
                    return false;

                if (len1 == 0)
                    return true;

                int iterations = (int)Math.Ceiling((double)len1 / sizeof(long));

                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * intSize) != BitConverter.ToInt64(buffer2, i * intSize))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
