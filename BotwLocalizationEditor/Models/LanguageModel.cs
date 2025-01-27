using BotwLocalizationEditor.ViewModels;
using MsbtLib;
using Revrs;
using SarcLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsYaz0;
using Endianness = Revrs.Endianness;

namespace BotwLocalizationEditor.Models
{
    public class LanguageModel
    {
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, MsbtEntry>>>> msbts;

        public LanguageModel(string folder)
        {
            msbts = [];

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }
            
            Endianness endianness = folder.Contains("romfs") ? Endianness.Little : Endianness.Big;

            foreach (string file in Directory.GetFiles(folder, "Bootup_????.pack"))
            {
                string name = Path.GetFileNameWithoutExtension(file)[7..];
                msbts[name] = [];
                RevrsReader reader = new(File.ReadAllBytes(file), endianness);
                ImmutableSarc sarc = new(ref reader);
                RevrsReader reader1 = new(Yaz0.Decompress(sarc[$"Message/Msg_{name}.product.ssarc"].Data), endianness);
                ImmutableSarc msg = new(ref reader1);
                foreach ((string msbtname, Span<byte> data) in msg)
                {
                    string[] path = msbtname.Split('/');
                    msbts[name].TryAdd(path[0], []);
                    msbts[name][path[0]][path[1]] = new MSBT(data.ToArray()).GetTexts();
                }
            }
        }
        private string[]? langs;
        public string[] GetLangs() => langs ??= [.. msbts.Keys];
        private SortedSet<string>? sortedLangs;
        public SortedSet<string> GetSortedLangs() => sortedLangs ??= [.. GetLangs()];

        private string[]? msbtFolders;
        public string[] GetMsbtFolders() => msbtFolders ??= [.. msbts.First().Value.Keys];
        private SortedSet<string>? sortedMsbtFolders;
        public SortedSet<string> GetSortedMsbtFolders() => sortedMsbtFolders ??= [.. GetMsbtFolders()];

        /*
         * One language
         */
        public SortedSet<string> GetOneLangMsbtNames(string lang, string msbtFolder)
        {
            return [.. msbts[lang][msbtFolder].Keys];
        }

        public void AddMsbtOneLang(string lang, string msbtFolder, string msbtName)
        {
            if (!msbts[lang][msbtFolder].ContainsKey(msbtName))
            {
                msbts[lang][msbtFolder][msbtName] = [];
            }
        }

        public SortedSet<string> GetOneLangMsbtKeys(string lang, string msbtFolder, string msbtName)
        {
            if (!msbts[lang][msbtFolder]
                .TryGetValue(msbtName, out Dictionary<string, MsbtEntry>? value))
            {
                return [];
            }
            return [.. value.Keys];
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
                msbts[lang][msbtFolder][msbtName] = [];
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
            HashSet<string> names = [.. msbts.Values.SelectMany(_ => _[msbtFolder].Keys)];
            return [.. names];
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
            if (!msbts.Any(l => l.Value[msbtFolder].ContainsKey(msbtName))) return [];
            HashSet<string> keys = [.. msbts.Values.SelectMany(_ => _[msbtFolder][msbtName].Keys)];
            return [.. keys];
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
            Dictionary<string, string> values = [];
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
            Endianness endian = be ? Endianness.Big : Endianness.Little;
            MsbtLib.Endianness endianness = be ? MsbtLib.Endianness.Big : MsbtLib.Endianness.Little;

            foreach ((string lang, Dictionary<string, Dictionary<string, Dictionary<string, MsbtEntry>>> files) in msbts)
            {
                Sarc msg = new() { Endianness = endian };
                foreach ((string msbtFolder, Dictionary<string, Dictionary<string, MsbtEntry>> msbtNames) in files)
                {
                    foreach ((string msbtName, Dictionary<string, MsbtEntry> dict) in msbtNames)
                    {
                        MSBT msbt = new(endianness, UTFEncoding.UTF16);
                        msbt.CreateLbl1();
                        msbt.CreateAtr1();
                        msbt.CreateTxt2();
                        msbt.SetTexts(dict);
                        msg[$"{msbtFolder}/{msbtName}"] = msbt.Write();
                    }
                }

                Sarc bootup = new() { Endianness = endian };
                using (MemoryStream stream = new())
                {
                    msg.Write(stream);
                    bootup[$"Message/Msg_{lang}.product.ssarc"] = Yaz0.Compress(
                        new ReadOnlySpan<byte>(stream.GetBuffer())[..(int)stream.Length]
                    ).ToArray();
                }

                using (MemoryStream stream = new())
                {
                    bootup.Write(stream);
                    File.WriteAllBytes($"{folder}/Bootup_{lang}.pack", stream.GetBuffer());
                }
            }
        }

        private Dictionary<string, Dictionary<string, HashSet<string>>> MergeAllLanguageFiles()
        {
            Dictionary<string, Dictionary<string, HashSet<string>>> allKeys = [];
            foreach (var folderSet in msbts.Values)
            {
                foreach ((string folder, var fileSet) in folderSet)
                {
                    if (!allKeys.ContainsKey(folder))
                    {
                        allKeys[folder] = [];
                    }
                    foreach ((string file, var keySet) in fileSet)
                    {
                        if (!allKeys[folder].ContainsKey(file))
                        {
                            allKeys[folder][file] = [];
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
            return FindMissing(MergeAllLanguageFiles());
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> FindMissing(Dictionary<string, Dictionary<string, HashSet<string>>> allKeys)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>> ret = [];

            foreach ((string folder, var fileSet) in allKeys)
            {
                foreach ((string lang, var msbtSet) in msbts)
                {
                    if (!msbtSet.ContainsKey(folder))
                    {
                        if (!ret.TryGetValue(lang, out Dictionary<string, Dictionary<string, Dictionary<string, bool>>>? value))
                        {
                            value = [];
                            ret[lang] = value;
                        }

                        value[folder] = [];
                        foreach ((string file, var keySet) in allKeys[folder])
                        {
                            value[folder][file] = keySet.ToDictionary(k => k, k => false);
                        }
                    }
                    foreach ((string file, var keySet) in fileSet)
                    {
                        if (!msbtSet[folder].ContainsKey(file))
                        {
                            if (!ret.ContainsKey(lang))
                            {
                                ret[lang] = [];
                            }
                            if (!ret[lang].ContainsKey(folder))
                            {
                                ret[lang][folder] = [];
                            }
                            ret[lang][folder][file] = keySet.ToDictionary(k => k, k => false);
                        }
                        foreach (string key in keySet)
                        {
                            bool missing = false;
                            bool missType = false;
                            if (!msbtSet[folder][file].TryGetValue(key, out MsbtEntry? value))
                            {
                                missing = true;
                            }
                            else if (string.IsNullOrEmpty(value.Value))
                            {
                                missing = true;
                                missType = true;
                            }
                            if (missing)
                            {
                                if (!ret.ContainsKey(lang))
                                {
                                    ret[lang] = [];
                                }
                                if (!ret[lang].ContainsKey(folder))
                                {
                                    ret[lang][folder] = [];
                                }
                                if (!ret[lang][folder].ContainsKey(file))
                                {
                                    ret[lang][folder][file] = [];
                                }
                                ret[lang][folder][file][key] = missType;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, bool>>> FindNew()
        {
            LanguageModel vanilla = new(Path.Combine(SettingsViewModel.GetDumpPath(), "content", "Pack"));
            return vanilla.FindMissing(MergeAllLanguageFiles()).First().Value;
        }
    }
}
