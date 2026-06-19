using HarmonyLib;
using UnityEngine;

namespace CUTE.Mechanism;

[HarmonyPatch(typeof(global::Item))]
public class FakeBlueprint
{
    [HarmonyPatch("SetupItems")]
    [HarmonyPostfix]
    private static void SetupItemsPostfix()
    {
        // FakeBlueprint.Value 是触发概率百分比（0~100），0 表示禁用
        var fakeChancePercent = Plugin.FakeBlueprint.Value;
        if (fakeChancePercent <= 0f)
            return;

        if (!global::Item.GlobalItems.TryGetValue("blueprint", out var info))
            return;

        // 保存原始 useAction
        var originalAction = info.useAction;

        // 替换为带概率触发的版本
        info.useAction = delegate(Body body, global::Item item)
        {
            if (Random.value * 100f < fakeChancePercent)
            {
                // 伪造蓝图：扣除经验，不学习配方（不低于0）
                item.condition = 0f;
                body.skills.expINT = Mathf.Max(0f, body.skills.expINT - 25f);

                _ = Recipes.recipes[item.GetComponent<BlueprintScript>().recipeIndex];
                PlayerCamera.main.DoAlert(":)");
                Sound.Play("combine", item.transform.position);
            }
            else
            {
                // 正常蓝图
                originalAction(body, item);
            }
        };
    }
}