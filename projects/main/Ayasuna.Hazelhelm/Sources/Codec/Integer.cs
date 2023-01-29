namespace Ayasuna.Hazelhelm.Codec;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

/// <summary>
/// Provides methods to convert integers from and to their representation in BASEn
/// </summary>
public static class Integer
{
    /// <summary>
    /// The default BASE2 character set
    /// </summary>
    public const string Base2CharacterSet = "01";

    /// <summary>
    /// The default BASE8 character set
    /// </summary>
    public const string Base8CharacterSet = "01234567";

    /// <summary>
    /// The default BASE10 character set
    /// </summary>
    public const string Base10CharacterSet = "0123456789";

    /// <summary>
    /// The default BASE16 character set
    /// </summary>
    public const string Base16CharacterSet = "0123456789abcdef";

    /// <summary>
    /// The default BASE62 character set
    /// </summary>
    public const string Base62CharacterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via BASE2, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <returns>The integer</returns>
    public static BigInteger FromBase2(string @string)
    {
        return FromBase(@string, Base2CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in BASE2
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>The given <paramref name="integer"/> in its BASE2 representation</returns>
    public static string ToBase2(BigInteger integer)
    {
        return ToBase(integer, Base2CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via BASE8, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <returns>The integer</returns>
    public static BigInteger FromBase8(string @string)
    {
        return FromBase(@string, Base8CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in BASE8
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>The given <paramref name="integer"/> in its BASE8 representation</returns>
    public static string ToBase8(BigInteger integer)
    {
        return ToBase(integer, Base8CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via BASE10, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <returns>The integer</returns>
    public static BigInteger FromBase10(string @string)
    {
        return FromBase(@string, Base10CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in BASE10
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>The given <paramref name="integer"/> in its BASE10 representation</returns>
    public static string ToBase10(BigInteger integer)
    {
        return ToBase(integer, Base10CharacterSet);
    }


    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via BASE16, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <returns>The integer</returns>
    public static BigInteger FromBase16(string @string)
    {
        return FromBase(@string, Base16CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in BASE16
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>The given <paramref name="integer"/> in its BASE16 representation</returns>
    public static string ToBase16(BigInteger integer)
    {
        return ToBase(integer, Base16CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via BASE62, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <returns>The integer</returns>
    public static BigInteger FromBase62(string @string)
    {
        return FromBase(@string, Base62CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in BASE62
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>The given <paramref name="integer"/> in its BASE62 representation</returns>
    public static string ToBase62(BigInteger integer)
    {
        return ToBase(integer, Base62CharacterSet);
    }

    /// <summary>
    /// Converts the given <paramref name="integer"/> to its representation in the base given by the length of the given <paramref name="characterSet"/>
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <param name="characterSet">The character set/the symbols the number system uses</param>
    /// <returns>The given <paramref name="integer"/> in the base given by the length of the given <paramref name="characterSet"/></returns>
    public static string ToBase(BigInteger integer, string characterSet)
    {
        var @base = Convert.ToByte(characterSet.Length);

        var result = new LinkedList<char>();

        if (integer == 0)
        {
            result.AddFirst(characterSet[0]);
        }
        else
        {
            while (integer > 0)
            {
                result.AddFirst(characterSet[(byte)(integer % @base)]);
                integer /= @base;
            }
        }

        return string.Join(string.Empty, result);
    }

    /// <summary>
    /// Converts the given <paramref name="string"/>, which is expected to contain an integer which is represented via the base given by the length of the given <paramref name="characterSet"/>, into a <see cref="BigInteger"/>
    /// </summary>
    /// <param name="string">The string to convert</param>
    /// <param name="characterSet">The character set/the symbols the number system uses</param>
    /// <returns>The given <paramref name="string"/> as integer</returns>
    public static BigInteger FromBase(string @string, string characterSet)
    {
        var @base = Convert.ToByte(characterSet.Length);

        return @string.Select(e => Convert.ToByte(characterSet.TakeWhile(x => x != e).Count())).Aggregate(BigInteger.Zero, (current, index) => current * @base + index);
    }
}