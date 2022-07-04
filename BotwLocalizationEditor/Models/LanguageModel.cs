using MsbtLib;
using Nintendo.Sarc;
using Syroot.BinaryData.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yaz0Library;

namespace BotwLocalizationEditor.Models
{
    public class LanguageModel
    {
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, MsbtEntry>>>> msbts;
        private readonly string loadFolder;

        public LanguageModel(string folder)
        {
            msbts = new();
            loadFolder = "";

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            loadFolder = folder;

            foreach (string file in Directory.GetFiles(folder, "Bootup_????.pack"))
            {
                string name = Path.GetFileNameWithoutExtension(file)[7..];
                msbts[name] = new();
                SarcFile msg = new(Yaz0.Decompress(new SarcFile(file).Files[$"Message/Msg_{name}.product.ssarc"]));
                foreach ((string msbtname, byte[] data) in msg.Files)
                {
                    string[] path = msbtname.Split('/');
                    msbts[name].TryAdd(path[0], new());
                    msbts[name][path[0]][path[1]] = new MSBT(data).GetTexts();
                }
            }
        }

        public string[] GetLangs()
        {
            return msbts.Keys.ToArray();
        }

        /*
         * One language
         */
        public SortedSet<string> GetOneLangMsbtFolders(string lang)
        {
            return new(msbts[lang].Keys);
        }

        public void AddMsbtFolderOneLang(string lang, string msbtFolder)
        {
            if (!msbts[lang].ContainsKey(msbtFolder))
            {
                msbts[lang][msbtFolder] = new();
            }
        }

        public SortedSet<string> GetOneLangMsbtNames(string lang, string msbtFolder)
        {
            return new(msbts[lang][msbtFolder].Keys);
        }

        public void AddMsbtOneLang(string lang, string msbtFolder, string msbtName)
        {
            if (!msbts[lang][msbtFolder].ContainsKey(msbtName))
            {
                msbts[lang][msbtFolder][msbtName] = new();
            }
        }

        public SortedSet<string> GetOneLangMsbtKeys(string lang, string msbtFolder, string msbtName)
        {
            return new(msbts[lang][msbtFolder][msbtName].Keys);
        }

        public void AddMsbtKeyOneLang(string lang, string msbtFolder, string msbtName, string key)
        {
            if (!msbts[lang][msbtName].ContainsKey(key))
            {
                msbts[lang][msbtFolder][msbtName][key] = new("", "");
            }
        }

        public string GetOneLangMsbtValue(string lang, string msbtFolder, string msbtName, string key)
        {
            return msbts[lang][msbtFolder][msbtName][key].Value;
        }

        public void SetOneLangMsbtValue(string lang, string msbtFolder, string msbtName, string key, string value)
        {
            msbts[lang][msbtFolder][msbtName][key].Value = value;
        }

        /*
         * Two languages
         */
        public SortedSet<string> GetTwoLangsMsbtFolders(string[] langs)
        {
            HashSet<string> names = new();
            foreach (string lang in langs)
            {
                foreach (string key in msbts[lang].Keys)
                {
                    names.Add(key);
                }
            }
            return new(names);
        }

        public void AddMsbtFolderTwoLangs(string[] langs, string msbtFolder)
        {
            AddMsbtFolderOneLang(langs[0], msbtFolder);
            AddMsbtFolderOneLang(langs[1], msbtFolder);
        }

        public SortedSet<string> GetTwoLangsMsbtNames(string[] langs, string msbtFolder)
        {
            HashSet<string> names = new();
            foreach (string lang in langs)
            {
                foreach (string key in msbts[lang][msbtFolder].Keys)
                {
                    names.Add(key);
                }
            }
            return new(names);
        }

        public void AddMsbtTwoLangs(string[] langs, string msbtFolder, string msbtName)
        {
            AddMsbtOneLang(langs[0], msbtFolder, msbtName);
            AddMsbtOneLang(langs[1], msbtFolder, msbtName);
        }

        public SortedSet<string> GetTwoLangsMsbtKeys(string[] langs, string msbtFolder, string msbtName)
        {
            HashSet<string> keys = new();
            foreach (string lang in langs)
            {
                foreach (string key in msbts[lang][msbtFolder][msbtName].Keys)
                {
                    keys.Add(key);
                }
            }
            return new(keys);
        }

        public void AddMsbtKeyTwoLangs(string[] langs, string msbtFolder, string msbtName, string key)
        {
            AddMsbtKeyOneLang(langs[0], msbtFolder, msbtName, key);
            AddMsbtKeyOneLang(langs[1], msbtFolder, msbtName, key);
        }

        public Dictionary<string, string> GetTwoLangsMsbtValues(string[] langs, string msbtFolder, string msbtName, string key)
        {
            return new()
            {
                { langs[0], msbts[langs[0]][msbtFolder][msbtName][key].Value },
                { langs[1], msbts[langs[1]][msbtFolder][msbtName][key].Value },
            };
        }

        public void SetTwoLangMsbtValues(string msbtFolder, string msbtName, string key, Dictionary<string, string> values)
        {
            foreach ((string lang, string value) in values)
            {
                msbts[lang][msbtFolder][msbtName][key].Value = value;
            }
        }

        /*
         * All languages
         */
        public SortedSet<string> GetAllLangsMsbtFolders()
        {
            HashSet<string> names = new();
            foreach (string lang in msbts.Keys)
            {
                foreach (string key in msbts[lang].Keys)
                {
                    names.Add(key);
                }
            }
            return new(names);
        }

        public void AddMsbtFolderAllLangs(string msbtFolder)
        {
            foreach (string lang in msbts.Keys)
            {
                AddMsbtFolderOneLang(lang, msbtFolder);
            }
        }

        public SortedSet<string> GetAllLangsMsbtNames(string msbtFolder)
        {
            HashSet<string> names = new();
            foreach (string name in msbts.Values.SelectMany(_ => _[msbtFolder].Keys))
            {
                names.Add(name);
            }
            return new(names);
        }

        public void AddMsbtAllLangs(string msbtFolder, string msbtName)
        {
            foreach (string lang in msbts.Keys)
            {
                AddMsbtOneLang(lang, msbtFolder, msbtName);
            }
        }

        public SortedSet<string> GetAllLangsMsbtKeys(string msbtFolder, string msbtName)
        {
            HashSet<string> keys = new();
            foreach (string key in msbts.Values.SelectMany(_ => _[msbtFolder][msbtName].Keys))
            {
                keys.Add(key);
            }
            return new(keys);
        }

        public void AddMsbtKeyAllLangs(string msbtFolder, string msbtName, string key)
        {
            foreach (string lang in msbts.Keys)
            {
                AddMsbtKeyOneLang(lang, msbtFolder, msbtName, key);
            }
        }

        public Dictionary<string, string> GetAllLangsMsbtValues(string msbtFolder, string msbtName, string key)
        {
            Dictionary<string, string> values = new();
            foreach (string lang in msbts.Keys)
            {
                values.Add(lang, msbts[lang][msbtFolder][msbtName][key].Value);
            }
            return values;
        }

        public void SetAllLangsMsbtValues(string msbtFolder, string msbtName, string key, Dictionary<string, string> values)
        {
            foreach ((string lang, string value) in values)
            {
                msbts[lang][msbtFolder][msbtName][key].Value = value;
            }
        }

        public void Save(string folder = "", bool be = false)
        {
            if (string.IsNullOrEmpty(folder))
            {
                folder = loadFolder;
            }
            Endian endian = be ? Endian.Big : Endian.Little;
            Endianness endianness = be ? Endianness.Big : Endianness.Little;

            foreach ((string lang, Dictionary<string, Dictionary<string, Dictionary<string, MsbtEntry>>> files) in msbts)
            {
                SarcFile msg = new(new(), endian);
                foreach ((string msbtFolder, Dictionary<string, Dictionary<string, MsbtEntry>> msbtNames) in files)
                {
                    foreach ((string msbtName, Dictionary<string, MsbtEntry> dict) in msbtNames)
                    {
                        MSBT msbt = new(endianness, UTFEncoding.UTF8);
                        msbt.CreateLbl1();
                        msbt.CreateAtr1();
                        msbt.CreateTxt2();
                        msbt.SetTexts(dict);
                        msg.Files[$"{msbtFolder}/{msbtName}"] = msbt.Write();
                    }
                }

                SarcFile bootup = new(new(), endian);
                bootup.Files[$"Message/Msg_{lang}.product.ssarc"] = Yaz0.Compress(msg.ToBinary());

                File.WriteAllBytes($"{folder}/Bootup_{lang}.pack", bootup.ToBinary());
            }
        }
    }
}
