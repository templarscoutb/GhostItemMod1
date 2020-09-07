using R2API;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;


namespace GhostItemMod1
{
    public class GhostItem
    {

        public static ItemIndex GhostIndex;



        public static void Init()
        {
            AddProvider();
            AddTokens();
            AddItem();
            On.RoR2.CharacterBody.OnInventoryChanged += orig_CharacterBody_OnInventoryChanged;
        }



        private static void AddProvider()
        {
            //namespace.asset bundle name
            using (System.IO.Stream stream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GhostItemMod1.ghostmod"))
            {
                AssetBundle bundle = AssetBundle.LoadFromStream(stream);
                AssetBundleResourcesProvider provider = new AssetBundleResourcesProvider("@GhostBundle", bundle);
                ResourcesAPI.AddProvider(provider);
            }
        }

        private static void AddTokens()
        {
            LanguageAPI.Add("GHOST_NAME_TOKEN", "Ghost");
            LanguageAPI.Add("GHOST_PICK_TOKEN", "Little light");
            LanguageAPI.Add("GHOST_DESC_TOKEN", "Revives you after death if outside of darkness zones.");
            string longLore = "TBW";
            LanguageAPI.Add("GHOST_LORE_TOKEN", longLore);
        }


        private static void AddItem()
        {
            ItemDef def = new ItemDef
            {
                name = "GhostItem",
                nameToken = "GHOST_NAME_TOKEN",
                pickupToken = "GHOST_PICK_TOKEN",
                descriptionToken = "GHOST_DESC_TOKEN",
                loreToken = "GHOST_LORE_TOKEN",
                tier = ItemTier.Tier1,
                pickupIconPath = "@GhostBundle:Assets/Ghost/GhostIcon.png",
                pickupModelPath = "@GhostBundle:Assets/Ghost/Ghost.prefab",
                canRemove = false,
                hidden = false,
                tags = new ItemTag[]
                {
                    ItemTag.Utility,
                    ItemTag.Healing
                }
            };

            GameObject ghostPrefab = Resources.Load<GameObject>("@GhostBundle:Assets/Ghost/Ghost.prefab");
            Vector3 generalScale = new Vector3(1.0f, 1.0f, 1.0f);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.22f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Base",
                    localPos = new Vector3(0.06f, -0.02f, -0.03f),
                    localAngles = new Vector3(90f, 90f, 180f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "LowerArmR",
                    localPos = new Vector3(-2f, 6f, 0f),
                    localAngles = new Vector3(45f, -90f, 0f),
                    localScale = generalScale * 10
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.25f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.21f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.22f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "WeaponPlatform",
                    localPos = new Vector3(0.2f, 0.05f, 0.2f),
                    localAngles = new Vector3(-45f, 0f, 0f),
                    localScale = generalScale * 2
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.22f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Hip",
                    localPos = new Vector3(-2.2f, 0f, 0f),
                    localAngles = new Vector3(-10f, -0.05f, 0f),
                    localScale = generalScale * 10
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
{
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ghostPrefab,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.22f, 0f, 0f),
                    localAngles = new Vector3(0f, -0.05f, 0f),
                    localScale = generalScale
                }
});
        

        CustomItem item = new CustomItem(def, rules);
        GhostIndex = ItemAPI.Add(item);
        }

        private static void orig_CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (!self.gameObject.GetComponent<GhostBehav>() && self.inventory.GetItemCount(GhostIndex) != 0) self.gameObject.AddComponent<GhostBehav>();
        }
    }
}