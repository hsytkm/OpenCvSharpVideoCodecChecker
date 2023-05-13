using System.Collections;
using OpenCvSharp;

namespace OpenCvSharpVideoCodecChecker;

/// <summary>
/// Prepare source mats for video to be created.
/// </summary>
internal sealed record SourceMatList : IReadOnlyList<Mat>, IDisposable
{
    private readonly IReadOnlyList<Mat> _mats;

    public Mat this[int index] => _mats[index];
    public int Count => _mats.Count;

    public SourceMatList(string directory = ".\\Resources")
    {
        _mats = EnumerateSourceMats(directory).ToArray();
    }

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
