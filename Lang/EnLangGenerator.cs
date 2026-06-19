using MossLib.Base;

namespace CUTE.Lang;

public class EnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "EN";

    protected override void BuildLocaleData()
    {
        // Item
        Add("config.item.powerful_mindwipe.name", "Powerful Mindwipe");
        Add("config.item.powerful_mindwipe.description", "Locks INT to 0 after using mindwipe");

        // Mechanism
        Add("config.mechanism.cave_ticks_generated_number.name", "Cave Tick Spawn Count");
        Add("config.mechanism.cave_ticks_generated_number.description", "Number of cave ticks spawned when triggering the trap (default 16)");
        Add("config.mechanism.trader_hate_you.name", "Trader Hates You");
        Add("config.mechanism.trader_hate_you.description", "When enabled, encountering a trader has a chance to randomly lower reputation and increase hostility, or even attack you directly");
    }
}