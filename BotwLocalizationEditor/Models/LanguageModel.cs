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

        public LanguageModel(string folder)
        {
            msbts = new();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

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
        private string[]? langs;
        public string[] GetLangs() => langs ??= msbts.Keys.ToArray();
        private SortedSet<string>? sortedLangs;
        public SortedSet<string> GetSortedLangs() => sortedLangs ??= new(GetLangs());

        private string[]? msbtFolders;
        public string[] GetMsbtFolders() => msbtFolders ??= msbts.First().Value.Keys.ToArray();
        private SortedSet<string>? sortedMsbtFolders;
        public SortedSet<string> GetSortedMsbtFolders() => sortedMsbtFolders ??= new(GetMsbtFolders());

        /*
         * One language
         */
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
            if (!msbts[lang][msbtFolder].ContainsKey(msbtName)) return new();
            return new(msbts[lang][msbtFolder][msbtName].Keys);
        }

        public void AddMsbtKeyOneLang(string lang, string msbtFolder, string msbtName, string key)
        {
            if (!msbts[lang][msbtFolder][msbtName].ContainsKey(key))
            {
                msbts[lang][msbtFolder][msbtName][key] = new("", "");
            }
        }

        public string GetOneLangMsbtValue(string lang, string msbtFolder, string msbtName, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            if (!msbts[lang][msbtFolder].ContainsKey(msbtName))
            {
                msbts[lang][msbtFolder][msbtName] = new();
            }
            if (!msbts[lang][msbtFolder][msbtName].ContainsKey(key))
            {
                msbts[lang][msbtFolder][msbtName][key] = new("", "");
            }
            return msbts[lang][msbtFolder][msbtName][key].Value;
        }

        public void SetOneLangMsbtValue(string lang, string msbtFolder, string msbtName, string key, string value)
        {
            msbts[lang][msbtFolder][msbtName][key].Value = value;
        }

        /*
         * All languages
         */
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
            if (!msbts.Any(l => l.Value[msbtFolder].ContainsKey(msbtName))) return new();
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
                values.Add(lang, GetOneLangMsbtValue(lang, msbtFolder, msbtName, key));
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

        public void Save(string folder, bool be = false)
        {
            Endian endian = be ? Endian.Big : Endian.Little;
            Endianness endianness = be ? Endianness.Big : Endianness.Little;

            foreach ((string lang, Dictionary<string, Dictionary<string, Dictionary<string, MsbtEntry>>> files) in msbts)
            {
                SarcFile msg = new(new(), endian);
                foreach ((string msbtFolder, Dictionary<string, Dictionary<string, MsbtEntry>> msbtNames) in files)
                {
                    foreach ((string msbtName, Dictionary<string, MsbtEntry> dict) in msbtNames)
                    {
                        MSBT msbt = new(endianness, UTFEncoding.UTF16);
                        msbt.CreateLbl1();
                        msbt.CreateAtr1();
                        msbt.CreateTxt2();
                        msbt.SetTexts(dict);
                        msg.Files[$"{msbtFolder}/{msbtName}"] = msbt.Write();
                    }
                }

                SarcFile bootup = new(new(), endian);
                bootup.Files[$"Message/Msg_{lang}.product.ssarc"] = Yaz0.Compress(msg.ToBinary());

                File.WriteAllBytes($"{folder}/content/Pack/Bootup_{lang}.pack", bootup.ToBinary());
            }
        }

        private Dictionary<string, Dictionary<string, HashSet<string>>> MergeAllLanguageFiles()
        {
            Dictionary<string, Dictionary<string, HashSet<string>>> allKeys = new();
            foreach (var folderSet in msbts.Values)
            {
                foreach ((string folder, var fileSet) in folderSet)
                {
                    if (!allKeys.ContainsKey(folder))
                    {
                        allKeys[folder] = new();
                    }
                    foreach ((string file, var keySet) in fileSet)
                    {
                        if (!allKeys[folder].ContainsKey(file))
                        {
                            allKeys[folder][file] = new();
                        }
                        foreach ((string key, MsbtEntry value) in keySet)
                        {
                            if (!string.IsNullOrEmpty(value.Value))
                            {
                                allKeys[folder][file].Add(key);
                            }
                        }
                    }
                }
            }
            return allKeys;
        }

        /// <summary>
        /// Finds all missing or empty keys from all languages. Something is defined as "missing" if even one other language has it.
        /// </summary>
        /// <returns>
        /// Dictionary of lang, folder, file, key, bool, where the final bools are false if missing, true if empty
        /// </returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> FindMissing()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> ret = new();
            var allKeys = MergeAllLanguageFiles();

            foreach ((string folder, var fileSet) in allKeys)
            {
                foreach ((string lang, var msbtSet) in msbts)
                {
                    if (!msbtSet.ContainsKey(folder))
                    {
                        if (!ret.ContainsKey(lang))
                        {
                            ret[lang] = new();
                        }
                        ret[lang][folder] = new();
                        foreach ((string file, var keySet) in allKeys[folder])
                        {
                            ret[lang][folder][file] = keySet.ToDictionary(k => k, k => false);
                        }
                    }
                    foreach ((string file, var keySet) in fileSet)
                    {
                        if (!msbtSet[folder].ContainsKey(file))
                        {
                            if (!ret.ContainsKey(lang))
                            {
                                ret[lang] = new();
                            }
                            if (!ret[lang].ContainsKey(folder))
                            {
                                ret[lang][folder] = new();
                            }
                            ret[lang][folder][file] = keySet.ToDictionary(k => k, k => false);
                        }
                        foreach (string key in keySet)
                        {
                            bool missing = false;
                            bool missType = false;
                            if (!msbtSet[folder][file].ContainsKey(key))
                            {
                                missing = true;
                            }
                            else if (string.IsNullOrEmpty(msbtSet[folder][file][key].Value))
                            {
                                missing = true;
                                missType = true;
                            }
                            if (missing)
                            {
                                if (!ret.ContainsKey(lang))
                                {
                                    ret[lang] = new();
                                }
                                if (!ret[lang].ContainsKey(folder))
                                {
                                    ret[lang][folder] = new();
                                }
                                if (!ret[lang][folder].ContainsKey(file))
                                {
                                    ret[lang][folder][file] = new();
                                }
                                ret[lang][folder][file][key] = missType;
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
