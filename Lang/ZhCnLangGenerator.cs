using MossLib.Base;

namespace CUTE.Lang;

public class ZhCnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-CN";

    protected override void BuildLocaleData()
    {
        // Item
        Add("config.item.fake_blueprint.name", "伪造蓝图概率");
        Add("config.item.fake_blueprint.description", "使用蓝图时有概率扣除经验且不学习配方（百分比，0=禁用");
        Add("config.item.powerful_mindwipe.name", "强力精神抹除剂");
        Add("config.item.powerful_mindwipe.description", "使用精神抹除剂后锁死智力至0");
        
        // Mechanism
        Add("config.mechanism.cave_ticks_generated_number.name", "洞穴蜱虫生成数量");
        Add("config.mechanism.cave_ticks_generated_number.description", "触发洞穴蜱虫陷阱时生成的蜱虫数量");
        Add("config.mechanism.trader_hate_you.name", "商人讨厌你");
        Add("config.mechanism.trader_hate_you.description", "启用后遇到商人时有概率随机降低声望并增加敌意，甚至直接攻击你");
    }
}