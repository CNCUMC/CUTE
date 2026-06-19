using HarmonyLib;
using UnityEngine;

namespace CUTE.Item;

public static class PowerfulMindwipe
{
    [HarmonyPatch(typeof(MindwipeScript), "Update")]
    [HarmonyPostfix]
    private static void UpdatePostfix(MindwipeScript __instance)
    {
        if (__instance.active && Plugin.PowerfulMindwipe.Value)
        {
            // 持续压制智力为0
            PlayerCamera.main.body.skills.INT = (int)Mathf.MoveTowards(
                PlayerCamera.main.body.skills.INT,
                0f,
                Time.deltaTime * 5f
            );
        }
    }
}