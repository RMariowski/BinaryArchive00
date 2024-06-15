namespace BinaryArchive00.Extensions;

public static class StringExtensions
{
    public static string Reverse(this string input)
    {
        return string.Create(input.Length, input, (chars, state) =>
        {
            state.AsSpan().CopyTo(chars);
            chars.Reverse();
        });
    }
}
