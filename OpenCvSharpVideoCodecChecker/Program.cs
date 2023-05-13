using System.Text;
using OpenCvSharp;
using OpenCvSharpVideoCodecChecker;

using var mats = new SourceMatList();

var sep = new string('=', 50);
var logs = new StringBuilder();

foreach (var ext in new[] { ".avi", ".mov" })
{
    logs.AppendLine(sep);
    logs.Append(CreateVideos(mats, ext));
}

logs.AppendLine(sep);
Console.WriteLine(logs.ToString());


static string CreateVideos(IEnumerable<Mat> sourceMats, string extension)
{
    var log = new StringBuilder();
    var sep = new string('-', 70);

    foreach (var codec in CvVideoCodecs.EnumerateCvVideoCodec(extension))
    {
        var writeFilename = codec.WriteFilename;

        Console.WriteLine(sep);
        Console.WriteLine($"Start {writeFilename}");

        var success = TryCreateVideFile(sourceMats, writeFilename, codec.FourCc);
        var filesize = new FileInfo(writeFilename).Length;

        Console.WriteLine($"Result -> {success,-5} ({filesize,10} Byte)");
        log.AppendLine($"{codec.Index,3} | {codec.Name,-4} | {extension} -> {success,-5} ({filesize,10} Byte)");
        File.Delete(writeFilename);
    }
    return log.ToString();
}

static bool TryCreateVideFile(IEnumerable<Mat> sourceMats, string filename, FourCC fourCc)
{
    VideoWriter? writer = null;
    try
    {
        foreach (var mat in sourceMats)
        {
            writer ??= new VideoWriter(filename, fourCc, 30d, mat.Size());

            if (!writer.IsOpened())
                return false;

            writer.Write(mat);
        }
    }
    finally
    {
        writer?.Dispose();
    }
    return true;
}
