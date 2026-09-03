using NUnit.Framework;
using StringCompressor.Core;

namespace StringCompressor.Tests;

[TestFixture]
public class LengthEncoderCompressionTests
{
    [Test]
    public void Compress_ExampleFromTask_ReturnsCorrectString()
    {
        // Arrange
        var input = "aaabbcccdde";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("a3b2c3d2e", result);
    }

    [Test]
    public void Compress_AllUniqueChars_ReturnsSameString()
    {
        // Arrange
        var input = "abcdef";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("abcdef", result);
    }

    [Test]
    public void Compress_AllSameChars_ReturnsLetterWithCount()
    {
        // Arrange
        var input = "aaaaa";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("a5", result);
    }

    [Test]
    public void Compress_SingleChar_ReturnsSameChar()
    {
        // Arrange
        var input = "z";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("z", result);
    }

    [Test]
    public void Compress_MixedGroups_ReturnsCorrect()
    {
        // Arrange
        var input = "aabbbccccddddd";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("a2b3c4d5", result);
    }

    [Test]
    public void Compress_StringWithSingleAndRepeatedGroups_ReturnsCorrect()
    {
        // Arrange
        var input = "abbcccdddd";

        // Act
        var result = LengthEncoder.Compress(input);

        // Assert
        Assert.AreEqual("ab2c3d4", result);
    }

    [Test]
    public void Compress_EmptyInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LengthEncoder.Compress(string.Empty));
    }

    [Test]
    public void Compress_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LengthEncoder.Compress(null));
    }

    [Test]
    public void Compress_InvalidCharacters_ThrowsArgumentException()
    {
        var input = "abc123";

        Assert.Throws<ArgumentException>(() => LengthEncoder.Compress(input));
    }

    [Test]
    public void Compress_UppercaseLetters_ThrowsArgumentException()
    {
        var input = "AbC";

        Assert.Throws<ArgumentException>(() => LengthEncoder.Compress(input));
    }
}
