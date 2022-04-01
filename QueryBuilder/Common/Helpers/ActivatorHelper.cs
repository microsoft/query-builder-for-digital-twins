// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class ActivatorHelper
    {
        internal static T CreateInstance<T>(object[] args, BindingFlags flags)
        {
            if (!args.Any())
            {
                return Activator.CreateInstance<T>();
            }

            var constructors = typeof(T).GetConstructors(flags);
            if (!constructors.Any())
            {
                throw new Exception("No constructors returned for provided flags");
            }

            var matchedArgsConstructor = constructors.FirstOrDefault(c => c.GetParameters().Count() == args.Count());
            if (matchedArgsConstructor == null)
            {
                throw new Exception("No matching constructor for the supplied args");
            }

            return (T)matchedArgsConstructor.Invoke(args);
        }

        internal static T CreateInstance<T>(params object[] args)
        {
            return ActivatorHelper.CreateInstance<T>(args, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance);
        }
    }
}