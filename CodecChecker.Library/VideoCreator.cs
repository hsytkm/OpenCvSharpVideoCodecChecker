using OpenCvSharp;

namespace CodecChecker.Library;

public static class VideoCreator
{
    public static bool TryCreate(IEnumerable<Mat> sourceMats, string filename, FourCC fourCc, out FileInfo? fileInfo)
    {
        VideoWriter? writer = null;
        try
        {
            foreach (var mat in sourceMats)
            {
                writer ??= new VideoWriter(filename, fourCc, 30d, mat.Size());

                if (!writer.IsOpened())
                {
                    fileInfo = null;
                    return false;
                }

                writer.Write(mat);
            }
        }
        finally
        {
            writer?.Dispose();
        }

        fileInfo = new FileInfo(filename);
        return true;
    }
}
