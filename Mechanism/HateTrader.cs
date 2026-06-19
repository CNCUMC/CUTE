using HarmonyLib;
using UnityEngine;

namespace CUTE.Mechanism;

[HarmonyPatch(typeof(TraderScript))]
public class HateTrader
{
    // 每帧检测的敌意触发概率（0~1）
    private const float HostilityChancePerSecond = 0.003f;
    // 每次触发时声望降低范围
    private const float RepLossMin = 5f;
    private const float RepLossMax = 15f;
    // 每次触发时敌意增加范围
    private const float HostilityGainMin = 15f;
    private const float HostilityGainMax = 35f;

    [HarmonyPatch("OnWillRenderObject")]
    [HarmonyPostfix]
    private static void OnWillRenderObjectPostfix(TraderScript __instance)
    {
        // 仅在以下条件满足时检测：
        // 1. 已开始对话
        // 2. 建筑还活着（health > 200）
        // 3. 尚未敌对
        // 4. 距离玩家足够近（<12）
        if (!__instance.startedConvo
            || __instance.hostile
            || __instance.build.health <= 200f)
            return;

        var dist = Vector2.Distance(__instance.transform.position, __instance.body.transform.position);
        if (dist > 12f)
            return;

        // 每帧以小概率触发"厌恶"事件
        if (Random.value > HostilityChancePerSecond * Time.deltaTime * 60f)
            return;

        // 计算声望降低量
        var repLoss = Random.Range(RepLossMin, RepLossMax);
        __instance.reputation -= repLoss;

        // 计算敌意增加量
        var hostilityGain = Random.Range(HostilityGainMin, HostilityGainMax);
        __instance.hostility += hostilityGain;

        Plugin.Logger.LogInfo(
            $"[HateTrader] Trader '{__instance.build.fullName}' hates player! " +
            $"Rep -{repLoss:F1} (now {__instance.reputation:F1}), " +
            $"Hostility +{hostilityGain:F1} (now {__instance.hostility:F1})");

        switch (__instance.hostility)
        {
            // 如果敌意达到100，直接触发攻击
            case >= 100f:
                __instance.hostility = 100f;
                __instance.talker.Talk(Locale.GetCharacter("tradercombat", __instance.character));
                __instance.build.cantHit = false;
                __instance.standTarget = 1f;
                Plugin.Logger.LogInfo($"[HateTrader] Trader '{__instance.build.fullName}' is now ATTACKING the player!");
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
