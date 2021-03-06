using MessagePack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Surging.Core.Codec.MessagePack.Utilities;
using System;

namespace Surging.Core.Codec.MessagePack.Messages
{
    [MessagePackObject]
    public class DynamicItem
    {
        #region Constructor

        public DynamicItem()
        { }

        public DynamicItem(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var valueType = value.GetType();
            var code = Type.GetTypeCode(valueType);

            if (code != TypeCode.Object)
                TypeName = valueType.FullName;
            else
                TypeName = valueType.AssemblyQualifiedName;

            if (valueType == typeof(JObject))
                Content = SerializerUtilitys.Serialize(value.ToString());
            else
                Content = SerializerUtilitys.Serialize(value);
        }

        #endregion Constructor

        #region Property

        [Key(0)]
        public string TypeName { get; set; }

        [Key(1)]
        public byte[] Content { get; set; }
        #endregion Property

        #region Public Method
        public object Get()
        {
            if (Content == null || TypeName == null)
                return null;

            var typeName = Type.GetType(TypeName);
            if (typeName == typeof(JObject))
            {
                var content = SerializerUtilitys.Deserialize<string>(Content);
                return JsonConvert.DeserializeObject<JObject>(content);
            }
            else
            {
                return SerializerUtilitys.Deserialize(Content, typeName);
            }
        }
        #endregion Public Method
    }
}
