using BepInEx;
using R2API.Utils;
using UnityEngine.Experimental.PlayerLoop;

namespace GhostItemMod1
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [R2APISubmoduleDependency(new string[] { "ResourcesAPI", "LanguageAPI", "ItemAPI" })]
    [BepInPlugin(modGuid, modName, modVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public sealed class GhostPlugin : BaseUnityPlugin
    {
        private const string modName = "GhostItem";
        private const string modGuid = "com.PaymonTheVoidwalker." + modName;
        private const string modVer = "o.o.1";
        internal static GhostPlugin instance;

        private void Awake()
        {
            if (instance == null) instance = this;
            GhostItem.Init();
            
        }
    }
}