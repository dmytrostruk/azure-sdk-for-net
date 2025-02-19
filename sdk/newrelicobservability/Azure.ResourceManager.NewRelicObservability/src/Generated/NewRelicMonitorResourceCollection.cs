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

namespace Azure.ResourceManager.NewRelicObservability
{
    /// <summary>
    /// A class representing a collection of <see cref="NewRelicMonitorResource" /> and their operations.
    /// Each <see cref="NewRelicMonitorResource" /> in the collection will belong to the same instance of <see cref="ResourceGroupResource" />.
    /// To get a <see cref="NewRelicMonitorResourceCollection" /> instance call the GetNewRelicMonitorResources method from an instance of <see cref="ResourceGroupResource" />.
    /// </summary>
    public partial class NewRelicMonitorResourceCollection : ArmCollection, IEnumerable<NewRelicMonitorResource>, IAsyncEnumerable<NewRelicMonitorResource>
    {
        private readonly ClientDiagnostics _newRelicMonitorResourceMonitorsClientDiagnostics;
        private readonly MonitorsRestOperations _newRelicMonitorResourceMonitorsRestClient;

        /// <summary> Initializes a new instance of the <see cref="NewRelicMonitorResourceCollection"/> class for mocking. </summary>
        protected NewRelicMonitorResourceCollection()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="NewRelicMonitorResourceCollection"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the parent resource that is the target of operations. </param>
        internal NewRelicMonitorResourceCollection(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _newRelicMonitorResourceMonitorsClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.NewRelicObservability", NewRelicMonitorResource.ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(NewRelicMonitorResource.ResourceType, out string newRelicMonitorResourceMonitorsApiVersion);
            _newRelicMonitorResourceMonitorsRestClient = new MonitorsRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, newRelicMonitorResourceMonitorsApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != ResourceGroupResource.ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, ResourceGroupResource.ResourceType), nameof(id));
        }

        /// <summary>
        /// Create a NewRelicMonitorResource
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_CreateOrUpdate</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="data"> Resource create parameters. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> or <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<NewRelicMonitorResource>> CreateOrUpdateAsync(WaitUntil waitUntil, string monitorName, NewRelicMonitorResourceData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = await _newRelicMonitorResourceMonitorsRestClient.CreateOrUpdateAsync(Id.SubscriptionId, Id.ResourceGroupName, monitorName, data, cancellationToken).ConfigureAwait(false);
                var operation = new NewRelicObservabilityArmOperation<NewRelicMonitorResource>(new NewRelicMonitorResourceOperationSource(Client), _newRelicMonitorResourceMonitorsClientDiagnostics, Pipeline, _newRelicMonitorResourceMonitorsRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, monitorName, data).Request, response, OperationFinalStateVia.AzureAsyncOperation);
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
        /// Create a NewRelicMonitorResource
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_CreateOrUpdate</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="data"> Resource create parameters. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> or <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<NewRelicMonitorResource> CreateOrUpdate(WaitUntil waitUntil, string monitorName, NewRelicMonitorResourceData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = _newRelicMonitorResourceMonitorsRestClient.CreateOrUpdate(Id.SubscriptionId, Id.ResourceGroupName, monitorName, data, cancellationToken);
                var operation = new NewRelicObservabilityArmOperation<NewRelicMonitorResource>(new NewRelicMonitorResourceOperationSource(Client), _newRelicMonitorResourceMonitorsClientDiagnostics, Pipeline, _newRelicMonitorResourceMonitorsRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, monitorName, data).Request, response, OperationFinalStateVia.AzureAsyncOperation);
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
        /// Get a NewRelicMonitorResource
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> is null. </exception>
        public virtual async Task<Response<NewRelicMonitorResource>> GetAsync(string monitorName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.Get");
            scope.Start();
            try
            {
                var response = await _newRelicMonitorResourceMonitorsRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, monitorName, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new NewRelicMonitorResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Get a NewRelicMonitorResource
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> is null. </exception>
        public virtual Response<NewRelicMonitorResource> Get(string monitorName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.Get");
            scope.Start();
            try
            {
                var response = _newRelicMonitorResourceMonitorsRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, monitorName, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new NewRelicMonitorResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// List NewRelicMonitorResource resources by resource group
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_ListByResourceGroup</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="NewRelicMonitorResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual AsyncPageable<NewRelicMonitorResource> GetAllAsync(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _newRelicMonitorResourceMonitorsRestClient.CreateListByResourceGroupRequest(Id.SubscriptionId, Id.ResourceGroupName);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _newRelicMonitorResourceMonitorsRestClient.CreateListByResourceGroupNextPageRequest(nextLink, Id.SubscriptionId, Id.ResourceGroupName);
            return PageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, e => new NewRelicMonitorResource(Client, NewRelicMonitorResourceData.DeserializeNewRelicMonitorResourceData(e)), _newRelicMonitorResourceMonitorsClientDiagnostics, Pipeline, "NewRelicMonitorResourceCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// List NewRelicMonitorResource resources by resource group
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_ListByResourceGroup</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="NewRelicMonitorResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual Pageable<NewRelicMonitorResource> GetAll(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _newRelicMonitorResourceMonitorsRestClient.CreateListByResourceGroupRequest(Id.SubscriptionId, Id.ResourceGroupName);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _newRelicMonitorResourceMonitorsRestClient.CreateListByResourceGroupNextPageRequest(nextLink, Id.SubscriptionId, Id.ResourceGroupName);
            return PageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, e => new NewRelicMonitorResource(Client, NewRelicMonitorResourceData.DeserializeNewRelicMonitorResourceData(e)), _newRelicMonitorResourceMonitorsClientDiagnostics, Pipeline, "NewRelicMonitorResourceCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> is null. </exception>
        public virtual async Task<Response<bool>> ExistsAsync(string monitorName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.Exists");
            scope.Start();
            try
            {
                var response = await _newRelicMonitorResourceMonitorsRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, monitorName, cancellationToken: cancellationToken).ConfigureAwait(false);
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
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/NewRelic.Observability/monitors/{monitorName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Monitors_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="monitorName"> Name of the Monitors resource. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="monitorName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="monitorName"/> is null. </exception>
        public virtual Response<bool> Exists(string monitorName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(monitorName, nameof(monitorName));

            using var scope = _newRelicMonitorResourceMonitorsClientDiagnostics.CreateScope("NewRelicMonitorResourceCollection.Exists");
            scope.Start();
            try
            {
                var response = _newRelicMonitorResourceMonitorsRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, monitorName, cancellationToken: cancellationToken);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        IEnumerator<NewRelicMonitorResource> IEnumerable<NewRelicMonitorResource>.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IAsyncEnumerator<NewRelicMonitorResource> IAsyncEnumerable<NewRelicMonitorResource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAllAsync(cancellationToken: cancellationToken).GetAsyncEnumerator(cancellationToken);
        }
    }
}
