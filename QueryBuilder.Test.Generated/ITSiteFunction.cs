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

    public class ITSiteFunction : BasicDigitalTwin, IEquatable<ITSiteFunction>, IEquatable<BasicDigitalTwin>
    {
        public ITSiteFunction()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:ITSiteFunction;1";
        [JsonPropertyName("externalId")]
        public int? ExternalId { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as ITSiteFunction);
        }

        public bool Equals(ITSiteFunction? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && ExternalId == other.ExternalId && Description == other.Description;
        }

        public static bool operator ==(ITSiteFunction? left, ITSiteFunction? right)
        {
            return EqualityComparer<ITSiteFunction?>.Default.Equals(left, right);
        }

        public static bool operator !=(ITSiteFunction? left, ITSiteFunction? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), ExternalId?.GetHashCode(), Description?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ITSiteFunction) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}