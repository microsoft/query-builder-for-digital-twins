// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses
{
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal abstract class Condition
    {
    }

    internal class SimpleCondition : Condition
    {
        internal string Alias { get; set; }
    }

    internal class CompoundCondition : Condition
    {
        internal IList<string> Conditions { get; set; }

        protected string LogicalOperator { get; set; }

        public override string ToString()
        {
            return $"({string.Join($" {LogicalOperator} ", Conditions)})";
        }
    }

    internal class WhereComparisonCondition : SimpleCondition
    {
        internal string Column { get; set; }

        internal ComparisonOperators Operator { get; set; }

        internal object Value { get; set; }

        public override string ToString()
        {
            return $"{Alias}.{Column} {Operator.Operator} {Value.WrapQuotes()}";
        }
    }

    internal class WhereInCondition : SimpleCondition
    {
        internal string Column { get; set; }

        internal string[] Value { get; set; }

        public override string ToString()
        {
            return $"{Alias}.{Column} {In} [{Value.WrapArrayQuotes()}]";
        }
    }

    internal class WhereNotInCondition : SimpleCondition
    {
        internal string Column { get; set; }

        internal string[] Value { get; set; }

        public override string ToString()
        {
            return $"{Alias}.{Column} {NotIn} [{Value.WrapArrayQuotes()}]";
        }
    }

    internal class WhereIsOfModelCondition : SimpleCondition
    {
        private const int minTwinVersion = 1;

        internal string Model { get; set; }

        public WhereIsOfModelCondition(string modelAlias, string model)
        {
            Alias = modelAlias;
            Model = model;
        }

        public override string ToString()
        {
            return $"{IsOfModel}({Alias}, '{GetModelWithVersion(Model, minTwinVersion)}')";
        }

        private string GetModelWithVersion(string model, int userSelectedVersion)
        {
            var modelType = model.Split(';')[0];
            return $"{modelType};{userSelectedVersion}";
        }
    }

    internal class WhereScalarFunctionCondition : SimpleCondition
    {
        internal string Column { get; set; }

        internal ScalarOperator ScalarFunction { get; set; }

        internal string Value { get; set; } = string.Empty;

        public override string ToString()
        {
            if (ScalarFunction is ScalarBinaryOperator)
            {
                return $"{ScalarFunction.Name}({Alias}.{Column}, '{Value.EscapeValue()}')";
            }

            return $"{ScalarFunction.Name}({Alias}.{Column})";
        }
    }

    internal class NotCondition : Condition
    {
        internal string Condition { get; set; }

        public override string ToString()
        {
            return $"{Not} {Condition}";
        }
    }

    internal class OrCondition : CompoundCondition
    {
        internal OrCondition()
        {
            LogicalOperator = Or;
        }
    }

    internal class AndCondition : CompoundCondition
    {
        internal AndCondition()
        {
            LogicalOperator = And;
        }
    }
}