// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers
{
    using Azure.DigitalTwins.Core;

    internal static class Terms
    {
        internal const string Select = "SELECT";
        internal const string From = "FROM";
        internal const string Where = "WHERE";
        internal const string Join = "JOIN";
        internal const string Related = "RELATED";
        internal const string Top = "TOP";
        internal const string Count = "COUNT";
        internal const string In = "IN";
        internal const string NotIn = "NIN";
        internal const string IsOfModel = "IS_OF_MODEL";
        internal const string And = "AND";
        internal const string Or = "OR";
        internal const string Not = "NOT";
        internal const string DigitalTwins = "DIGITALTWINS";
        internal const string Equal = "=";
        internal const string NotEqual = "!=";
        internal const string Less = "<";
        internal const string LessOrEqual = "<=";
        internal const string Greater = ">";
        internal const string GreaterOrEqual = ">=";
        internal const string IsBool = "IS_BOOL";
        internal const string IsDefined = "IS_DEFINED";
        internal const string IsNull = "IS_NULL";
        internal const string IsNumber = "IS_NUMBER";
        internal const string IsObject = "IS_OBJECT";
        internal const string IsString = "IS_STRING";
        internal const string IsPrimitive = "IS_PRIMITIVE";
        internal const string EndsWith = "ENDSWITH";
        internal const string StartsWith = "STARTSWITH";
        internal const string Contains = "CONTAINS";
    }
}