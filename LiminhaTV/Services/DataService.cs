using LiminhaTV.Data;
using LiminhaTV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

namespace LiminhaTV.Services
{
    public class DataService
    {
        public static async Task<List<Channel>> GetM3U(string url)
        {
            try
            {
                List<Channel> channels;

                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var stream = await httpClient.GetStreamAsync(url);
                    StreamReader reader = new StreamReader(stream);

                    channels = await Channels.Load(reader);
                }

                return channels;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task GetAndSaveEpg(string url, bool saveList = true)
        {
            try
            {

            List<ChProgram> list;

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                list = LoadStream(reader).ToList();
            }

            if (list == null || list.Count == 0)
                throw new Exception("Lista invalida");
                        
            var lista = new ListContainer { Type = ListType.EPG, Url = url };
            
            using (var db = new ChannelContext())
            {
                var all = from c in db.Programs select c;
                db.Programs.RemoveRange(all);
                db.Programs.AddRange(list);

                if (saveList)
                    db.ListContainer.Add(lista);

                db.SaveChanges();
            }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        static IEnumerable<ChProgram> LoadStream(
                    StreamReader stream)
        {

            //string XMLFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "webcsgo.xml");
            XDocument loadedData = XDocument.Load(stream);

            var data = from query in loadedData.Descendants("programme")
                       //select query;
                        select new ChProgram(query.Attribute("start").Value,
                      query.Attribute("stop").Value, query.Attribute("channel").Value,
                       query.Element("title").Value);

            return data;
        }
    }
}
