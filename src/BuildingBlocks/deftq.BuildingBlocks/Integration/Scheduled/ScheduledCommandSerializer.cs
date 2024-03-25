using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deftq.BuildingBlocks.Integration.Scheduled
{
    internal sealed class ScheduledCommandSerializer
    {
        private readonly JsonSerializerSettings settings;

        public ScheduledCommandSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new ScheduledCommandResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
        }

        public string Serialize(IScheduledCommand command, Type? type)
        {
            return JsonConvert.SerializeObject(command, type, settings);
        }

        public object? Deserialize(string payload, string messageType)
        {
            var type = Type.GetType(messageType);
            return JsonConvert.DeserializeObject(payload, type, settings);
        }
    }

    public class ScheduledCommandResolver : DefaultContractResolver
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
