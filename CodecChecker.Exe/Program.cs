using CodecChecker.Library;
using OpenCvSharp;
using System.Text;

using var mats = SourceMatList.Create();

var sep = new string('=', 50);
var logs = new StringBuilder();

foreach (var ext in new[] { ".avi", ".mov" })
{
    logs.AppendLine(sep);
    logs.Append(Create(mats, ext));
}

logs.AppendLine(sep);
Console.WriteLine(logs.ToString());


static string Create(IEnumerable<Mat> sourceMats, string extension, bool isDeleteVideo = true)
{
    var log = new StringBuilder();
    var sep = new string('-', 70);

    foreach (var codec in CvVideoCodecs.EnumerateCvVideoCodec(extension))
    {
        var writeFilename = codec.WriteFilename;

        Console.WriteLine(sep);
        Console.WriteLine($"Start {writeFilename}");

        var success = VideoCreator.TryCreate(sourceMats, writeFilename, codec.FourCc, out FileInfo? fileInfo);
        var fileSize = fileInfo?.Length ?? 0;

        Console.WriteLine($"Result -> {success,-5} ({fileSize,10} Byte)");
        log.AppendLine($"{codec.Index,3} | {codec.Name,-4} | {extension} -> {success,-5} ({fileSize,10} Byte)");

        if (isDeleteVideo && (fileInfo?.Exists ?? false))
        {
            File.Delete(fileInfo.FullName);
        }
    }
    return log.ToString();
}
