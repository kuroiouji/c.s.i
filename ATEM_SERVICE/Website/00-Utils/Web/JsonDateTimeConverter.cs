using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Web.Converter
{
    public class JsonDateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                if (objectType == typeof(DateTime))
                {
                    DateTime date = (DateTime)reader.Value;
                    if (date.Kind == DateTimeKind.Local)
                        return Utils.IOUtil.GetDateTimeTH(date);
                    else
                        return date;

                }
                else if (objectType == typeof(DateTime?))
                {
                    if (reader.Value != null)
                    {
                        DateTime date = ((DateTime?)reader.Value).Value;
                        if (date.Kind == DateTimeKind.Local)
                            return Utils.IOUtil.GetDateTimeTH(date);
                        else
                            return date;
                    }
                }
            }
            return null;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
}
