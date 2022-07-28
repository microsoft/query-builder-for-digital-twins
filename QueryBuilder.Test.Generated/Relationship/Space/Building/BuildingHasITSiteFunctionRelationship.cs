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

    public class BuildingHasITSiteFunctionRelationship : Relationship<ITSiteFunction>, IEquatable<BuildingHasITSiteFunctionRelationship>
    {
        public BuildingHasITSiteFunctionRelationship()
        {
            Name = "hasITSiteFunction";
        }

        public BuildingHasITSiteFunctionRelationship(Building source, ITSiteFunction target) : this()
        {
            InitializeFromTwins(source, target);
        }

        [JsonPropertyName("maxPriority")]
        public int? MaxPriority { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as BuildingHasITSiteFunctionRelationship);
        }

        public bool Equals(BuildingHasITSiteFunctionRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name && MaxPriority == other.MaxPriority;
        }

        public static bool operator ==(BuildingHasITSiteFunctionRelationship? left, BuildingHasITSiteFunctionRelationship? right)
        {
            return EqualityComparer<BuildingHasITSiteFunctionRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(BuildingHasITSiteFunctionRelationship? left, BuildingHasITSiteFunctionRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode(), MaxPriority?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as BuildingHasITSiteFunctionRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}