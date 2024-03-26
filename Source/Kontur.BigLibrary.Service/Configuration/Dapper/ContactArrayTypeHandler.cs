using System.Data;
using Dapper;
using Kontur.BigLibrary.Service.Contracts;
using Newtonsoft.Json;

namespace Kontur.BigLibrary.Service.Configuration.Dapper
{
    public class ContactArrayTypeHandler: SqlMapper.TypeHandler<Contact[]>
    {
        public override void SetValue(IDbDataParameter parameter, Contact[] value)
        {
            parameter.Value = JsonConvert.SerializeObject(value, EntityTypeHandler.SerializerSettings);
        }

        public override Contact[] Parse(object value)
        {
            return JsonConvert.DeserializeObject<Contact[]>(value.ToString(), EntityTypeHandler.SerializerSettings);
        }
    }
}