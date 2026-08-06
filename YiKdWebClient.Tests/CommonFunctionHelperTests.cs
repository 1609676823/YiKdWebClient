using System.Security.Cryptography;
using System.Text;

namespace YiKdWebClient.Tests;

public class CommonFunctionHelperTests
{
    [Fact]
    public void CurrentTimeMillis_returns_current_unix_milliseconds()
    {
        var before = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var actual = CommonFunctionHelper.CurrentTimeMillis();

        var after = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Assert.InRange(actual, before, after);
    }

    [Fact]
    public void GetTimestamp_returns_current_unix_seconds()
    {
        var before = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var actual = CommonFunctionHelper.GetTimestamp();

        var after = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Assert.InRange(actual, before, after);
    }

    [Fact]
    public void GetServerUrl_normalizes_trailing_slash()
    {
        Assert.Equal("https://example.test/k3cloud/", CommonFunctionHelper.GetServerUrl("https://example.test/k3cloud"));
        Assert.Equal("https://example.test/k3cloud/", CommonFunctionHelper.GetServerUrl("https://example.test/k3cloud/"));
        Assert.Equal(string.Empty, CommonFunctionHelper.GetServerUrl(string.Empty));
        Assert.Equal(string.Empty, CommonFunctionHelper.GetServerUrl("   "));
    }

    [Fact]
    public void Sha256Hex_string_uses_utf8_and_lowercase_hex()
    {
        Assert.Equal(
            "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
            CommonFunctionHelper.Sha256Hex("abc"));
    }

    [Fact]
    public void Sha256Hex_encoding_overload_uses_supplied_encoding()
    {
        var expected = Convert.ToHexString(SHA256.HashData(Encoding.Unicode.GetBytes("abc"))).ToLowerInvariant();

        Assert.Equal(expected, CommonFunctionHelper.Sha256Hex("abc", Encoding.Unicode));
    }

    [Fact]
    public void Sha256Hex_byte_overload_hashes_exact_bytes()
    {
        var bytes = new byte[] { 0, 1, 2, 255 };
        var expected = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

        Assert.Equal(expected, CommonFunctionHelper.Sha256Hex(bytes));
    }

    [Fact]
    public void ToHexString_supports_lowercase_and_uppercase()
    {
        var bytes = new byte[] { 0, 10, 171, 255 };

        Assert.Equal("000aabff", CommonFunctionHelper.ToHexString(bytes));
        Assert.Equal("000AABFF", CommonFunctionHelper.ToHexString(bytes, false));
    }

    [Fact]
    public void ToBase64_encodes_bytes()
    {
        Assert.Equal("AAEC/v8=", new byte[] { 0, 1, 2, 254, 255 }.ToBase64());
    }

    [Fact]
    public void GetSignatureSHA1Util_sorts_then_hashes_values()
    {
        Assert.Equal(
            "a9993e364706816aba3e25717850c26c9cd0d89d",
            CommonFunctionHelper.GetSignatureSHA1Util(new[] { "c", "a", "b" }));
    }

    [Fact]
    public void GetSHA1_sorts_then_hashes_values()
    {
        Assert.Equal(
            "a9993e364706816aba3e25717850c26c9cd0d89d",
            CommonFunctionHelper.GetSHA1(new[] { "b", "c", "a" }));
    }

    [Fact]
    public void GetSHA256_sorts_then_hashes_values()
    {
        Assert.Equal(
            "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
            CommonFunctionHelper.GetSHA256(new[] { "c", "b", "a" }));
    }

    [Fact]
    public void GetSHA256_returns_empty_for_null_or_empty_input()
    {
        Assert.Equal(string.Empty, CommonFunctionHelper.GetSHA256(null!));
        Assert.Equal(string.Empty, CommonFunctionHelper.GetSHA256(Array.Empty<string>()));
    }
}
