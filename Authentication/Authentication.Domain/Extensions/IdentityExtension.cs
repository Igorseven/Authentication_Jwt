using System.Text;

namespace Authentication.Domain.Extensions;
public static class IdentityExtension
{
    public static string GetAllWritableCharacters(Encoding encoding)
    {
        encoding = Encoding.GetEncoding(
            encoding.WebName, 
            new EncoderExceptionFallback(), 
            new DecoderExceptionFallback());
        
        var builder = new StringBuilder();

        var chars = new char[1];
        var bytes = new byte[16];

        for (var i = 20; i <= char.MaxValue; i++)
        {
            chars[0] = (char)i;
            try
            {
                var count = encoding.GetBytes(chars, 0, 1, bytes, 0);

                if (count != 0)
                {
                    builder.Append(chars[0]);
                }
            }
            catch
            {
                break;
            }
        }
        return builder.ToString();
    }
}
