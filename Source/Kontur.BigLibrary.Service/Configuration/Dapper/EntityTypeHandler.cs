using System.Data;
using Dapper;
using Kontur.BigLibrary.Service.Contracts.DataContracts;
using Newtonsoft.Json;

namespace Kontur.BigLibrary.Service.Configuration.Dapper
{
    public class EntityTypeHandler: SqlMapper.TypeHandler<Entity>
    {
        public static JsonSerializerSettings SerializerSettings { get; }

        static EntityTypeHandler()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
            };
        }

        public override void SetValue(IDbDataParameter parameter, Entity value)
        {
            parameter.Value = JsonConvert.SerializeObject(value, SerializerSettings);
        }

        public override Entity Parse(object value)
        {
            return JsonConvert.DeserializeObject<Entity>(value.ToString(), SerializerSettings);
        }
    }
}