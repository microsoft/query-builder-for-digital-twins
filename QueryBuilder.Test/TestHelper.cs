// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    public static class TestHelper
    {
        public static string UpdateVersion(this string model, int version)
        {
            var modelType = model.Split(';')[0];
            return $"{modelType};{version}";
        }
    }
}