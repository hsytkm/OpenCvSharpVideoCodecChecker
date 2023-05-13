using System.Reflection;
using OpenCvSharp;

namespace OpenCvSharpVideoCodecChecker;

/// <summary>
/// Get all candidates for Video Cordecs.
/// </summary>
internal static class CvVideoCodecs
{
    internal static IEnumerable<CvVideoCodec> EnumerateCvVideoCodec(string extension)
    {
        static IEnumerable<string> enumerateAllFourCCNames()
        {
            foreach (FieldInfo field in typeof(FourCC).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.IsInitOnly)
                    yield return field.Name;
            }
        }

        int index = 0;
        foreach (var fourCcNames in enumerateAllFourCCNames())
        {
            // 拡張子がなければ生成に失敗します。
            // コーデックが見つからない場合、拡張子から別のコーデックに差し替える動作のようです。
            // 拡張子指定（"avi", "mov"）で同じコーデック指定時でも動作結果が変化します。ムズイ。
            var writeFilename = $"{index++:D3}_{fourCcNames}{extension}";

            yield return new CvVideoCodec(fourCcNames, writeFilename);
        }
    }
}

internal sealed record CvVideoCodec(string Name, FourCC FourCc, string WriteFilename)
{
    public CvVideoCodec(string codecName, string writeFilename)
        : this(codecName, FourCC.FromString(codecName), writeFilename)
    { }
}
