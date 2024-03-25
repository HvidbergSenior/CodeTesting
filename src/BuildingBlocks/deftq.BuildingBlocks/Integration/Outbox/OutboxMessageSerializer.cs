using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deftq.BuildingBlocks.Integration.Outbox
{
    public class OutboxMessageSerializer
    {
        private readonly JsonSerializerSettings settings;

        public OutboxMessageSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new OutboxResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
        }

        public string Serialize(IntegrationEvent command, Type? type)
        {
            return JsonConvert.SerializeObject(command, type, settings);
        }

        public object? Deserialize(string payload, string messageType)
        {
            var type = Type.GetType(messageType);
            return JsonConvert.DeserializeObject(payload, type, settings);
        }
    }

    public class OutboxResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(p => base.CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                               .Select(f => base.CreateProperty(f, memberSerialization)))
                    .ToList();
            props?.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props!;
        }
    }
}
