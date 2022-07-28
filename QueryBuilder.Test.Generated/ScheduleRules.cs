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
    public class ScheduleRules : IEquatable<ScheduleRules>
    {
        [JsonPropertyName("isAccessAfterHoursAllowed")]
        public bool? IsAccessAfterHoursAllowed { get; set; }
        [JsonPropertyName("isFTEAndGuestsAllowed")]
        public bool? IsFTEAndGuestsAllowed { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as ScheduleRules);
        }

        public bool Equals(ScheduleRules? other)
        {
            return other is not null && IsAccessAfterHoursAllowed == other.IsAccessAfterHoursAllowed && IsFTEAndGuestsAllowed == other.IsFTEAndGuestsAllowed;
        }

        public static bool operator ==(ScheduleRules? left, ScheduleRules? right)
        {
            return EqualityComparer<ScheduleRules?>.Default.Equals(left, right);
        }

        public static bool operator !=(ScheduleRules? left, ScheduleRules? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(IsAccessAfterHoursAllowed?.GetHashCode(), IsFTEAndGuestsAllowed?.GetHashCode());
        }
    }
}