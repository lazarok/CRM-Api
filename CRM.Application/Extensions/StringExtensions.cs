using System.Text.RegularExpressions;

namespace CRM.Application.Extensions;

public static class StringExtensions
{
    public static string GenerateSlug(this string phrase)
    {
        var str = phrase.ToLower();

        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars          
        str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it  
        str = Regex.Replace(str, @"\s", "-"); // hyphens  

        return str;
    }
}