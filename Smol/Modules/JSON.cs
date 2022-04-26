using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class JSON
    {
        public static SmolValue JsonToSmol(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                return new SmolString(element.GetString());
            }
            else if (element.ValueKind == JsonValueKind.Object)
            {
                var obj = new SmolObject();

                foreach (var val in element.EnumerateObject())
                {
                    obj.Set(val.Name, JsonToSmol(val.Value));
                }

                return obj;
            }
            else if (element.ValueKind == JsonValueKind.Number)
            {
                return new SmolNumber(element.GetDouble());
            }
            else if (element.ValueKind == JsonValueKind.True)
            {
                return new SmolBoolean(true);
            }
            else if (element.ValueKind == JsonValueKind.False)
            {
                return new SmolBoolean(false);
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                return new SmolArray(element.EnumerateArray().Select(x => JsonToSmol(x)).ToArray());
            }
            else
            {
                throw new Exception("Cannot convert json to smol");
            }
        }

        public static JsonNode? SmolToJSON(SmolValue value)
        {
            if (value.IsString)
            {
                return JsonValue.Create(value.AsString());
            }
            else if (value.IsBoolean)
            {
                return JsonValue.Create(value.AsBoolean());
            }
            else if (value.IsNumber)
            {
                return JsonValue.Create(value.AsNumber());
            }
            else if (value.IsObject)
            {
                var smol = value.AsObject();

                JsonObject obj = new JsonObject();

                foreach (var v in smol.Data)
                {
                    obj.Add(v.Key, SmolToJSON(v.Value));
                }

                return obj;
            }
            else if (value.IsArray)
            {
                JsonArray array = new JsonArray();
                foreach (var v in value.AsArray())
                {
                    array.Add(SmolToJSON(v));
                }

                return array;
            }
            else
            {
                throw new Exception("Cannot convert smol to json");
            }
        }

        public static void Initialize(SmolContext context)
        {
            context.RegisterCommand("json_parse", (func, context) => {
                var value = context.Pop().AsString();

                var document = JsonDocument.Parse(value);

                context.PushValue(JsonToSmol(document.RootElement));
            });
            context.RegisterCommand("json_serialize", (func, context) => {
                var value = context.Pop().AsObject();

                var options = new JsonSerializerOptions() { 
                    WriteIndented = true,
                };

                context.PushValue(SmolToJSON(value).ToJsonString(options));
            });
        }
    }
}
