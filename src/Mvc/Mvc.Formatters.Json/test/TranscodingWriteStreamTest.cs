using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Formatters.Json
{
    public class TranscodingWriteStreamTest
    {
        public static TheoryData WriteAsyncInputLatin =>
            new TheoryData<string>
            {
                "Hello world",
                new string('A', count: 4096),
                new string('A', count: 18000),
                new string('Æ', count: 2854),
               "pingüino",
            };

        [Theory]
        [MemberData(nameof(WriteAsyncInputLatin))]
        public Task WriteAsync_WorksForReadStream_WhenInputIs_Unicode(string message)
        {
            var x = "ஐ";
            var bytes = JsonSerializer.ToBytes(x, typeof(string));
            System.Console.WriteLine(bytes);

            var targetEncoding = Encoding.Unicode;
            return WriteAsyncTest(targetEncoding, message);
        }

        [Theory]
        [MemberData(nameof(WriteAsyncInputLatin))]
        public Task WriteAsync_WorksForReadStream_WhenInputIs_UTF7(string message)
        {
            var targetEncoding = Encoding.UTF7;
            return WriteAsyncTest(targetEncoding, message);
        }

        [Theory]
        [MemberData(nameof(WriteAsyncInputLatin))]
        public Task WriteAsync_WorksForReadStream_WhenInputIs_WesternEuropeanEncoding(string message)
        {
            // Arrange
            var targetEncoding = Encoding.GetEncoding(28591);
            return WriteAsyncTest(targetEncoding, message);
        }

        private static async Task WriteAsyncTest(Encoding targetEncoding, string message)
        {
            var input = $"{{\"Message\":\"{message}\"}}";
            var expected = targetEncoding.GetBytes(input);

            var model = new TestModel { Message = message };
            var stream = new MemoryStream();

            var transcodingStream = new TranscodingWriteStream(stream, Encoding.UTF8, targetEncoding);
            await JsonSerializer.WriteAsync(model, model.GetType(), transcodingStream);
            await transcodingStream.FlushAsync();

            Assert.Equal(expected, stream.ToArray());
        }

        private class TestModel
        {
            public string Message { get; set; }

        }
    }
}
