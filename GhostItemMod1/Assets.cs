using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;


namespace GhostItemMod1
{
    internal static class Assets
    {
        internal static GameObject GhostPrefab;
        internal static ItemIndex GhostItemIndex;

        //Pathways can be found in unity under the asset bundle viewer
        private const string ModPrefix = "@GhostItem:";
        private const string PrefabPath = ModPrefix + "Assets/Import/Ghost/Ghost.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/Ghost/GhostIcon.png";

        internal static void Init()
        {
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomItem.ghost"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                GhostPrefab = bundle.LoadAsset<GameObject>("Assets/Import/Ghost/Ghost.prefab");
            }

            GhostAsRedTierItem();

            AddLanguageTokens();
        }

        private static void GhostAsRedTierItem()
        {
            var ghostItemDef = new ItemDef
            {
                name = "GhostItem", // its the internal name, no spaces, apostrophes and stuff like that
                tier = ItemTier.Tier3,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "GhostName", // stylised name
                pickupToken = "GhostShell",
                descriptionToken =
                    "GhostDes",
                loreToken = "GhostLore",
                tags = new[]
                {
                    ItemTag.Utility,
                    ItemTag.Damage
                }
            };

            var itemDisplayRules =
                new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = GhostPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName =
                "IKArmPole.l"; // this will define the starting point for the position of the 3d model, you can see what are the different name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos =
                new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var ghostitem = new R2API.CustomItem(ghostItemDef, itemDisplayRules);

            GhostItemIndex = ItemAPI.Add(ghostitem); // ItemAPI sends back the ItemIndex of your item
        }

        private static void AddLanguageTokens()
        {
            //The Name should be self explanatory
            R2API.AssetPlus.Languages.AddToken("GhostName", "Ghost");
            //The Pickup is the short text that appears when you first pick this up. This text should be short and to the point, nuimbers are generally ommited.
            R2API.AssetPlus.Languages.AddToken("GhostShell", "Little Light");
            //The Description is where you put the actual numbers and give an advanced description.
            R2API.AssetPlus.Languages.AddToken("GhostDes",
                "Your Ghost revives you after death unless in a darkness zone.");
            //The Lore is, well, flavor. You can write pretty much whatever you want here.
            R2API.AssetPlus.Languages.AddToken("GhostLore",
                "Ghosts are small, sapient machines created by the Traveler shortly after the Collapse. Their sole purpose is to locate and resurrect deceased individuals capable of wielding the Light as Guardians, and to support their Guardian charge in combat.");
        }

    }
}