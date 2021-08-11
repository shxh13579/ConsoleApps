using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestApp.Model
{
    public class TestModel
    {
        /// <summary>
        /// test key
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// from database's field,named 'Type'.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// from database's field,named 'Value'.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ViewField { get; set; }


        public string BackupData { get; set; }
    }
}
