/// <file>
/// Velentr.Miscellaneous\Sqlite\IModelParser.cs
/// </file>
///
/// <copyright file="IModelParser.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Declares the IModelParser interface.
/// </summary>
using System.Data;

namespace Velentr.Miscellaneous.Sqlite
{
    /// <summary>
    /// Interface for model parser.
    /// </summary>
    public interface IModelParser
    {
        /// <summary>
        /// Parses the given row.
        /// </summary>
        ///
        /// <param name="row">  The row. </param>
        public abstract void Parse(IDataReader row);
    }
}