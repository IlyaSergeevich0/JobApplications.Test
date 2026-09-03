using System.Text;
using System.Text.RegularExpressions;

namespace StringCompressor.Core;

public static class StringCompressor
{
    /// <summary>
    /// Алгоритм компрессии строки, замешающий группы последовательно идущих одинаковых букв формой "sc", где:
    /// "s" - символ;
    /// "c" - количество букв в группе.
    /// </summary>
    /// <param name="input">Исходная строка (только строчные латинские буквы)</param>
    /// <returns>Сжатая строка</returns>
    /// <exception cref="ArgumentNullException">Если входная строка null или пустая</exception>
    /// <exception cref="ArgumentException">Если строка содержит недопустимые символы</exception>
    /// <remarks>Одиночные буквы остаются без числа.</remarks>
    public static string Compress(string? input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input), "Входная строка не может быть null или пустой");

        if (!Regex.IsMatch(input, @"^[a-z]+$"))
            throw new ArgumentException("Строка должна содержать только строчные латинские буквы (a-z)", nameof(input));

        var inputSpan = input.AsSpan();
        var estimatedResultLength = inputSpan.Length / 2;
        var result = new StringBuilder(estimatedResultLength);
        var i = 0;

        while (i < inputSpan.Length)
        {
            var currentChar = inputSpan[i];
            var charCount = 1;

            while (i + charCount < inputSpan.Length && inputSpan[i + charCount] == currentChar)
                charCount++;

            result.Append(currentChar);

            if (charCount > 1)
                result.Append(charCount);

            i += charCount;
        }

        return result.ToString();
    }

    /// <summary>
    /// Восстанавливает исходную строку из сжатой. Подробнее: <seealso cref="Compress(string)"/>    
    /// </summary>
    /// <param name="compressed">Сжатая строка</param>
    /// <returns>Исходная строка</returns>
    /// <exception cref="ArgumentNullException">Если входная строка null или пустая</exception>
    /// <exception cref="ArgumentException">Если строка имеет неверный формат</exception>
    public static string Decompress(string? compressed)
    {
        if (string.IsNullOrEmpty(compressed))
            throw new ArgumentNullException(nameof(compressed), "Входная строка не может быть null");

        var compressedSpan = compressed.AsSpan();
        var estimatedResultLength = compressed.Length * 2;
        var result = new StringBuilder(estimatedResultLength);
        var i = 0;

        while (i < compressed.Length)
        {
            if (!char.IsLower(compressed[i]))
                throw new ArgumentException($"Неверный формат! Ожидалась буква на позиции {i}", nameof(compressed));

            var letter = compressed[i];
            i++;

            var count = 1;
            if (i < compressed.Length && char.IsDigit(compressed[i]))
            {
                var numberStartIndex = i;
                while (i < compressed.Length && char.IsDigit(compressed[i]))
                    i++;

                var numberStr = compressedSpan[numberStartIndex..i];
                if (!int.TryParse(numberStr, out count) || count < 1)
                    throw new ArgumentException($"Неверное число на позиции {numberStartIndex}", nameof(compressed));
            }

            result.Append(letter, count);
        }

        return result.ToString();
    }
}