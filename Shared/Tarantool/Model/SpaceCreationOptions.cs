using System.Collections;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model
{
    public class SpaceCreationOptions
    {
        public bool Temporary { get; set; } = false;

        public uint Id { get; set; } = uint.MaxValue;

        public uint FieldCount { get; set; } = 0;

        public bool IfNotExists { get; set; } = false;

        public StorageEngine StorageEngine { get; set; } = StorageEngine.Memtx;

        public string User { get; set; } = string.Empty;

        public Hashtable Format { get; } = new Hashtable();
    }
}
