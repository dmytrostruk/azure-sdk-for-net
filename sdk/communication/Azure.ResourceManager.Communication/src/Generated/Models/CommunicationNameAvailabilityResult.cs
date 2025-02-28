// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.ResourceManager.Communication.Models
{
    /// <summary> The check availability result. </summary>
    public partial class CommunicationNameAvailabilityResult
    {
        /// <summary> Initializes a new instance of CommunicationNameAvailabilityResult. </summary>
        internal CommunicationNameAvailabilityResult()
        {
        }

        /// <summary> Initializes a new instance of CommunicationNameAvailabilityResult. </summary>
        /// <param name="isNameAvailable"> Indicates if the resource name is available. </param>
        /// <param name="reason"> The reason why the given name is not available. </param>
        /// <param name="message"> Detailed reason why the given name is available. </param>
        internal CommunicationNameAvailabilityResult(bool? isNameAvailable, CommunicationNameAvailabilityReason? reason, string message)
        {
            IsNameAvailable = isNameAvailable;
            Reason = reason;
            Message = message;
        }

        /// <summary> Indicates if the resource name is available. </summary>
        public bool? IsNameAvailable { get; }
        /// <summary> The reason why the given name is not available. </summary>
        public CommunicationNameAvailabilityReason? Reason { get; }
        /// <summary> Detailed reason why the given name is available. </summary>
        public string Message { get; }
    }
}
