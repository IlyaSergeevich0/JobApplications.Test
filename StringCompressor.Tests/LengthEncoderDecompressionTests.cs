using NUnit.Framework;
using StringCompressor.Core;

namespace StringCompressor.Tests;

[TestFixture]
public class LengthEncoderDecompressionTests
{
    [Test]
    public void Decompress_ExampleFromTask_ReturnsOriginalString()
    {
        // Arrange
        string compressed = "a3b2c3d2e";

        // Act
        string result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("aaabbcccdde", result);
    }

    [Test]
    public void Decompress_OnlyLetters_ReturnsSameString()
    {
        // Arrange
        var compressed = "abcdef";

        // Act
        var result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("abcdef", result);
    }

    [Test]
    public void Decompress_SingleCharWithNumber_ReturnsRepeatedChar()
    {
        // Arrange
        var compressed = "a5";

        // Act
        var result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("aaaaa", result);
    }

    [Test]
    public void Decompress_SingleCharWithoutNumber_ReturnsSameChar()
    {
        // Arrange
        var compressed = "z";

        // Act
        var result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("z", result);
    }

    [Test]
    public void Decompress_MultipleGroups_ReturnsCorrect()
    {
        // Arrange
        var compressed = "a2b3c4d5";

        // Act
        var result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("aabbbccccddddd", result);
    }

    [Test]
    public void Decompress_MultiDigitNumber_ReturnsCorrect()
    {
        // Arrange
        var compressed = "a12b3";

        // Act
        var result = LengthEncoder.Decompress(compressed);

        // Assert
        Assert.AreEqual("aaaaaaaaaaaabbb", result);
    }

    [Test]
    public void Decompress_EmptyInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LengthEncoder.Decompress(string.Empty));
    }

    [Test]
    public void Decompress_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LengthEncoder.Decompress(null));
    }

    [Test]
    public void Decompress_InvalidFormat_StartsWithDigit_ThrowsArgumentException()
    {
        var compressed = "1a2b";

        Assert.Throws<ArgumentException>(() => LengthEncoder.Decompress(compressed));
    }

    [Test]
    public void Decompress_InvalidFormat_ZeroCount_ThrowsArgumentException()
    {
        var compressed = "a0b";

        Assert.Throws<ArgumentException>(() => LengthEncoder.Decompress(compressed));
    }
}
