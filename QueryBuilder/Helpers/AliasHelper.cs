// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers
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