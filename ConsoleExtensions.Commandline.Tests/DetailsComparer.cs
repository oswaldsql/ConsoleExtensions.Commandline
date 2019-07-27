// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetailsComparer.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System;
    using System.Collections.Generic;

    using ConsoleExtensions.Commandline.Help;

    /// <summary>
    ///     Class DetailsComparer. Implements the IEqualityComparer
    /// </summary>
    /// <seealso cref="!:System.Collections.Generic.IEqualityComparer{ArgumentDetails}" />
    public class DetailsComparer : IEqualityComparer<ArgumentDetails>
    {
        /// <summary>
        ///     Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the specified objects are equal;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(ArgumentDetails x, ArgumentDetails y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Name == y.Name && x.DisplayName == y.DisplayName && x.Description == y.Description
                   && x.Optional == y.Optional && x.Type == y.Type
                   && ((x.DefaultValue == null && y.DefaultValue == null) || x.DefaultValue.Equals(y.DefaultValue));
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="System.Object" /> for which a hash code is to be
        ///     returned.
        /// </param>
        /// <exception cref="System.NotImplementedException">
        ///     Always thrown.
        /// </exception>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing
        ///     algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(ArgumentDetails obj)
        {
            throw new NotImplementedException();
        }
    }
}