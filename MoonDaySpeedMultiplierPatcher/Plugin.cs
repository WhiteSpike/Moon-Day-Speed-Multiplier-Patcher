using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MoonDaySpeedMultiplierPatcher.Misc;
using MoonDaySpeedMultiplierPatcher.Patches;
namespace MoonDaySpeedMultiplierPatcher
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static ManualLogSource mls;

        void Awake()
        {
            mls = Logger;
            harmony.PatchAll(typeof(TimeOfDayPatcher));
            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
    }   
}
