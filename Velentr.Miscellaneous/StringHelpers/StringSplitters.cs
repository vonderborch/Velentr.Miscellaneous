/// <file>
/// Velentr.Miscellaneous\StringHelpers\StringSplitters.cs
/// </file>
///
/// <copyright file="StringSplitters.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the string splitters class.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;

namespace Velentr.Miscellaneous.StringHelpers
{
    /// <summary>
    /// A string splitters.
    /// </summary>
    public static class StringSplitters
    {
        /// <summary>
        /// The new line characters.
        /// </summary>
        private static string[] _newLineChars = new string[] { "\r\n", "\r", "\n" };

        /// <summary>
        /// Splits string by new lines.
        /// </summary>
        ///
        /// <param name="str">  The string. </param>
        ///
        /// <returns>
        /// A List&lt;string&gt;
        /// </returns>
        public static List<string> SplitStringByNewLines(string str)
        {
            return str.Split(_newLineChars, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// Splits string by chunk size.
        /// </summary>
        ///
        /// <param name="str">          The string. </param>
        /// <param name="maxChunkSize"> The maximum size of the chunk. </param>
        ///
        /// <returns>
        /// A List&lt;string&gt;
        /// </returns>
        public static List<string> SplitStringByChunkSize(string str, int maxChunkSize)
        {
            var output = new List<string>();
            for (var i = 0; i < str.Length; i += maxChunkSize)
            {
                output.Add(str.Substring(i, Math.Min(maxChunkSize, str.Length - i)));
            }

            return output;
        }
    }
}