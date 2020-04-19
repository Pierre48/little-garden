using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace LittleGarden.Core.Entities
{
    public class Image : IEntity
    {
        public Byte[] Bytes {
            get;
            set;
        }

        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public byte[] Hash { get; set; }
    }

}