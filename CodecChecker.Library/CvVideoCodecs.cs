using System.Reflection;
using OpenCvSharp;

namespace CodecChecker.Library;

/// <summary>
/// Get all candidates for Video Cordecs.
/// </summary>
public static class CvVideoCodecs
{
    public static IEnumerable<CvVideoCodec> EnumerateCvVideoCodec(string extension)
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
            yield return new CvVideoCodec(fourCcNames, index++, extension);
    }
}

public sealed record CvVideoCodec(
    string Name, FourCC FourCc, string WriteFilename, int Index)
{
    public CvVideoCodec(string codecName, int index, string extension)
        : this(codecName, FourCC.FromString(codecName), GetFilename(codecName, index, extension), index)
    { }

    private static string GetFilename(string codecName, int index, string extension)
    {
        // 拡張子がなければ生成に失敗します。
        // コーデックが見つからない場合、拡張子から別のコーデックに差し替える動作のようです。
        // 拡張子指定（"avi", "mov"）で同じコーデック指定時でも動作結果が変化します。ムズイ。
        return $"{index++:D3}_{codecName}{extension}";
    }
}
