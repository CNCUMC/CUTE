using HarmonyLib;
using UnityEngine;

namespace CUTE.Item;

public static class PowerfulMindwipe
{
    private static PlayerCamera _playerCamera;
    
    [HarmonyPatch(typeof(MindwipeScript), "Update")]
    [HarmonyPostfix]
    private static void UpdatePostfix(MindwipeScript __instance)
    {
        if (__instance.active && Plugin.PowerfulMindwipe.Value)
        {
            // 持续压制智力为0
            _playerCamera.body.skills.INT = (int)Mathf.MoveTowards(
                _playerCamera.body.skills.INT,
                0f,
                Time.deltaTime * 5f
            );
        }
    }
    
    [HarmonyPatch(typeof(PlayerCamera), "Start")]
    [HarmonyPostfix]
    private static void GetPlayerCamera(PlayerCamera __instance)
    {
       _playerCamera = __instance;
    }
}