using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Assets.Scripts.Camera;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using System.Reflection;
using System;

namespace MegabonkMinimap
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Plugin : BasePlugin
    {
        public const string
            MODNAME = "MegabonkMinimap",
            AUTHOR = "svindler",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "0.3.0";

        public static ManualLogSource log;

        internal static ConfigEntry<float> MinimapSize;
        internal static ConfigEntry<float> MinimapZoom;
        internal static ConfigEntry<bool> AlwaysShowBoss;

        public Plugin() => log = Log;

        public override void Load()
        {
            MinimapSize = Config.Bind(
                "Minimap", "Size", 1.5f,
                "Scale of the minimap UI (higher = bigger minimap on screen)."
            );

            MinimapZoom = Config.Bind(
                "Minimap", "Zoom", 110f,
                "Zoom level of the minimap camera.\n" +
                "Higher = zoomed out (see more of the map).\n" +
                "Lower = zoomed in (see less)."
            );

            AlwaysShowBoss = Config.Bind(
                "Minimap", "AlwaysShowBossArrow", false,
                "If true, the minimap creates the boss arrow immediately.\n" +
                "If false, the arrow only appears after the portal is spotted (default behaviour)."
            );

            log.LogInfo($"Loading {MODNAME} v{VERSION} by {AUTHOR}");
            var harmony = new Harmony(GUID);
            harmony.PatchAll();
            log.LogInfo($"{MODNAME} loaded.");
        }

        [HarmonyPatch(typeof(MinimapUi), "UpdateScale")]
        public static class MinimapUi_UpdateScale_Patch
        {
            private static void Prefix(ref float scale) => scale = Plugin.MinimapSize.Value;
        }

        [HarmonyPatch(typeof(MinimapCamera), "Start")]
        public static class MinimapCamera_Start_Patch
        {
            private static void Postfix(MinimapCamera __instance)
            {
                if (__instance?.minimapCamera == null) return;
                __instance.minimapCamera.orthographicSize = Plugin.MinimapZoom.Value;
            }
        }

        [HarmonyPatch(typeof(Assets.Scripts.Camera.MinimapCamera), "TrySpotBossSpawner")]
        public static class MinimapCamera_TrySpotBossSpawner_Patch
        {
            private static readonly PropertyInfo BossSpawnerProp =
                AccessTools.Property(typeof(Assets.Scripts.Camera.MinimapCamera), "bossSpawner");

            private static bool Prefix(Assets.Scripts.Camera.MinimapCamera __instance)
            {
                if (__instance == null) return true;

                var spawnerTf = BossSpawnerProp?.GetValue(__instance) as Transform;
                if (spawnerTf == null) return true;

                if (Plugin.AlwaysShowBoss.Value && !__instance.bossSpotted)
                {
                    __instance.AddArrow(spawnerTf, __instance.bossColor);
                    __instance.bossSpotted = true;
                    return false;
                }

                return true;
            }
        }
    }
}