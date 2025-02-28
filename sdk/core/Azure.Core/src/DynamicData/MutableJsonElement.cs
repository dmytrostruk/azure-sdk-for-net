﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Azure.Core.Json
{
    /// <summary>
    /// A mutable representation of a JSON element.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(MutableJsonElementConverter))]
    internal readonly partial struct MutableJsonElement
    {
        internal const int MaxStackLimit = 1024;

        private readonly MutableJsonDocument _root;
        private readonly JsonElement _element;
        private readonly string _path;
        private readonly int _highWaterMark;

        private readonly MutableJsonDocument.ChangeTracker Changes => _root.Changes;

        internal MutableJsonElement(MutableJsonDocument root, JsonElement element, string path, int highWaterMark = -1)
        {
            _element = element;
            _root = root;
            _path = path;
            _highWaterMark = highWaterMark;
        }

        /// <summary>
        /// Gets the type of the current JSON value.
        /// </summary>
        public JsonValueKind? ValueKind
        {
            get
            {
                if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
                {
                    return change.ValueKind;
                }

                return _element.ValueKind;
            }
        }

        /// <summary>
        /// Gets the MutableJsonElement for the value of the property with the specified name.
        /// </summary>
        public MutableJsonElement GetProperty(string name)
        {
            return GetProperty(name.AsSpan());
        }

        /// <summary>
        /// Gets the MutableJsonElement for the value of the property with the specified name.
        /// </summary>
        public MutableJsonElement GetProperty(ReadOnlySpan<char> name)
        {
            if (!TryGetProperty(name, out MutableJsonElement value))
            {
                throw new InvalidOperationException($"'{_path}' does not contain property called '{GetString(name, 0, name.Length)}'");
            }

            return value;
        }

        /// <summary>
        /// Looks for a property named propertyName in the current object, returning a value that indicates whether or not such a property exists. When the property exists, its value is assigned to the value argument.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The value to assign to the element.</param>
        /// <returns></returns>
        public bool TryGetProperty(string name, out MutableJsonElement value)
        {
            return TryGetProperty(name.AsSpan(), out value);
        }

        /// <summary>
        /// Looks for a property named propertyName in the current object, returning a value that indicates whether or not such a property exists. When the property exists, its value is assigned to the value argument.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The value to assign to the element.</param>
        /// <returns></returns>
        public bool TryGetProperty(ReadOnlySpan<char> name, out MutableJsonElement value)
        {
            EnsureValid();

            EnsureObject();

            char[]? tempArray = null;
            int length = name.Length;
            length += _path.Length > 0 ? _path.Length + 1 : 0;
            Span<char> path = length <= MaxStackLimit ?
                stackalloc char[length] :
                tempArray = ArrayPool<char>.Shared.Rent(length);

            try
            {
                _path.AsSpan().CopyTo(path);
                int pathLength = _path.Length;

                MutableJsonDocument.ChangeTracker.PushProperty(path, ref pathLength, name);

                if (Changes.TryGetChange(path, _highWaterMark, out MutableJsonChange change))
                {
                    if (change.ChangeKind == MutableJsonChangeKind.PropertyRemoval)
                    {
                        value = default;
                        return false;
                    }

                    value = new MutableJsonElement(_root, change.GetSerializedValue(), GetString(path, 0, pathLength), change.Index);
                    return true;
                }

                bool hasProperty = _element.TryGetProperty(name, out JsonElement element);
                if (!hasProperty)
                {
                    value = default;
                    return false;
                }

                value = new MutableJsonElement(_root, element, GetString(path, 0, pathLength), _highWaterMark);
                return true;
            }
            finally
            {
                if (tempArray != null)
                {
                    ArrayPool<char>.Shared.Return(tempArray);
                }
            }
        }

        private static string GetString(ReadOnlySpan<char> value, int start, int end)
        {
#if NET5_0_OR_GREATER
            return new string(value.Slice(start, end));
#else
            return new string(value.Slice(start, end).ToArray());
#endif
        }

        public int GetArrayLength()
        {
            EnsureValid();

            EnsureArray();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                return change.GetSerializedValue().GetArrayLength();
            }

            return _element.GetArrayLength();
        }

        internal MutableJsonElement GetIndexElement(int index)
        {
            EnsureValid();

            EnsureArray();

            string path = MutableJsonDocument.ChangeTracker.PushIndex(_path, index);
            if (Changes.TryGetChange(path, _highWaterMark, out MutableJsonChange change))
            {
                return new MutableJsonElement(_root, change.GetSerializedValue(), path, change.Index);
            }

            return new MutableJsonElement(_root, _element[index], path, _highWaterMark);
        }

        /// <summary>
        /// Attempts to represent the current JSON number as a <see cref="double"/>.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/> if the number can be represented as a <see cref="double"/>,
        ///   <see langword="false"/> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        public bool TryGetDouble(out double value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case double d:
                        value = d;
                        return true;
                    case JsonElement element:
                        return element.TryGetDouble(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetDouble(out value);
                }
            }

            return _element.TryGetDouble(out value);
        }

        /// <summary>
        /// Gets the current JSON number as a <see cref="double"/>.
        /// </summary>
        /// <returns>The current JSON number as a <see cref="double"/>.</returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        /// <exception cref="FormatException">The value cannot be represented as a <see cref="double"/>.</exception>
        public double GetDouble()
        {
            if (!TryGetDouble(out double value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(double)));
            }

            return value;
        }

        private static string GetFormatExceptionText(string path, Type type)
        {
            return $"Element at '{path}' cannot be formatted as type '{type}.";
        }

        /// <summary>
        /// Attempts to represent the current JSON number as a <see cref="int"/>.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/> if the number can be represented as a <see cref="int"/>,
        ///   <see langword="false"/> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        public bool TryGetInt32(out int value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case int i:
                        value = i;
                        return true;
                    case JsonElement element:
                        return element.TryGetInt32(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetInt32(out value);
                }
            }

            return _element.TryGetInt32(out value);
        }

        /// <summary>
        /// Gets the current JSON number as a <see cref="int"/>.
        /// </summary>
        /// <returns>The current JSON number as a <see cref="int"/>.</returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        /// <exception cref="FormatException">The value cannot be represented as a <see cref="int"/>.</exception>
        public int GetInt32()
        {
            if (!TryGetInt32(out int value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(int)));
            }

            return value;
        }

        /// <summary>
        /// Attempts to represent the current JSON number as a <see cref="long"/>.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/> if the number can be represented as a <see cref="long"/>,
        ///   <see langword="false"/> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        public bool TryGetInt64(out long value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case long l:
                        value = l;
                        return true;
                    case JsonElement element:
                        return element.TryGetInt64(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetInt64(out value);
                }
            }

            return _element.TryGetInt64(out value);
        }

        /// <summary>
        /// Gets the current JSON number as a <see cref="long"/>.
        /// </summary>
        /// <returns>The current JSON number as a <see cref="long"/>.</returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        /// <exception cref="FormatException">The value cannot be represented as a <see cref="long"/>.</exception>
        public long GetInt64()
        {
            if (!TryGetInt64(out long value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(long)));
            }

            return value;
        }

        /// <summary>
        /// Attempts to represent the current JSON number as a <see cref="float"/>.
        /// </summary>
        /// <returns>
        ///   <see langword="true"/> if the number can be represented as a <see cref="float"/>,
        ///   <see langword="false"/> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        public bool TryGetSingle(out float value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case float f:
                        value = f;
                        return true;
                    case JsonElement element:
                        return element.TryGetSingle(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetSingle(out value);
                }
            }

            return _element.TryGetSingle(out value);
        }

        /// <summary>
        /// Gets the current JSON number as a <see cref="float"/>.
        /// </summary>
        /// <returns>The current JSON number as a <see cref="float"/>.</returns>
        /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.Number"/>.</exception>
        /// <exception cref="FormatException">The value cannot be represented as a <see cref="float"/>.</exception>
        public float GetSingle()
        {
            if (!TryGetSingle(out float value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(float)));
            }

            return value;
        }

        /// <summary>
        /// Gets the value of the element as a string.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string? GetString()
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case string s:
                        return s;
                    case JsonElement element:
                        return element.GetString();
                    case null:
                        return null;
                    default:
                        JsonElement el = change.GetSerializedValue();
                        if (el.ValueKind == JsonValueKind.String)
                        {
                            return el.GetString();
                        }
                        throw new InvalidOperationException($"Element at '{_path}' is not a string.");
                }
            }

            return _element.GetString();
        }

        /// <summary>
        /// Gets the value of the element as a bool.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool GetBoolean()
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                return change.Value switch
                {
                    bool b => b,
                    JsonElement element => element.GetBoolean(),
                    _ => throw new InvalidOperationException($"Element at '{_path}' is not a bool."),
                };
            }

            return _element.GetBoolean();
        }

        public bool TryGetByte(out byte value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case byte b:
                        value = b;
                        return true;
                    case JsonElement element:
                        return element.TryGetByte(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetByte(out value);
                }
            }

            return _element.TryGetByte(out value);
        }

        public byte GetByte()
        {
            if (!TryGetByte(out byte value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(byte)));
            }

            return value;
        }

        public bool TryGetDateTime(out DateTime value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case DateTime d:
                        value = d;
                        return true;
                    case JsonElement element:
                        return element.TryGetDateTime(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetDateTime(out value);
                }
            }

            return _element.TryGetDateTime(out value);
        }

        public DateTime GetDateTime()
        {
            if (!TryGetDateTime(out DateTime value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(DateTime)));
            }

            return value;
        }

        public bool TryGetDateTimeOffset(out DateTimeOffset value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case DateTimeOffset o:
                        value = o;
                        return true;
                        ;
                    case JsonElement element:
                        return element.TryGetDateTimeOffset(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetDateTimeOffset(out value);
                }
            }

            return _element.TryGetDateTimeOffset(out value);
        }

        public DateTimeOffset GetDateTimeOffset()
        {
            if (!TryGetDateTimeOffset(out DateTimeOffset value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(DateTimeOffset)));
            }

            return value;
        }

        public bool TryGetDecimal(out decimal value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case decimal d:
                        value = d;
                        return true;
                    case JsonElement element:
                        return element.TryGetDecimal(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetDecimal(out value);
                }
            }

            return _element.TryGetDecimal(out value);
        }

        public decimal GetDecimal()
        {
            if (!TryGetDecimal(out decimal value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(decimal)));
            }

            return value;
        }

        public bool TryGetGuid(out Guid value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case Guid g:
                        value = g;
                        return true;
                    case JsonElement element:
                        return element.TryGetGuid(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetGuid(out value);
                }
            }

            return _element.TryGetGuid(out value);
        }

        public Guid GetGuid()
        {
            if (!TryGetGuid(out Guid value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(Guid)));
            }

            return value;
        }

        public bool TryGetInt16(out short value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case short s:
                        value = s;
                        return true;
                    case JsonElement element:
                        return element.TryGetInt16(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetInt16(out value);
                }
            }

            return _element.TryGetInt16(out value);
        }

        public short GetInt16()
        {
            if (!TryGetInt16(out short value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(short)));
            }

            return value;
        }

        public bool TryGetSByte(out sbyte value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case sbyte b:
                        value = b;
                        return true;
                    case JsonElement element:
                        return element.TryGetSByte(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetSByte(out value);
                }
            }

            return _element.TryGetSByte(out value);
        }

        public sbyte GetSByte()
        {
            if (!TryGetSByte(out sbyte value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(sbyte)));
            }

            return value;
        }

        public bool TryGetUInt16(out ushort value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case ushort u:
                        value = u;
                        return true;
                    case JsonElement element:
                        return element.TryGetUInt16(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetUInt16(out value);
                }
            }

            return _element.TryGetUInt16(out value);
        }

        public ushort GetUInt16()
        {
            if (!TryGetUInt16(out ushort value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(ushort)));
            }

            return value;
        }

        public bool TryGetUInt32(out uint value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case uint d:
                        value = d;
                        return true;
                    case JsonElement element:
                        return element.TryGetUInt32(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetUInt32(out value);
                }
            }

            return _element.TryGetUInt32(out value);
        }

        public uint GetUInt32()
        {
            if (!TryGetUInt32(out uint value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(uint)));
            }

            return value;
        }

        public bool TryGetUInt64(out ulong value)
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                switch (change.Value)
                {
                    case ulong u:
                        value = u;
                        return true;
                    case JsonElement element:
                        return element.TryGetUInt64(out value);
                    case null:
                        value = default;
                        return false;
                    default:
                        return change.GetSerializedValue().TryGetUInt64(out value);
                }
            }

            return _element.TryGetUInt64(out value);
        }

        public ulong GetUInt64()
        {
            if (!TryGetUInt64(out ulong value))
            {
                throw new FormatException(GetFormatExceptionText(_path, typeof(ulong)));
            }

            return value;
        }

        /// <summary>
        /// Gets an enumerator to enumerate the values in the JSON array represented by this MutableJsonElement.
        /// </summary>
        public ArrayEnumerator EnumerateArray()
        {
            EnsureValid();

            EnsureArray();

            return new ArrayEnumerator(this);
        }

        /// <summary>
        /// Gets an enumerator to enumerate the properties in the JSON object represented by this JsonElement.
        /// </summary>
        public ObjectEnumerator EnumerateObject()
        {
            EnsureValid();

            EnsureObject();

            return new ObjectEnumerator(this);
        }

        /// <summary>
        /// Set the value of the property with the specified name to the passed-in value.  If the property is not already present, it will be created.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">The value to assign to the element.</param>
        public MutableJsonElement SetProperty(string name, object value)
        {
            if (TryGetProperty(name, out MutableJsonElement element))
            {
                element.Set(value);
                return this;
            }

#if !NET6_0_OR_GREATER
            // Earlier versions of JsonSerializer.Serialize include "RootElement"
            // as a property when called on JsonDocument.
            if (value is JsonDocument doc)
            {
                value = doc.RootElement;
            }
#endif

            // It is a new property.
            string path = MutableJsonDocument.ChangeTracker.PushProperty(_path, name);
            Changes.AddChange(path, GetSerializedValue(value), MutableJsonChangeKind.PropertyAddition, name);
            return this;
        }

        /// <summary>
        /// Remove the property with the specified name from the current MutableJsonElement.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveProperty(string name)
        {
            EnsureValid();

            EnsureObject();

            if (!_element.TryGetProperty(name, out _))
            {
                throw new InvalidOperationException($"Object does not have property: '{name}'.");
            }

            string path = MutableJsonDocument.ChangeTracker.PushProperty(_path, name);
            Changes.AddChange(path, null, changeKind: MutableJsonChangeKind.PropertyRemoval);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(double value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(int value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(long value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(float value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(string value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(bool value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(byte value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(sbyte value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(short value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(ushort value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(uint value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(ulong value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(decimal value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(Guid value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(DateTime value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(DateTimeOffset value)
        {
            EnsureValid();

            Changes.AddChange(_path, value);
        }

        /// <summary>
        /// Sets the value of this element to the passed-in value.
        /// </summary>
        /// <param name="value">The value to assign to the element.</param>
        public void Set(object value)
        {
            EnsureValid();

            switch (value)
            {
                case bool b:
                    Set(b);
                    break;
                case string s:
                    Set(s);
                    break;
                case byte b:
                    Set(b);
                    break;
                case sbyte sb:
                    Set(sb);
                    break;
                case short sh:
                    Set(sh);
                    break;
                case ushort us:
                    Set(us);
                    break;
                case int i:
                    Set(i);
                    break;
                case uint u:
                    Set(u);
                    break;
                case long l:
                    Set(l);
                    break;
                case ulong ul:
                    Set(ul);
                    break;
                case float f:
                    Set(f);
                    break;
                case double d:
                    Set(d);
                    break;
                case decimal d:
                    Set(d);
                    break;
                case DateTime d:
                    Set(d);
                    break;
                case DateTimeOffset d:
                    Set(d);
                    break;
                case Guid g:
                    Set(g);
                    break;
                case null:
                    Changes.AddChange(_path, null);
                    break;
                default:
                    Changes.AddChange(_path, GetSerializedValue(value));
                    break;
            }
        }

        private object GetSerializedValue(object value)
        {
            if (value is JsonDocument doc)
            {
                return doc.RootElement;
            }

            if (value is JsonElement element)
            {
                return element;
            }

            if (value is MutableJsonDocument mjd)
            {
                mjd.RootElement.EnsureValid();
            }

            if (value is MutableJsonElement mje)
            {
                mje.EnsureValid();
            }

            // If it's not a special type, we'll serialize it on assignment.
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, _root.SerializerOptions);
            return JsonDocument.Parse(bytes).RootElement;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                return change.AsString();
            }

            // Account for changes to descendants of this element as well
            if (Changes.DescendantChanged(_path, _highWaterMark))
            {
                return Encoding.UTF8.GetString(GetRawBytes());
            }

            return _element.ToString() ?? "null";
        }

        internal JsonElement GetJsonElement()
        {
            EnsureValid();

            if (Changes.TryGetChange(_path, _highWaterMark, out MutableJsonChange change))
            {
                return change.GetSerializedValue();
            }

            // Account for changes to descendants of this element as well
            if (Changes.DescendantChanged(_path, _highWaterMark))
            {
                JsonDocument document = JsonDocument.Parse(GetRawBytes());
                return document.RootElement;
            }

            return _element;
        }

        private byte[] GetRawBytes()
        {
            using MemoryStream changedElementStream = new();
            using (Utf8JsonWriter changedElementWriter = new(changedElementStream))
            {
                WriteTo(changedElementWriter);
            }

            return changedElementStream.ToArray();
        }

        internal static Utf8JsonReader GetReaderForElement(JsonElement element)
        {
            using MemoryStream stream = new();
            using (Utf8JsonWriter writer = new(stream))
            {
                element.WriteTo(writer);
            }

            return new Utf8JsonReader(stream.GetBuffer().AsSpan().Slice(0, (int)stream.Position));
        }

        internal void DisposeRoot()
        {
            _root.Dispose();
        }

        private void EnsureObject()
        {
            if (_element.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException($"Expected an 'Object' type but was '{_element.ValueKind}'.");
            }
        }

        private void EnsureArray()
        {
            if (_element.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException($"Expected an 'Array' type but was '{_element.ValueKind}'.");
            }
        }

        private void EnsureValid()
        {
            if (Changes.AncestorChanged(_path, _highWaterMark))
            {
                throw new InvalidOperationException("An ancestor node of this element has unapplied changes.  Please re-request this property from the RootElement.");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay => $"ValueKind = {ValueKind} : \"{ToString()}\"";

        private class MutableJsonElementConverter : JsonConverter<MutableJsonElement>
        {
            public override MutableJsonElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonDocument document = JsonDocument.ParseValue(ref reader);
                return new MutableJsonDocument(document, options).RootElement;
            }

            public override void Write(Utf8JsonWriter writer, MutableJsonElement value, JsonSerializerOptions options)
            {
                value.WriteTo(writer);
            }
        }
    }
}
