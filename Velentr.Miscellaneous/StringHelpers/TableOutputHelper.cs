/// <file>
/// Velentr.Miscellaneous\StringHelpers\TableOutputHelper.cs
/// </file>
///
/// <copyright file="TableOutputHelper.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the table output helper class.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Velentr.Miscellaneous.StringHelpers
{
    /// <summary>
    /// A table output helper.
    /// </summary>
    public static class TableOutputHelper
    {
        /// <summary>
        /// Converts this object to a table.
        /// </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="columns">          The columns. </param>
        /// <param name="rows">             The rows. </param>
        /// <param name="maxColumnSize">    (Optional) The maximum size of the column. </param>
        /// <param name="addSeparatorText"> (Optional) True to add separator text. </param>
        ///
        /// <returns>
        /// The given data converted to a table.
        /// </returns>
        public static string ConvertToTable(List<string> columns, List<List<string>> rows, int maxColumnSize = int.MaxValue, bool addSeparatorText = true)
        {
            // argument checking
            if (maxColumnSize < 1)
            {
                throw new Exception("Column size must be at least 1!");
            }

            var invalidRows = rows.Select(x => x.Count).Where(x => x != columns.Count).ToList();
            if (invalidRows.Count > 0)
            {
                throw new Exception(
                    "There must be the same number of columns in each row, and headers for those columns!");
            }

            // first we need to figure out the max size of each column...
            var maxLengthMapping = new List<int>();
            var separators = new List<string>();
            for (var i = 0; i < columns.Count; i++)
            {
                var column = rows.Select(x => x[i]).ToList();
                var columnMax = column.Max(x => x.Length);
                if (columnMax > maxColumnSize)
                {
                    columnMax = maxColumnSize;
                }
                maxLengthMapping.Add(Math.Max(columnMax, columns[i].Length));
                separators.Add("-");
            }

            var str = new StringBuilder();
            var line = new StringBuilder();

            // Next, create the headers...
            str.Append(GetRowText(columns, maxLengthMapping));

            // Create the separators...
            if (addSeparatorText)
            {
                str.Append(GetRowText(separators, maxLengthMapping, '-'));
            }

            // Add the rows...
            for (var i = 0; i < rows.Count; i++)
            {
                str.Append(GetRowText(rows[i], maxLengthMapping));
            }

            return str.ToString();
        }

        /// <summary>
        /// Converts this object to a table.
        /// </summary>
        ///
        /// <param name="columns">          The columns. </param>
        /// <param name="rows">             The rows. </param>
        /// <param name="maxColumnSize">    (Optional) The maximum size of the column. </param>
        /// <param name="addSeparatorText"> (Optional) True to add separator text. </param>
        ///
        /// <returns>
        /// The given data converted to a table.
        /// </returns>
        public static string ConvertToTable(List<string> columns, List<List<object>> rows, int maxColumnSize = int.MaxValue, bool addSeparatorText = true)
        {
            // convert the rows into a list of strings...
            var actualRows = rows.Select(x => x.Select(y => y.ToString()).ToList()).ToList();

            return ConvertToTable(columns, actualRows, maxColumnSize, addSeparatorText);
        }

        /// <summary>
        /// Gets row text.
        /// </summary>
        ///
        /// <param name="text">             The text. </param>
        /// <param name="maxColumnSize">    The maximum size of the column. </param>
        /// <param name="padCharacter">     (Optional) The pad character. </param>
        ///
        /// <returns>
        /// The row text.
        /// </returns>
        private static string GetRowText(List<string> text, List<int> maxColumnSize, char padCharacter = ' ')
        {
            var line = new StringBuilder();
            var rowText = GetRowSplitListText(text, maxColumnSize);
            var maxNeededLines = rowText.Max(x => x.Count);

            for (var i = 0; i < maxNeededLines; i++)
            {
                var linePart = new StringBuilder();
                linePart.Append($"{rowText[0][i].PadRight(maxColumnSize[0], padCharacter)}");
                for (var j = 1; j < rowText.Count; j++)
                {
                    if (rowText[j].Count > i)
                    {
                        linePart.Append($" | {rowText[j][i].PadRight(maxColumnSize[j], padCharacter)}");
                    }
                    else
                    {
                        linePart.Append(" ".PadRight(maxColumnSize[j], padCharacter));
                    }
                }

                line.AppendLine(linePart.ToString());
            }

            return line.ToString();
        }

        /// <summary>
        /// Gets row split list text.
        /// </summary>
        ///
        /// <param name="row">              The row. </param>
        /// <param name="maxColumnSize">    The maximum size of the column. </param>
        ///
        /// <returns>
        /// The row split list text.
        /// </returns>
        private static List<List<string>> GetRowSplitListText(List<string> row, List<int> maxColumnSize)
        {
            var output = new List<List<string>>();
            // split each column by the max column size...
            for (var i = 0; i < row.Count; i++)
            {
                output.Add(StringSplitters.SplitStringByChunkSize(row[i], maxColumnSize[i]));
            }

            return output;
        }
    }
}