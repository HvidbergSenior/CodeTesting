using Newtonsoft.Json;

namespace deftq.BuildingBlocks.Serialization
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings();
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            settings.ContractResolver = new PrivateResolver();
            settings.Converters.Add(new NewtonsoftDateOnlyJsonConverter());
        }

        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T? Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json, settings);

            return result ?? default;
        }
    }
}