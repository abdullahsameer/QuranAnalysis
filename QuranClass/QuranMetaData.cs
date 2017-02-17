

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;
using System.IO;

namespace QuranLibrary
{

    [XmlRoot("quran")]
    public class QuranMetaData
    {
        //QuranCollection q;
        [XmlElement("pages")]
        public PagesContainer PagesContainer { get; set; }
        public QuranMetaData()
        {
        }


        [XmlElement("suras")]
        public SurahsContainer SurahsContainer { get; set; }

        public string[] SurahNames()
        {
            var surahNames = new string[114];
            for (int i = 0; i < 114; i++)
                surahNames[i] = SurahsContainer._suras[i].tname;
            return surahNames;

        }
        
    }

    [XmlType("pages")]
    public class PagesContainer
    {
        [XmlElement("page")]
        public Page[] _pages;

        public Page[] Pages => _pages;
    }

    [XmlType("page")]
    public class Page
    {
        //	<page index="1" sura="1" aya="1"/>
        [XmlAttribute]
        public int index, sura, aya;
    }

    [XmlType("suras")]
    public class SurahsContainer
    {
        [XmlElement("sura")]
        public SuraMetaData[] _suras;

    }

    [XmlType("sura")]
    public class SuraMetaData
    {
        public override int GetHashCode()
        {
            unchecked
            {
                int result = SurahNumber.GetHashCode();
                result = (result * 397) ^ AyahCount.GetHashCode() ;
                return result;
            }
        }

        public int SurahNumber => index;
        public int MagicNumber => SurahNumber + AyahCount;

        public int AyahCount
        {
            get { return ayas; }
            set { ayas = value; }
        }

        [XmlAttribute]
        public int start, ayas, order, rukus, index;

        [XmlAttribute]
        public string name, tname, ename, type;
    }


    public class Juz
    {
        int sura, ayah;
    }

    public class HizbQaurter
    {
        int sura, ayah;
    }


}
