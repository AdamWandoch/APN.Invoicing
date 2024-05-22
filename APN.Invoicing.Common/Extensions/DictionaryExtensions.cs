using System.Text;

namespace APN.Invoicing.Common.Extensions;

public static class DictionaryExtensions
{
    /// <summary>
    /// Tranforms a dictionary to a readable text format.
    /// </summary>
    /// <param name="errors">IDictionary<string, string[]></param>
    /// <returns>String with keys and values in a readable format.</returns>
    //public static string AsString(this IDictionary<string, string[]> errors)
    //{
    //    var stringBuilder = new StringBuilder();

    //    foreach (var error in errors)
    //    {
    //        stringBuilder.Append($"{error.Key}:");
    //        foreach (var message in error.Value)
    //        {
    //            stringBuilder.Append($" - {message}");
    //        }
    //    }

    //    return stringBuilder.ToString();
    //}
}
