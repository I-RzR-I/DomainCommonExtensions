using System.Runtime.Serialization;

namespace DataTypeTests.TestHelpers
{
    [DataContract]
    public sealed class XmlDocModelTestHelper
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
