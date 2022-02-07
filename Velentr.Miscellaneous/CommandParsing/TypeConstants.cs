/// <file>
/// Velentr.Miscellaneous\CommandParsing\TypeConstants.cs
/// </file>
///
/// <copyright file="TypeConstants.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the type constants class.
/// </summary>
using System;
using System.Collections.Generic;

namespace Velentr.Miscellaneous.CommandParsing
{
    /// <summary>
    /// A type constants.
    /// </summary>
    public static class TypeConstants
    {
        /// <summary>
        /// Type of the int.
        /// </summary>
        public static Type IntType = typeof(int);

        /// <summary>
        /// Type of the long.
        /// </summary>
        public static Type LongType = typeof(long);

        /// <summary>
        /// Type of the short.
        /// </summary>
        public static Type ShortType = typeof(short);

        /// <summary>
        /// Type of the unsigned int.
        /// </summary>
        public static Type UnsignedIntType = typeof(uint);

        /// <summary>
        /// Type of the unsigned long.
        /// </summary>
        public static Type UnsignedLongType = typeof(ulong);

        /// <summary>
        /// Type of the unsigned short.
        /// </summary>
        public static Type UnsignedShortType = typeof(ushort);

        /// <summary>
        /// Type of the byte.
        /// </summary>
        public static Type ByteType = typeof(byte);

        /// <summary>
        /// The type.
        /// </summary>
        public static Type BoolType = typeof(bool);

        /// <summary>
        /// Type of the string.
        /// </summary>
        public static Type StringType = typeof(string);

        /// <summary>
        /// Type of the float.
        /// </summary>
        public static Type FloatType = typeof(float);

        /// <summary>
        /// Type of the double.
        /// </summary>
        public static Type DoubleType = typeof(double);

        /// <summary>
        /// Type of the decimal.
        /// </summary>
        public static Type DecimalType = typeof(decimal);

        /// <summary>
        /// Type of the byte.
        /// </summary>
        public static Type SByteType = typeof(sbyte);

        /// <summary>
        /// Type of the character.
        /// </summary>
        public static Type CharType = typeof(char);

        /// <summary>
        /// Type of the int.
        /// </summary>
        public static Type NIntType = typeof(nint);

        /// <summary>
        /// Type of the unsigned int.
        /// </summary>
        public static Type NUnsignedIntType = typeof(nuint);

        /// <summary>
        /// List of types of the valids.
        /// </summary>
        public static List<Type> ValidTypes = new()
        {
            IntType,
            LongType,
            ShortType,
            UnsignedIntType,
            UnsignedLongType,
            UnsignedShortType,
            ByteType,
            BoolType,
            StringType,
            FloatType,
            DoubleType,
            DecimalType,
            SByteType,
            CharType,
            NIntType,
            NUnsignedIntType
        };
    }
}