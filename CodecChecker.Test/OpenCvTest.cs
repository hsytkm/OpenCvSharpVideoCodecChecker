using CodecChecker.Library;
using OpenCvSharp;

namespace CodecChecker.Test;

// テスト全体で1回だけ初期化されるらしい
public class ImageProvider : IDisposable
{
    public SourceMatList SourceMats { get; }

    public ImageProvider() => SourceMats = SourceMatList.Create();

    public void Dispose()
    {
        SourceMats.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class OpenCvTest : IClassFixture<ImageProvider>
{
    private readonly ImageProvider _imageProvider;

    public OpenCvTest(ImageProvider imageProvider)
    {
        _imageProvider = imageProvider;
    }

    public static IEnumerable<object[]> GetTestData() => new[] { ".avi", ".mov" }
        .SelectMany(static ext => CvVideoCodecs.EnumerateCvVideoCodec(ext))
        .Select(static x => new object[] { x.WriteFilename, x.FourCc });

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void TryWriteVideoTest(string filename, FourCC fourCc)
    {
        var success = VideoCreator.TryCreate(_imageProvider.SourceMats, filename, fourCc, out FileInfo? fileInfo);

        if (fileInfo?.Exists ?? false)
        {
            File.Delete(fileInfo.FullName);
        }

        Assert.True(success);
    }
}