
#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

#endregion

namespace LiminhaTV.Models
{

    // M3U.NET   
    public class Channels
    {
        private static readonly List<Channel> _entries = new List<Channel>();

        private const string c_name = "tvg-name";
        private const string c_logo = "tvg-logo";
        private const string c_group = "group-title";


        private static Match getPropertyValue(string line, string value)
        {
            return Regex.Match(line, string.Format("(?<={0}=\")(.)+?(?=\")", value));
        }

        public static async Task<List<Channel>> Load(StreamReader reader)
        {

            try
            {

                _entries.Clear();

                string line;
                var lineCount = 0;

                Channel entry = null;
                string logo = null, name, group = null;

                while ((line = reader.ReadLine()) != null)
                {
                    // if (lineCount == 0 && line != "#EXTM3U")
                    //     throw new ChException("M3U header is missing.");

                    if (line.StartsWith("#EXTINF:"))
                    {
                        if (entry != null)
                            throw new ChException("Unexpected entry detected.");

                        var split = line.Substring(8, line.Length - 8).Split(new[] { ',' }, 2);

                        Match match_name = getPropertyValue(line, c_name);

                        if (match_name.Success)
                        {
                            name = match_name.Value;
                        }
                        else
                        {
                            if (split.Length < 2) {
                                //  throw new ChException("Invalid file!");
                                name = "Nome Indefinido";
                            } else

                            name = split[1];
                        }

                        Match match_logo = getPropertyValue(line, c_logo);

                        if (match_logo.Success)
                        {
                            logo = match_logo.Value;
                        }

                        Match match_group = getPropertyValue(line, c_group);

                        if (match_group.Success)
                        {
                            group = match_group.Value;
                        }

                        entry = new Channel(name, logo, group);
                    }

                    else if (entry != null && !line.StartsWith("#"))
                    {
                        Uri path;
                        if (Uri.TryCreate(line, UriKind.RelativeOrAbsolute, out path))
                        {
                            //  throw new ChException("Invalid entry path.");

                            entry.Path = path.ToString();
                            _entries.Add(entry);
                        }

                        entry = null;
                    }

                    lineCount++;
                }

                return _entries;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
