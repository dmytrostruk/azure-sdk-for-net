// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager;

namespace Azure.ResourceManager.DataShare
{
    /// <summary>
    /// A Class representing a ShareDataSetMapping along with the instance operations that can be performed on it.
    /// If you have a <see cref="ResourceIdentifier" /> you can construct a <see cref="ShareDataSetMappingResource" />
    /// from an instance of <see cref="ArmClient" /> using the GetShareDataSetMappingResource method.
    /// Otherwise you can get one from its parent resource <see cref="ShareSubscriptionResource" /> using the GetShareDataSetMapping method.
    /// </summary>
    public partial class ShareDataSetMappingResource : ArmResource
    {
        /// <summary> Generate the resource identifier of a <see cref="ShareDataSetMappingResource"/> instance. </summary>
        public static ResourceIdentifier CreateResourceIdentifier(string subscriptionId, string resourceGroupName, string accountName, string shareSubscriptionName, string dataSetMappingName)
        {
            var resourceId = $"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}";
            return new ResourceIdentifier(resourceId);
        }

        private readonly ClientDiagnostics _shareDataSetMappingDataSetMappingsClientDiagnostics;
        private readonly DataSetMappingsRestOperations _shareDataSetMappingDataSetMappingsRestClient;
        private readonly ShareDataSetMappingData _data;

        /// <summary> Initializes a new instance of the <see cref="ShareDataSetMappingResource"/> class for mocking. </summary>
        protected ShareDataSetMappingResource()
        {
        }

        /// <summary> Initializes a new instance of the <see cref = "ShareDataSetMappingResource"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="data"> The resource that is the target of operations. </param>
        internal ShareDataSetMappingResource(ArmClient client, ShareDataSetMappingData data) : this(client, data.Id)
        {
            HasData = true;
            _data = data;
        }

        /// <summary> Initializes a new instance of the <see cref="ShareDataSetMappingResource"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the resource that is the target of operations. </param>
        internal ShareDataSetMappingResource(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _shareDataSetMappingDataSetMappingsClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.DataShare", ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(ResourceType, out string shareDataSetMappingDataSetMappingsApiVersion);
            _shareDataSetMappingDataSetMappingsRestClient = new DataSetMappingsRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, shareDataSetMappingDataSetMappingsApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        /// <summary> Gets the resource type for the operations. </summary>
        public static readonly ResourceType ResourceType = "Microsoft.DataShare/accounts/shareSubscriptions/dataSetMappings";

        /// <summary> Gets whether or not the current instance has data. </summary>
        public virtual bool HasData { get; }

        /// <summary> Gets the data representing this Feature. </summary>
        /// <exception cref="InvalidOperationException"> Throws if there is no data loaded in the current instance. </exception>
        public virtual ShareDataSetMappingData Data
        {
            get
            {
                if (!HasData)
                    throw new InvalidOperationException("The current instance does not have data, you must call Get first.");
                return _data;
            }
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, ResourceType), nameof(id));
        }

        /// <summary>
        /// Get a DataSetMapping in a shareSubscription
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ShareDataSetMappingResource>> GetAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Get");
            scope.Start();
            try
            {
                var response = await _shareDataSetMappingDataSetMappingsRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new ShareDataSetMappingResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Get a DataSetMapping in a shareSubscription
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ShareDataSetMappingResource> Get(CancellationToken cancellationToken = default)
        {
            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Get");
            scope.Start();
            try
            {
                var response = _shareDataSetMappingDataSetMappingsRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new ShareDataSetMappingResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Delete a DataSetMapping in a shareSubscription
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Delete</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<ArmOperation> DeleteAsync(WaitUntil waitUntil, CancellationToken cancellationToken = default)
        {
            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Delete");
            scope.Start();
            try
            {
                var response = await _shareDataSetMappingDataSetMappingsRestClient.DeleteAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, cancellationToken).ConfigureAwait(false);
                var operation = new DataShareArmOperation(response);
                if (waitUntil == WaitUntil.Completed)
                    await operation.WaitForCompletionResponseAsync(cancellationToken).ConfigureAwait(false);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Delete a DataSetMapping in a shareSubscription
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Delete</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual ArmOperation Delete(WaitUntil waitUntil, CancellationToken cancellationToken = default)
        {
            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Delete");
            scope.Start();
            try
            {
                var response = _shareDataSetMappingDataSetMappingsRestClient.Delete(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, cancellationToken);
                var operation = new DataShareArmOperation(response);
                if (waitUntil == WaitUntil.Completed)
                    operation.WaitForCompletionResponse(cancellationToken);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Create a DataSetMapping
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="data"> Destination data set configuration details. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<ShareDataSetMappingResource>> UpdateAsync(WaitUntil waitUntil, ShareDataSetMappingData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Update");
            scope.Start();
            try
            {
                var response = await _shareDataSetMappingDataSetMappingsRestClient.CreateAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, data, cancellationToken).ConfigureAwait(false);
                var operation = new DataShareArmOperation<ShareDataSetMappingResource>(Response.FromValue(new ShareDataSetMappingResource(Client, response), response.GetRawResponse()));
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
        /// Create a DataSetMapping
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DataShare/accounts/{accountName}/shareSubscriptions/{shareSubscriptionName}/dataSetMappings/{dataSetMappingName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>DataSetMappings_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="data"> Destination data set configuration details. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<ShareDataSetMappingResource> Update(WaitUntil waitUntil, ShareDataSetMappingData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _shareDataSetMappingDataSetMappingsClientDiagnostics.CreateScope("ShareDataSetMappingResource.Update");
            scope.Start();
            try
            {
                var response = _shareDataSetMappingDataSetMappingsRestClient.Create(Id.SubscriptionId, Id.ResourceGroupName, Id.Parent.Parent.Name, Id.Parent.Name, Id.Name, data, cancellationToken);
                var operation = new DataShareArmOperation<ShareDataSetMappingResource>(Response.FromValue(new ShareDataSetMappingResource(Client, response), response.GetRawResponse()));
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
    }
}
