// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.CosmosDB;

namespace Azure.ResourceManager.CosmosDB.Models
{
    /// <summary> The List operation response, that contains the Cassandra views and their properties. </summary>
    internal partial class CassandraViewListResult
    {
        /// <summary> Initializes a new instance of CassandraViewListResult. </summary>
        internal CassandraViewListResult()
        {
            Value = new ChangeTrackingList<CassandraViewGetResultData>();
        }

        /// <summary> Initializes a new instance of CassandraViewListResult. </summary>
        /// <param name="value"> List of Cassandra views and their properties. </param>
        internal CassandraViewListResult(IReadOnlyList<CassandraViewGetResultData> value)
        {
            Value = value;
        }

        /// <summary> List of Cassandra views and their properties. </summary>
        public IReadOnlyList<CassandraViewGetResultData> Value { get; }
    }
}
