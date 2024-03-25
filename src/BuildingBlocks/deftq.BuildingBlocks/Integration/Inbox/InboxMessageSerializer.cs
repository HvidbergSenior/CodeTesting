using System.Reflection;
using deftq.BuildingBlocks.Application.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deftq.BuildingBlocks.Integration.Inbox
{
    internal sealed class InboxMessageSerializer
    {
        private readonly JsonSerializerSettings settings;

        public InboxMessageSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new InboxResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
        }

        public string Serialize(IInternalCommand command, Type? type)
        {
            return JsonConvert.SerializeObject(command, type, settings);
        }

        public object? Deserialize(string payload, string messageType)
        {
            Type? type = Type.GetType(messageType);
            return JsonConvert.DeserializeObject(payload, type, settings);
        }
    }

    public class InboxResolver : DefaultContractResolver
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
