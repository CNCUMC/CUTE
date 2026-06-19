using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CUTE.Mechanism;

[HarmonyPatch(typeof(TraderScript))]
public class TraderHateYou
{
    // 一次性判断的敌意触发概率（0~1）
    private const float HostilityChance = 0.15f;

    // 每次触发时声望降低范围
    private const float RepLossMin = 5f;

    private const float RepLossMax = 15f;

    // 每次触发时敌意增加范围
    private const float HostilityGainMin = 15f;
    private const float HostilityGainMax = 35f;

    // 记录已触发过敌意的商人，避免重复判断
    private static readonly HashSet<int> CheckedTraders = [];

    [HarmonyPatch("OnWillRenderObject")]
    [HarmonyPostfix]
    private static void OnWillRenderObjectPostfix(TraderScript __instance, BuildingEntity ___build)
    {
        // 仅在以下条件满足时检测：
        // 1. 商人讨厌你已经开启
        // 2. 已开始对话
        // 3. 建筑还活着（health > 200）
        // 4. 尚未敌对
        // 5. 距离玩家足够近（<12）
        // 6. 尚未判断过
        if (!Plugin.TraderHateYou.Value 
            || !__instance.startedConvo
            || __instance.hostile
            || ___build.health <= 200f
            || CheckedTraders.Contains(__instance.GetHashCode()))
            return;

        var dist = Vector2.Distance(__instance.transform.position, PlayerCamera.main.body.transform.position);
        if (dist > 12f)
            return;

        // 一次性随机判断是否触发敌意（不再每帧判断）
        CheckedTraders.Add(__instance.GetHashCode());
        if (Random.value > HostilityChance)
            return;

        // 计算声望降低量
        var repLoss = Random.Range(RepLossMin, RepLossMax);
        __instance.reputation -= repLoss;

        // 计算敌意增加量
        var hostilityGain = Random.Range(HostilityGainMin, HostilityGainMax);
        __instance.hostility += hostilityGain;

        switch (__instance.hostility)
        {
            // 如果敌意达到100，直接触发攻击
            case >= 100f:
                __instance.hostility = 100f;
                __instance.talker.Talk(Locale.GetCharacter("tradercombat", __instance.character));
                ___build.cantHit = false;
                __instance.standTarget = 1f;
                break;
            case >= 55f when __instance.hostility - hostilityGain < 55f:
                // 刚超过55线，发出警告
                __instance.talker.Talk(Locale.GetCharacter("traderwarning", __instance.character));
                break;
            case >= 30f when __instance.hostility - hostilityGain < 30f:
                // 刚超过30线，发出警告并站起来
                __instance.talker.Talk(Locale.GetCharacter("traderwarning", __instance.character));
                __instance.standTarget = 1f;
                break;
        }
    }
}