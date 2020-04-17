using Avro;
using Avro.Specific;

namespace LittleGarden.Core.Bus.Events
{
    public partial class ErrorEvent : ISpecificRecord
    {
        public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"ErrorEvent\",\"namespace\":\"LittleGarden.Core.Bus.Events\",\"field" +
                                                         "s\":[{\"name\":\"Name\",\"type\":\"string\"},{\"name\":\"Exception\",\"type\":\"string\"},{\"name\"" +
                                                         ":\"StackTrace\",\"type\":\"string\"}]}");
        private string _Name;
        private string _Exception;
        private string _StackTrace;
        public virtual Schema Schema
        {
            get
            {
                return ErrorEvent._SCHEMA;
            }
        }
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        public string Exception
        {
            get
            {
                return this._Exception;
            }
            set
            {
                this._Exception = value;
            }
        }
        public string StackTrace
        {
            get
            {
                return this._StackTrace;
            }
            set
            {
                this._StackTrace = value;
            }
        }
        public virtual object Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.Name;
                case 1: return this.Exception;
                case 2: return this.StackTrace;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
            };
        }
        public virtual void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.Name = (System.String)fieldValue; break;
                case 1: this.Exception = (System.String)fieldValue; break;
                case 2: this.StackTrace = (System.String)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            };
        }
    }
}