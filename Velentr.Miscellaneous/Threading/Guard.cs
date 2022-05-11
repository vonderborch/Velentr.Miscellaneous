/// <file>
/// Velentr.Miscellaneous\Threading\Guard.cs
/// </file>
///
/// <copyright file="Guard.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the Guard class.
/// </summary>

using System.Diagnostics;
using System.Threading;

namespace Velentr.Miscellaneous.Threading
{
    /// <summary>
    /// A thread-safe boolean guard/flag.
    /// </summary>
    [DebuggerDisplay("State = {state == TRUE}")]
    public class Guard
    {
        /// <summary>
        /// The value for false
        /// </summary>
        private const int False = 0;

        /// <summary>
        /// The value for true
        /// </summary>
        private const int True = 1;

        /// <summary>
        /// The current state
        /// </summary>
        private int _state = False;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Guard"/> is check.
        /// </summary>
        /// <value><c>true</c> if check; otherwise, <c>false</c>.</value>
        public bool Check => _state == True;

        /// <summary>
        /// Gets a value indicating whether [check set].
        /// </summary>
        /// <value><c>true</c> if [check set]; otherwise, <c>false</c>.</value>
        public bool CheckSet => Interlocked.Exchange(ref _state, True) == False;

        /// <summary>
        /// Mark the guard as being checked.
        /// </summary>
        public void MarkChecked()
        {
            Interlocked.Exchange(ref _state, True);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref _state, False);
        }
    }
}