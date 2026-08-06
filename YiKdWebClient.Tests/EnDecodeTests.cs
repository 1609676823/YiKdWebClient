using System.Security.Cryptography;
using System.Text;

namespace YiKdWebClient.Tests;

public class EnDecodeTests
{
    [Fact]
    public void Encode_is_deterministic_and_matches_modern_implementation()
    {
        var first = EnDecode.Encode("hello");
        var second = EnDecode.Encode("hello");

        Assert.Equal(first, second);
        Assert.Equal(EnDecode.EncodeNew1("hello"), first);
        Assert.Equal("hello", DecryptDes(first));
    }

    [Fact]
    public void EncodeNew1_encrypts_utf8_text()
    {
        var encrypted = EnDecode.EncodeNew1("hello-world");

        Assert.Equal("hello-world", DecryptDes(encrypted));
    }

    [Fact]
    public void HmacSHA256_returns_standard_base64_digest()
    {
        const string message = "message";
        const string secret = "secret";
        var expected = Convert.ToBase64String(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(message)));

        Assert.Equal(expected, EnDecode.HmacSHA256(message, secret, Encoding.UTF8));
    }

    [Fact]
    public void HmacSHA256_hex_mode_returns_base64_encoded_lower_hex()
    {
        const string message = "message";
        const string secret = "secret";
        var digest = HMACSHA256.HashData(Encoding.ASCII.GetBytes(secret), Encoding.ASCII.GetBytes(message));
        var lowerHex = Convert.ToHexString(digest).ToLowerInvariant();
        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(lowerHex));

        Assert.Equal(expected, EnDecode.HmacSHA256(message, secret, Encoding.ASCII, true));
    }

    [Fact]
    public void HmacSHA256_treats_null_secret_as_empty()
    {
        var expected = Convert.ToBase64String(
            HMACSHA256.HashData(Array.Empty<byte>(), Encoding.UTF8.GetBytes("message")));

        Assert.Equal(expected, EnDecode.HmacSHA256("message", null!, Encoding.UTF8));
    }

    [Fact]
    public void ByteToHexStr_returns_uppercase_hex()
    {
        Assert.Equal("000AABFF", EnDecode.ByteToHexStr(new byte[] { 0, 10, 171, 255 }));
        Assert.Equal(string.Empty, EnDecode.ByteToHexStr(null!));
    }

    [Fact]
    public void UrlEncodeWithUpperCode_uses_uppercase_percent_sequences()
    {
        Assert.Equal("a+b%2Bc%2F%3F", EnDecode.UrlEncodeWithUpperCode("a b+c/?", Encoding.UTF8));
    }

    private static string DecryptDes(string encrypted)
    {
        var key = Encoding.ASCII.GetBytes("KingdeeK");
        using var des = DES.Create();
        des.Key = key;
        des.IV = key;

        using var input = new MemoryStream(Convert.FromBase64String(encrypted));
        using var crypto = new CryptoStream(input, des.CreateDecryptor(), CryptoStreamMode.Read);
        using var reader = new StreamReader(crypto, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
