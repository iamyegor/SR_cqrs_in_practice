using System.Text;

namespace DbSynchronization.Extensions;

public static class StringExtensions
{
    public static string? ToSnakeCase(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        StringBuilder stringBuilder = new StringBuilder();
        char[] characters = input.ToCharArray();

        stringBuilder.Append(char.ToLower(characters[0]));

        for (int i = 1; i < characters.Length; i++)
        {
            if (char.IsUpper(characters[i]))
            {
                stringBuilder.Append('_');
                stringBuilder.Append(char.ToLower(characters[i]));
            }
            else
            {
                stringBuilder.Append(characters[i]);
            }
        }

        return stringBuilder.ToString();
    }
}
