

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QuranLibrary
{
    [XmlRoot("quran")]
    public class QuranCollection
    {
        [XmlElement("sura")]
        public Sura[] surahs;

    }
}
