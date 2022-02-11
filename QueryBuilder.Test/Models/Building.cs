// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Building : Space
    {
        public Building()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:microsoft:Space:Building;1";

        [JsonPropertyName("businessEntityNumber")]
        public string BusinessEntityNumber { get; set; }

        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("squareMeter")]
        public float? SquareMeter { get; set; }

        [JsonPropertyName("rationalSortKey")]
        public string RationalSortKey { get; set; }

        [JsonPropertyName("regionId")]
        public string RegionId { get; set; }

        [JsonPropertyName("startOfBusinessTime")]
        public object StartOfBusinessTime { get; set; }

        [JsonPropertyName("endOfBusinessTime")]
        public object EndOfBusinessTime { get; set; }

        [JsonPropertyName("businessEntityName")]
        public string BusinessEntityName { get; set; }

        [JsonPropertyName("amenities")]
        public IDictionary<string, bool> Amenities { get; set; }

        [JsonIgnore]
        public BuildingHasITSiteFunctionRelationship HasITSiteFunction { get; private set; } = new BuildingHasITSiteFunctionRelationship();

        [JsonIgnore]
        public BuildingHasSalesGeoRelationship HasSalesGeo { get; private set; } = new BuildingHasSalesGeoRelationship();

        [JsonIgnore]
        public BuildingHasBuildingContactRelationship HasBuildingContact { get; private set; } = new BuildingHasBuildingContactRelationship();
    }
}