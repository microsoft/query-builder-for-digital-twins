// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers
{
    using System;

    internal class AliasHelper
    {
        internal static string ExtractAliasFromType(Type type)
        {
            return type.Name.ToLower();
        }
    }
}