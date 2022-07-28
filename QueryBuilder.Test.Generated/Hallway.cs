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

    public class Hallway : Space, IEquatable<Hallway>
    {
        public Hallway()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Hallway;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Hallway);
        }

        public bool Equals(Hallway? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Hallway? left, Hallway? right)
        {
            return EqualityComparer<Hallway?>.Default.Equals(left, right);
        }

        public static bool operator !=(Hallway? left, Hallway? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}