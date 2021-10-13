using System.Collections;
using System.Collections.Generic;
public static class StringExtensions {
    public static bool Contains(this string source, string toCheck, System.StringComparison comp) {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}