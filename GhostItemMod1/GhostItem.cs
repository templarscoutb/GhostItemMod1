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
        public static GameObject GhostPrefab;
        
        

        public static void Init()
        {
            AddProvider();
            AddTokens();
            AddItem();
            On.RoR2.CharacterBody.OnInventoryChanged += orig_CharacterBody_OnInventoryChanged;
            On.RoR2.ItemFollower.Update += orig_ItemFollow_Update;
        }

        private static void AddProvider()
        {
                                                                                                         //namespace.asset bundle name
            using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GhostItemMod1.ghostmod"))
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
        public static void orig_ItemFollow_Update(On.RoR2.ItemFollower.orig_Update orig, ItemFollower self)
        {
            GhostPrefab = self.followerPrefab;
            orig(self);
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
                canRemove = true,
                hidden = false,
                tags = new ItemTag[]
                {
                    ItemTag.Utility,
                    ItemTag.OnKillEffect
                }
            };

            
            GameObject GhostPrefab = Resources.Load<GameObject>("@GhostBundle:Assets/Ghost/Ghost.prefab");
            
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = GhostPrefab,
                    childName = "chest",
                    localPos = new Vector3(-0.22f, 0f, 0f),
                    localAngles = new Vector3(0f, 0f, 0f),
                    localScale = new Vector3(0f,0f,0f)
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