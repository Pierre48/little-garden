using System;
using Avro;
using Avro.Specific;

namespace LittleGarden.Core.Bus.Events
{
    public partial class ImageEvent : IEvent
    {
        public Byte[] Bytes {
            get;
            set;
        }

        public string Name { get; set; }
        public byte[] Hash { get; set; }
        public byte[] ThumbBytes { get; set; }

        /// TODO Not manage in avro
    }
}