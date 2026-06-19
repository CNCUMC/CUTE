using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CUTE.Lang;
using HarmonyLib;
using MossLib.Tool;

namespace CUTE;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.cncumc.cute";
    public const string Name = "CUTE";
    public const string Version = "1.0.0";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);
    private static readonly Dictionary<string, ConfigEntryBase> ConfigRegistry = new();

    // Item
    public static ConfigEntry<float> FakeBlueprint;
    public static ConfigEntry<bool> PowerfulMindwipe;

    // Mechanism
    public static ConfigEntry<int> CaveTicksGeneratedNumber;
    public static ConfigEntry<bool> TraderHateYou;

    public void Awake()
    {
        Logger = base.Logger;
        LocaleGenerator.SetLogger(Logger);
        LocaleGenerator.Register(new EnLangGenerator(), Logger);
        LocaleGenerator.Register(new ZhCnLangGenerator(), Logger);
        LocaleGenerator.Register(new ZhTwLangGenerator(), Logger);
        LocaleGenerator.GenerateAll();

        ModLocale.Initialize(Logger);
        _harmony.PatchAll();

        // Item
        FakeBlueprint = RegisterConfigItem(Config, "fake_blueprint", 75f);
        PowerfulMindwipe = RegisterConfigItem(Config, "powerful_mindwipe", true);

        // Mechanism
        CaveTicksGeneratedNumber = RegisterConfigMechanism(Config, "cave_ticks_generated_number", 16);
        TraderHateYou = RegisterConfigMechanism(Config, "trader_hate_you", true);
    }

    private static ConfigEntry<T> RegisterConfigItem<T>(ConfigFile configFile, string key, T defaultValue)
    {
        return RegisterConfig(configFile, "Item", key, defaultValue);
    }
    
    private static ConfigEntry<T> RegisterConfigMechanism<T>(ConfigFile configFile, string key, T defaultValue)
    {
        return RegisterConfig(configFile, "Mechanism", key, defaultValue);
    }

    private static ConfigEntry<T> RegisterConfig<T>(ConfigFile configFile, string section, string key, T defaultValue)
    {
        return MossLib.Tool.Config.Register(configFile, section, key, defaultValue,
            _ => Locale($"config.{section.ToLower()}.{key}.description"), ConfigRegistry);
    }

    private static string Locale(string key)
    {
        return ModLocale.GetFormat(key);
    }
}