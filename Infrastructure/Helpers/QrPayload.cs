using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;

public static class QrPayload
{
    public static async Task<string?> TryDecodeTextAsync(Stream pngStream)
    {
        // Important: Stream might be non-seekable; ImageSharp reads it fine.
        using var image = await Image.LoadAsync<Rgba32>(pngStream);

        var reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>
        {
            AutoRotate = true,
            TryInverted = true,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new[] { BarcodeFormat.QR_CODE }
            }
        };

        var result = reader.Decode(image);
        return string.IsNullOrWhiteSpace(result?.Text) ? null : result!.Text.Trim();
    }

    public static bool IsValidPayload(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return false;

        var fields = ParsePipeColonFormat(payload.Trim());
        if (fields == null) return false;

        // constants
        if (!fields.TryGetValue("K", out var k) || k != "PR") return false;
        if (!fields.TryGetValue("V", out var v) || v != "01") return false;
        if (!fields.TryGetValue("C", out var c) || c != "1") return false;

        // required variable fields
        if (!fields.TryGetValue("R", out var r) || string.IsNullOrWhiteSpace(r)) return false;
        if (!fields.TryGetValue("N", out var n) || string.IsNullOrWhiteSpace(n)) return false;
        if (!fields.TryGetValue("I", out var i) || string.IsNullOrWhiteSpace(i)) return false;
        if (!fields.TryGetValue("SF", out var sf) || string.IsNullOrWhiteSpace(sf)) return false;

        return true;
    }

    private static Dictionary<string, string>? ParsePipeColonFormat(string raw)
    {
        var parts = raw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) return null;

        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var part in parts)
        {
            var idx = part.IndexOf(':');
            if (idx <= 0 || idx == part.Length - 1) return null;

            var key = part.Substring(0, idx).Trim();
            var value = part[(idx + 1)..].Trim();

            if (string.IsNullOrWhiteSpace(key)) return null;
            dict[key] = value;
        }

        return dict.Count == 0 ? null : dict;
    }
}