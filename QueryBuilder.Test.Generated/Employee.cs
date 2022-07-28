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

    public class Employee : BasicDigitalTwin, IEquatable<Employee>, IEquatable<BasicDigitalTwin>
    {
        public Employee()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Employee;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Employee);
        }

        public bool Equals(Employee? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Employee? left, Employee? right)
        {
            return EqualityComparer<Employee?>.Default.Equals(left, right);
        }

        public static bool operator !=(Employee? left, Employee? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Employee) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}