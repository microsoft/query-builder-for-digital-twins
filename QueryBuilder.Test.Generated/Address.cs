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

    public class Address : BasicDigitalTwin, IEquatable<Address>, IEquatable<BasicDigitalTwin>
    {
        public Address()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Address;1";
        [JsonPropertyName("street1")]
        public string? Street1 { get; set; }
        [JsonPropertyName("street2")]
        public string? Street2 { get; set; }
        [JsonPropertyName("county")]
        public string? County { get; set; }
        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }
        [JsonIgnore]
        public AddressHasStateRelationshipCollection HasState { get; set; } = new AddressHasStateRelationshipCollection();
        [JsonIgnore]
        public AddressHasCityRelationshipCollection HasCity { get; set; } = new AddressHasCityRelationshipCollection();
        [JsonIgnore]
        public AddressHasCountryRelationshipCollection HasCountry { get; set; } = new AddressHasCountryRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Address);
        }

        public bool Equals(Address? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Street1 == other.Street1 && Street2 == other.Street2 && County == other.County && Zipcode == other.Zipcode;
        }

        public static bool operator ==(Address? left, Address? right)
        {
            return EqualityComparer<Address?>.Default.Equals(left, right);
        }

        public static bool operator !=(Address? left, Address? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Street1?.GetHashCode(), Street2?.GetHashCode(), County?.GetHashCode(), Zipcode?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Address) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}