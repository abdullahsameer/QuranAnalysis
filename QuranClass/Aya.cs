

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QuranLibrary
{
    [XmlType("aya")]
    public class Aya
    {
        [XmlAttribute]
        public int index;

        [XmlAttribute]
        public string text;
    }
}
