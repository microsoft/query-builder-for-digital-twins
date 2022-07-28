// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [Serializable]
    public class OperationHour : IEquatable<OperationHour>
    {
        [JsonPropertyName("start")]
        public object? Start { get; set; }
        [JsonPropertyName("end")]
        public object? End { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as OperationHour);
        }

        public bool Equals(OperationHour? other)
        {
            return other is not null && Start == other.Start && End == other.End;
        }

        public static bool operator ==(OperationHour? left, OperationHour? right)
        {
            return EqualityComparer<OperationHour?>.Default.Equals(left, right);
        }

        public static bool operator !=(OperationHour? left, OperationHour? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Start?.GetHashCode(), End?.GetHashCode());
        }
    }
}