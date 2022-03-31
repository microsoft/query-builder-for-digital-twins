// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements
{
    using System;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class JoinOptions
    {
        internal string With { get; set; }

        internal string Source { get; set; }

        internal string RelationshipName { get; set; }

        internal string RelationshipAlias { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TWhereStatement"></typeparam>
    public class JoinWithStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private string source;
        private readonly WhereClause whereClause;

        internal JoinWithStatement(WhereClause whereClause, string source = DefaultTwinAlias)
        {
            this.whereClause = whereClause;
            this.source = source;
        }

        internal JoinRelatedByStatement<TWhereStatement> With(string with)
        {
            return new JoinRelatedByStatement<TWhereStatement>(whereClause, new JoinOptions { With = with, Source = source });
        }
    }

    internal class JoinRelatedByStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly WhereClause whereClause;
        private readonly JoinOptions options;

        public JoinRelatedByStatement(WhereClause whereClause, JoinOptions options)
        {
            this.whereClause = whereClause;
            this.options = options;
        }

        public JoinFinalStatement<TWhereStatement> RelatedBy(string relationshipName)
        {
            options.RelationshipName = relationshipName;
            return new JoinFinalStatement<TWhereStatement>(whereClause, options);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TWhereStatement"></typeparam>
    public class JoinFinalStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinOptions Options { get; private set; }

        private WhereClause whereClause;

        internal JoinFinalStatement(WhereClause whereClause, JoinOptions options)
        {
            this.whereClause = whereClause;
            this.Options = options;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceTwin"></param>
        /// <returns></returns>
        public JoinFinalStatement<TWhereStatement> On(string sourceTwin)
        {
            Options.Source = sourceTwin;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="relationshipAlias"></param>
        /// <returns></returns>
        public JoinFinalStatement<TWhereStatement> WithAlias(string relationshipAlias)
        {
            Options.RelationshipAlias = relationshipAlias;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="whereLogic"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> Where(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> whereLogic)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(Options, whereClause, Options.With);
            return whereLogic.Invoke(statement);
        }
    }
}