// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers
{
    using System;
    using Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses;

    internal static class ConditionHelper
    {
        internal static WhereIsOfModelCondition CreateWhereIsOfModelCondition(string modelAlias, string model)
        {
            return new WhereIsOfModelCondition
            {
                Alias = modelAlias,
                Model = model
            };
        }
    }
}