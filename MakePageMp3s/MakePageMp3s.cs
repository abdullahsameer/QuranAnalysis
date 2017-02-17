

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuranLibrary;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace MakePageMp3s
{
    /// <summary>
    /// Tool to make page mp3s (generate list of ayat on a page)
    /// </summary>
    class MakePageMp3s
    {
        /*
    * example info
    Page 33: Ayah 211 Index 33 Surah 2
    Page 34: Ayah 216 Index 34 Surah 2
    Page 35: Ayah 220 Index 35 Surah 2
    Page 36: Ayah 225 Index 36 Surah 2
    Page 37: Ayah 231 Index 37 Surah 2
    Page 38: Ayah 234 Index 38 Surah 2
    Page 39: Ayah 238 Index 39 Surah 2
         
    Page 600: 100:10 to 100:11, then 101:01 to 101:11, followed by 102:01 to 102:08

    Update Nov-3-2010 Fixing bugs where pages not generated properly if new surah starts on the page
         * Also add bismillah before next surah
 */
        //next step would be to create for ALL based on list of  recitations (Sudais, Shuraim ,etc) and also add Id3 tags

        string _path = "";

        QuranLibrary.QuranLibrary library = new QuranLibrary.QuranLibrary();
        public MakePageMp3s()
        {
        }

        public void Execute(Recitation recitation)
        {
            _path = recitation.subfolder;
            int i = 0;
            int startSurah = 1, startAyah = 1, endSurah, endAyah;
            do
            {
                Page p;
                if (i == 604) //exception for last page
                {
                    endSurah = 114;
                    endAyah = 6;
                    p = library.QuranMetaData.PagesContainer.Pages[i - 1];
                }
                else
                {
                    p = library.QuranMetaData.PagesContainer.Pages[i];
                    endSurah = p.sura;
                    endAyah = p.aya - 1;
                }

                if (i == 0)
                {
                    i++;
                    continue;

                }
                else if (i == 1) //exception for page 1
                {
                    endSurah = 1;
                    endAyah = 7;
                }

                if (endAyah == 0)
                {
                    endAyah = library.QuranMetaData.SurahsContainer._suras[p.sura - 2].ayas;
                    endSurah = p.sura - 1;
                }

                Debug.WriteLine(string.Format("Page {3}: Ayah {0} Index {1} Surah {2}", p.aya, p.index, p.sura, i));
                Debug.WriteLine(string.Format("Page {4}: From {0}:{1} to {2}:{3}", startSurah, startAyah, endSurah, endAyah, i));
                string pageMp3Name = "Page" + i.ToString().PadLeft(3, '0') + ".mp3";
                var mp3sToWrapArray = Mp3List(startSurah, startAyah, endSurah, endAyah);

                StringBuilder mp3listSpaceDelimited = new StringBuilder(mp3sToWrapArray.Length);
                //mp3listSpaceDelimited.AppendFormat("{0} ", pageMp3Name);
                if (!Directory.Exists(Path.Combine(_path,"PageMp3s")))
                    Directory.CreateDirectory(Path.Combine(_path,"PageMp3s"));

                foreach (string mp3 in mp3sToWrapArray)
                {
                    mp3listSpaceDelimited.AppendFormat("{0}|", mp3);
                }
                string pageFilenameWithPath = Path.Combine(Path.Combine(_path, "PageMp3s"), pageMp3Name);
                //disabled mpgtx to try ffmpeg instead.
                //string joinCmd = string.Format("-f -j -o {0} {1}", pageFilenameWithPath, mp3listSpaceDelimited.ToString());
                //Console.WriteLine(joinCmd);
                //Process process = System.Diagnostics.Process.Start(new ProcessStartInfo("mpgtx", joinCmd) { UseShellExecute = false });
                //requires ffmpeg version N-44317-g2474ca1 Copyright (c) 2000-2012 the FFmpeg developers
                const string cmd = "ffmpeg";
                //-n do not overwrite -y overwrite
                string joinCmd = string.Format("-n -i concat:{1} -acodec copy {0}", pageFilenameWithPath, mp3listSpaceDelimited.ToString());
                Console.WriteLine(joinCmd);
                Process process = System.Diagnostics.Process.Start(new ProcessStartInfo(cmd, joinCmd) { UseShellExecute = false });
                process.WaitForExit();
                //System.Threading.Thread.Sleep(250);
                string tagCmd = string.Format("set \"%A:{0} %a:(C) VerseByVerseQuran PageMp3s %t:Page {1}\" {2}", recitation.name, i.ToString().PadLeft(3, '0'), pageFilenameWithPath);
                Console.WriteLine(tagCmd);
                process = System.Diagnostics.Process.Start(new ProcessStartInfo("tagmp3", tagCmd) { UseShellExecute = false });
                process.WaitForExit();
                //System.Threading.Thread.Sleep(250);
                //AppDomain mp3wrap = AppDomain.CreateDomain("mp3wrapdomain");                
                //mp3wrap.ExecuteAssembly("mp3wrap.exe", new System.Security.Policy.Evidence(), args);                
                //var proc = Process.Start(new ProcessStartInfo("3rdParty/mp3wrap.exe", mp3listSpaceDelimited.ToString()) { WindowStyle = ProcessWindowStyle.Hidden });
                //proc.WaitForExit();
                //Console.ReadKey();
                startSurah = p.sura;
                startAyah = p.aya;
                ++i;

            } while (i <= library.QuranMetaData.PagesContainer._pages.Length);
            //also zip them up
            //one more time 
            //Console.ReadKey();
        }



        public static List<Recitation> GetRecitations()
        {
            System.Net.WebClient Client = new WebClient();
            string strRecitations = Client.DownloadString("http://www.everyayah.com/data/recitationsInXml.php");            
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            //Dictionary<string, object> obj = (Dictionary<string, object>)serializer.DeserializeObject(recitationsJs);
            
            //Dictionary<string, Dictionary<string, string>> recitations;

            //var result = from o in obj.Skip(1)
            //             select recitations = ((Dictionary<string,Dictionary<string,string>>)o.Value)
            //                 ;

            //foreach (Dictionary<string, string> vals in recitations.Values)
            //{

            //}
            XElement recitations = XElement.Parse (strRecitations);
            var folders = new List<Recitation>();
            foreach (XElement child in recitations.Elements())
            {
                folders.Add(new Recitation() { subfolder = child.Element("subfolder").Value, bitrate = child.Element("bitrate").Value, name = child.Element("name").Value });
            }

            return folders;
            //XPathDocument
            //XmlDocument x = new XmlDocument();
            //x.LoadXml(strRecitations);
            //XmlNodeList l = x.SelectNodes("/Recitations/Recitation");
            //var result = from xmlnode in x 
            //             select x.SelectNodes("/Recitations/Recitation");
            //foreach (XmlNode node in )
            //{
                
            //}


                              


        }

        /// <summary>
        /// grab list of start and end ayahs for each surah
        /// </summary>
        /// <param name="startSurah"></param>
        /// <param name="startAyah"></param>
        /// <param name="endSurah"></param>
        /// <param name="endAyah"></param>
        /// <returns></returns>
        private string[] Mp3List(int startSurah, int startAyah, int endSurah, int endAyah)
        {
            //TEST case
            //Page 600: start surah 100, start ayah 10, end surah 102, end ayah 8 ( 100:10 to 100:11, then 101:01 to 101:11, followed by 102:01 to 102:08)

            //const string path = @"D:\Quran\Abdul_Basit_Murattal\out\";
            var c = new List<string>();
            for (int surah = startSurah; surah <= endSurah; surah++ )
            {
                int finalAyah = endAyah;

                //BUG caught nov-10-2010: if surah is not startsurah, reset ayah counter back to 0 to add bismillah
                if (surah != startSurah)
                    startAyah = 0; 


                //if (surah == endSurah)
                //    finalAyah = endAyah;
                //else 
                if (surah < endSurah) //if this surah is not the last on the page, then all the ayahs will be used up until end of this surah
                    finalAyah = library.QuranMetaData.SurahsContainer._suras[surah-1].ayas;

                for (int ayah = startAyah; ayah <= finalAyah; ayah++)
                {
                    if (ayah == 0)
                    {
                        c.Add( Path.Combine(_path , "001001.mp3")); //Add Bismillah
                    }
                    else
                        c.Add( Path.Combine( _path , surah.ToString().PadLeft(3, '0') + ayah.ToString().PadLeft(3, '0') + ".mp3"));
                }
            }
            if (startSurah == 100)
                System.Threading.Thread.Sleep(1);
            return c.ToArray();
        }
    }
}
