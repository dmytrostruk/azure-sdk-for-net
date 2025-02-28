// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Azure.ResourceManager.SecurityCenter
{
    /// <summary>
    /// A class representing a collection of <see cref="IngestionSettingResource" /> and their operations.
    /// Each <see cref="IngestionSettingResource" /> in the collection will belong to the same instance of <see cref="SubscriptionResource" />.
    /// To get an <see cref="IngestionSettingCollection" /> instance call the GetIngestionSettings method from an instance of <see cref="SubscriptionResource" />.
    /// </summary>
    public partial class IngestionSettingCollection : ArmCollection, IEnumerable<IngestionSettingResource>, IAsyncEnumerable<IngestionSettingResource>
    {
        private readonly ClientDiagnostics _ingestionSettingClientDiagnostics;
        private readonly IngestionSettingsRestOperations _ingestionSettingRestClient;

        /// <summary> Initializes a new instance of the <see cref="IngestionSettingCollection"/> class for mocking. </summary>
        protected IngestionSettingCollection()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="IngestionSettingCollection"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the parent resource that is the target of operations. </param>
        internal IngestionSettingCollection(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _ingestionSettingClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.SecurityCenter", IngestionSettingResource.ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(IngestionSettingResource.ResourceType, out string ingestionSettingApiVersion);
            _ingestionSettingRestClient = new IngestionSettingsRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, ingestionSettingApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != SubscriptionResource.ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, SubscriptionResource.ResourceType), nameof(id));
        }

        /// <summary>
        /// Create setting for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="data"> Ingestion setting object. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> or <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<IngestionSettingResource>> CreateOrUpdateAsync(WaitUntil waitUntil, string ingestionSettingName, IngestionSettingData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = await _ingestionSettingRestClient.CreateAsync(Id.SubscriptionId, ingestionSettingName, data, cancellationToken).ConfigureAwait(false);
                var operation = new SecurityCenterArmOperation<IngestionSettingResource>(Response.FromValue(new IngestionSettingResource(Client, response), response.GetRawResponse()));
                if (waitUntil == WaitUntil.Completed)
                    await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Create setting for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="data"> Ingestion setting object. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> or <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<IngestionSettingResource> CreateOrUpdate(WaitUntil waitUntil, string ingestionSettingName, IngestionSettingData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = _ingestionSettingRestClient.Create(Id.SubscriptionId, ingestionSettingName, data, cancellationToken);
                var operation = new SecurityCenterArmOperation<IngestionSettingResource>(Response.FromValue(new IngestionSettingResource(Client, response), response.GetRawResponse()));
                if (waitUntil == WaitUntil.Completed)
                    operation.WaitForCompletion(cancellationToken);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Settings for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> is null. </exception>
        public virtual async Task<Response<IngestionSettingResource>> GetAsync(string ingestionSettingName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.Get");
            scope.Start();
            try
            {
                var response = await _ingestionSettingRestClient.GetAsync(Id.SubscriptionId, ingestionSettingName, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new IngestionSettingResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Settings for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> is null. </exception>
        public virtual Response<IngestionSettingResource> Get(string ingestionSettingName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.Get");
            scope.Start();
            try
            {
                var response = _ingestionSettingRestClient.Get(Id.SubscriptionId, ingestionSettingName, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new IngestionSettingResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Settings for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_List</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="IngestionSettingResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual AsyncPageable<IngestionSettingResource> GetAllAsync(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _ingestionSettingRestClient.CreateListRequest(Id.SubscriptionId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _ingestionSettingRestClient.CreateListNextPageRequest(nextLink, Id.SubscriptionId);
            return PageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, e => new IngestionSettingResource(Client, IngestionSettingData.DeserializeIngestionSettingData(e)), _ingestionSettingClientDiagnostics, Pipeline, "IngestionSettingCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// Settings for ingesting security data and logs to correlate with resources associated with the subscription.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_List</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="IngestionSettingResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual Pageable<IngestionSettingResource> GetAll(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _ingestionSettingRestClient.CreateListRequest(Id.SubscriptionId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _ingestionSettingRestClient.CreateListNextPageRequest(nextLink, Id.SubscriptionId);
            return PageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, e => new IngestionSettingResource(Client, IngestionSettingData.DeserializeIngestionSettingData(e)), _ingestionSettingClientDiagnostics, Pipeline, "IngestionSettingCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> is null. </exception>
        public virtual async Task<Response<bool>> ExistsAsync(string ingestionSettingName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.Exists");
            scope.Start();
            try
            {
                var response = await _ingestionSettingRestClient.GetAsync(Id.SubscriptionId, ingestionSettingName, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Security/ingestionSettings/{ingestionSettingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>IngestionSettings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="ingestionSettingName"> Name of the ingestion setting. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="ingestionSettingName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="ingestionSettingName"/> is null. </exception>
        public virtual Response<bool> Exists(string ingestionSettingName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(ingestionSettingName, nameof(ingestionSettingName));

            using var scope = _ingestionSettingClientDiagnostics.CreateScope("IngestionSettingCollection.Exists");
            scope.Start();
            try
            {
                var response = _ingestionSettingRestClient.Get(Id.SubscriptionId, ingestionSettingName, cancellationToken: cancellationToken);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        IEnumerator<IngestionSettingResource> IEnumerable<IngestionSettingResource>.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IAsyncEnumerator<IngestionSettingResource> IAsyncEnumerable<IngestionSettingResource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAllAsync(cancellationToken: cancellationToken).GetAsyncEnumerator(cancellationToken);
        }
    }
}
