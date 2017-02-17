

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Resources;
using System.Reflection;

namespace QuranLibrary
{
    public class QuranLibrary
    {
        public QuranMetaData QuranMetaData { get; private set; }
        public SuraMetaData[] Surahs => QuranMetaData.SurahsContainer._suras;

        public QuranLibrary()
        {
            LoadData();
        }

        public void LoadData()
        {
            //string appFolder =  @"C:\Documents and Settings\ID50706.TRENTDRUGS\Desktop\Private\Quran APp\QuranClass\bin\Debug\";
            string appFolder = System.Environment.CurrentDirectory;            
            //using (StreamReader sr = new StreamReader(Path.Combine(appFolder, @"Data\quran-simple.xml")))
            //{
            //    XmlSerializer sSerializer = new XmlSerializer(typeof(QuranCollection));
            //    q = (QuranCollection)sSerializer.Deserialize(sr);
            //    //MessageBox.Show(q.surahs.Length.ToString());
             
            //}

            //ResourceManager resourceManager = new ResourceManager ("QuranLibrary.QuranLibrary", GetType ().Assembly);
            System.Reflection.Assembly asm = Assembly.GetExecutingAssembly();
            System.IO.Stream xmlStream = asm.GetManifestResourceStream("QuranLibrary.Data.quran-data.xml");

            //using (StreamReader sr = new StreamReader(Path.Combine(appFolder, Path.Combine("Data", "quran-data.xml"))))
            using (StreamReader sr = new StreamReader(xmlStream))
            {
                XmlSerializer sSerializer = new XmlSerializer(typeof(QuranMetaData));
                QuranMetaData = (QuranMetaData)sSerializer.Deserialize(sr);
                
            }
        }


    }
}
