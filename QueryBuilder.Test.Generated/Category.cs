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
    public class Category : IEquatable<Category>
    {
        [JsonPropertyName("isCreativeSpace")]
        public bool? IsCreativeSpace { get; set; }
        [JsonPropertyName("isSportsAndRecreation")]
        public bool? IsSportsAndRecreation { get; set; }
        [JsonPropertyName("isLandmark")]
        public bool? IsLandmark { get; set; }
        [JsonPropertyName("isRecreationSpace")]
        public bool? IsRecreationSpace { get; set; }
        [JsonPropertyName("isPhotoWorthy")]
        public bool? IsPhotoWorthy { get; set; }
        [JsonPropertyName("isArtwork")]
        public bool? IsArtwork { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Category);
        }

        public bool Equals(Category? other)
        {
            return other is not null && IsCreativeSpace == other.IsCreativeSpace && IsSportsAndRecreation == other.IsSportsAndRecreation && IsLandmark == other.IsLandmark && IsRecreationSpace == other.IsRecreationSpace && IsPhotoWorthy == other.IsPhotoWorthy && IsArtwork == other.IsArtwork;
        }

        public static bool operator ==(Category? left, Category? right)
        {
            return EqualityComparer<Category?>.Default.Equals(left, right);
        }

        public static bool operator !=(Category? left, Category? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(IsCreativeSpace?.GetHashCode(), IsSportsAndRecreation?.GetHashCode(), IsLandmark?.GetHashCode(), IsRecreationSpace?.GetHashCode(), IsPhotoWorthy?.GetHashCode(), IsArtwork?.GetHashCode());
        }
    }
}