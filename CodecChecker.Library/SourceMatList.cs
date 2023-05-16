using System.Collections;
using OpenCvSharp;

namespace CodecChecker.Library;

/// <summary>
/// Prepare source mats for video to be created.
/// </summary>
public sealed record SourceMatList : IReadOnlyList<Mat>, IDisposable
{
    private readonly List<Mat> _mats;

    public Mat this[int index] => _mats[index];
    public int Count => _mats.Count;

    private SourceMatList(string directory)
    {
        _mats = EnumerateSourceMats(directory).ToList();
    }

    public static SourceMatList Create(string directory = ".\\Resources") => new(directory);

    private static IEnumerable<Mat> EnumerateSourceMats(string directory)
    {
        var frameFiles = Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        foreach (var frameFile in frameFiles)
            yield return Cv2.ImRead(frameFile, ImreadModes.Unchanged);
    }

    public IEnumerator<Mat> GetEnumerator() => _mats.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Dispose()
    {
        foreach (var mat in _mats)
            mat.Dispose();
    }
}
