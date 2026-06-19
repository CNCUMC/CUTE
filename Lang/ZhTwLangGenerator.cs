using MossLib.Base;

namespace CUTE.Lang;

public class ZhTwLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-TW";

    protected override void BuildLocaleData()
    {
        // Item
        Add("config.item.fake_blueprint.name", "偽造藍圖機率");
        Add("config.item.fake_blueprint.description", "使用藍圖時有機率扣除經驗且不學習配方（百分比，0=停用）");
        Add("config.item.powerful_mindwipe.name", "強力精神抹除劑");
        Add("config.item.powerful_mindwipe.description", "使用精神抹除劑後鎖死智力至0");

        // Mechanism
        Add("config.mechanism.cave_ticks_generated_number.name", "洞穴蜱蟲生成數量");
        Add("config.mechanism.cave_ticks_generated_number.description", "觸發洞穴蜱蟲陷阱時生成的蜱蟲數量");
        Add("config.mechanism.trader_hate_you.name", "商人討厭你");
        Add("config.mechanism.trader_hate_you.description", "啟用後遇到商人時有機率隨機降低聲望並增加敵意，甚至直接攻擊你");
    }
}