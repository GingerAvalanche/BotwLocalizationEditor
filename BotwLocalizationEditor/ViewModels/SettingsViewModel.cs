using Avalonia;
using Avalonia.Styling;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReactiveUI;
using System;
using System.IO;

namespace BotwLocalizationEditor.ViewModels
{
    public enum InstallType
    {
        Permanent,
        Portable
    }

    public enum Theme
    {
        Dark,
        Light
    }

    public struct Settings
    {
        public string dumpPath;
        public Theme theme;
        public InstallType installType;
    }

    public class SettingsViewModel : ViewModelBase
    {
        // Static Variables/Fields
        private static string? settingsPath;
        private static readonly string[] possibleSettingsPaths =
        [
            Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.FullName, "settings.json"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ble", "settings.json"),
        ];
        private static string SettingsPath => settingsPath ??= File.Exists(possibleSettingsPaths[0]) ? possibleSettingsPaths[0] : possibleSettingsPaths[1];

        // Instance Variables/Fields
        public Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath), new StringEnumConverter());
        public string DumpPath { get => settings.dumpPath; set => this.RaiseAndSetIfChanged(ref settings.dumpPath, value); }
        public bool LightTheme => settings.theme == Theme.Light;
        public bool DarkTheme => settings.theme == Theme.Dark;
        public bool PermanentInstall => settings.installType == InstallType.Permanent;
        public bool PortableInstall => settings.installType == InstallType.Portable;

        public static void InitSettingsFile()
        {
            if (File.Exists(SettingsPath)) return;
            Directory.CreateDirectory(Directory.GetParent(SettingsPath)!.FullName);
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new Settings(), new StringEnumConverter()));
        }

        public static string GetDumpPath()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath), new StringEnumConverter()).dumpPath;
        }

        public static Theme GetTheme()
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath), new StringEnumConverter()).theme;
        }

        public void OnInstallTypeSelected(int installTypeNum)
        {
            settings.installType = (InstallType)installTypeNum;
        }

        public void OnThemeSelected(int themeNum)
        {
            settings.theme = (Theme)themeNum;
            ThemeVariant theme = themeNum == 0 ? ThemeVariant.Dark: ThemeVariant.Light;
            Application.Current!.RequestedThemeVariant = theme;
        }

        public void SaveSettings()
        {
            string delPath;
            if (PortableInstall)
            {
                settingsPath = possibleSettingsPaths[0];
                delPath = possibleSettingsPaths[1];
            }
            else
            {
                settingsPath = possibleSettingsPaths[1];
                delPath = possibleSettingsPaths[0];
            }
            File.Delete(delPath);
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settings, new StringEnumConverter()));
        }
    }
}
