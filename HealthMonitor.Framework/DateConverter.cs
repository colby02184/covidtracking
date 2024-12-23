﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthMonitor.Framework
{
    public class DateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                return ParseDate(dateString);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var dateNumber = reader.GetInt64();
                return ParseDate(dateNumber.ToString());
            }

            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyyMMdd"));
        }

        private static DateTime ParseDate(string dateString)
        {
            if (DateTime.TryParseExact(
                    dateString,
                    "yyyyMMdd",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            throw new JsonException($"Invalid date format: {dateString}");
        }
    }
}
