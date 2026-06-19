using HarmonyLib;
using UnityEngine;

namespace CUTE.Mechanism;

[HarmonyPatch(typeof(CaveTickSpawner))]
public class CaveTicksGeneratedNumber
{
    [HarmonyPatch("SpawnSpiders")]
    [HarmonyPrefix]
    public static bool SpawnSpidersPrefix(CaveTickSpawner __instance)
    {
        var count = Plugin.CaveTicksGeneratedNumber.Value;
        for (var i = 0; i < count; ++i)
        {
            Object.Instantiate(
                Resources.Load("cavetick"),
                __instance.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)),
                Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
        }
        Object.Destroy(__instance);
        return false;
    }
}
