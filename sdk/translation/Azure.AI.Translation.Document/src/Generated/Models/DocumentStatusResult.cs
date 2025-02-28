// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace Azure.AI.Translation.Document
{
    /// <summary> Document Status Response. </summary>
    public partial class DocumentStatusResult
    {
        /// <summary> Initializes a new instance of DocumentStatusResult. </summary>
        /// <param name="sourceDocumentUri"> Location of the source document. </param>
        /// <param name="createdOn"> Operation created date time. </param>
        /// <param name="lastModified"> Date time in which the operation's status has been updated. </param>
        /// <param name="status"> List of possible statuses for job or document. </param>
        /// <param name="translatedToLanguageCode"> To language. </param>
        /// <param name="progress"> Progress of the translation if available. </param>
        /// <param name="id"> Document Id. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="sourceDocumentUri"/>, <paramref name="translatedToLanguageCode"/> or <paramref name="id"/> is null. </exception>
        internal DocumentStatusResult(Uri sourceDocumentUri, DateTimeOffset createdOn, DateTimeOffset lastModified, DocumentTranslationStatus status, string translatedToLanguageCode, float progress, string id)
        {
            Argument.AssertNotNull(sourceDocumentUri, nameof(sourceDocumentUri));
            Argument.AssertNotNull(translatedToLanguageCode, nameof(translatedToLanguageCode));
            Argument.AssertNotNull(id, nameof(id));

            SourceDocumentUri = sourceDocumentUri;
            CreatedOn = createdOn;
            LastModified = lastModified;
            Status = status;
            TranslatedToLanguageCode = translatedToLanguageCode;
            Progress = progress;
            Id = id;
        }

        /// <summary> Initializes a new instance of DocumentStatusResult. </summary>
        /// <param name="translatedDocumentUri"> Location of the document or folder. </param>
        /// <param name="sourceDocumentUri"> Location of the source document. </param>
        /// <param name="createdOn"> Operation created date time. </param>
        /// <param name="lastModified"> Date time in which the operation's status has been updated. </param>
        /// <param name="status"> List of possible statuses for job or document. </param>
        /// <param name="translatedToLanguageCode"> To language. </param>
        /// <param name="error"> This contains an outer error with error code, message, details, target and an inner error with more descriptive details. </param>
        /// <param name="progress"> Progress of the translation if available. </param>
        /// <param name="id"> Document Id. </param>
        /// <param name="charactersCharged"> Character charged by the API. </param>
        internal DocumentStatusResult(Uri translatedDocumentUri, Uri sourceDocumentUri, DateTimeOffset createdOn, DateTimeOffset lastModified, DocumentTranslationStatus status, string translatedToLanguageCode, JsonElement error, float progress, string id, long charactersCharged)
        {
            TranslatedDocumentUri = translatedDocumentUri;
            SourceDocumentUri = sourceDocumentUri;
            CreatedOn = createdOn;
            LastModified = lastModified;
            Status = status;
            TranslatedToLanguageCode = translatedToLanguageCode;
            _error = error;
            Progress = progress;
            Id = id;
            CharactersCharged = charactersCharged;
        }
    }
}
