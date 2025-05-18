#if NANOFRAMEWORK_1_0
using System;
using System.IO;
#endif

namespace nanoFramework.Tarantool.Tests.Mocks
{
    internal class TestStream : MemoryStream
    {
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
