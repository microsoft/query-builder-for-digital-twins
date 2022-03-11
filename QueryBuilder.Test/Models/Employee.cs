// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class Employee : BasicDigitalTwin
    {
        public Employee()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:microsoft:Employee;214";

        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }

        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonPropertyName("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("businessPhone")]
        public string BusinessPhone { get; set; }
    }
}