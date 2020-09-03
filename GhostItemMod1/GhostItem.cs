using BepInEx;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace GhostItemMod1
{
    [BepInDependency("com.bepis.r2api")]
    //Change these
    [BepInPlugin("com.Paymon.GhostItem", "Ghost Item", "1.0.0")]
    [R2APISubmoduleDependency(nameof(AssetPlus), nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    public class GhostItem : BaseUnityPlugin
    {
        public bool teleporterActive = false;
        public void Awake()
        {
            // RegisterItem();
            
            // Register all the hooks
            On.RoR2.HealthComponent.TakeDamage += orig_TakeDamage;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += orig_OnTPInteractionBegin;
            On.RoR2.SceneDirector.PlaceTeleporter += orig_OnTPPlace;
        }

        private void orig_OnTPPlace(On.RoR2.SceneDirector.orig_PlaceTeleporter orig, SceneDirector self)
        {
            // When the teleporter is placed on a level, set teleporterActive to false and call orig
            teleporterActive = false;
            orig(self);
        }

        private void orig_OnTPInteractionBegin(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig,
            TeleporterInteraction self, Interactor activator)
        {
            // When the teleporter is interacted with, set teleporterActive to true and call orig
            teleporterActive = true;
            orig(self, activator);
        }

        private void orig_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self, DamageInfo info)
        {
            // Run the original damage code to figure out whether or not they're actually dead
            orig(self, info);
            var characterBody = self.body;
            if (!teleporterActive && // If the teleporter hasn't been touched yet that level
                characterBody.inventory.GetItemCount(ItemIndex.FireRing) > 0 && // Check whether or not they have the
                                                                        // item (currently Kjaro's Band (the fire one)
                characterBody.master.IsDeadAndOutOfLivesServer()) // And make sure they're dead before respawning them
                                                              // or this will respawn them every time they take damage
            {
                characterBody.master.Respawn(characterBody.footPosition, new Quaternion()); // Respawn the
                                                                                                    // character where
                                                                                                    // they already
                                                                                                    // are
                // Remove item or whatever penalty here
            }
        }

        // Currently all commented out, here's where you'll register all your item stuff
        
        // public void RegisterItem()
        // {
        //     var ghostItemDef = new ItemDef
        //     {
        //         name = "GhostItem", // its the internal name, no spaces, apostrophes and stuff like that
        //         tier = ItemTier.Tier3,
        //         pickupModelPath = PrefabPath,
        //         pickupIconPath = IconPath,
        //         nameToken = "BISCOLEASH_NAME", // stylised name
        //         pickupToken = "BISCOLEASH_PICKUP",
        //         descriptionToken = "BISCOLEASH_DESC",
        //         loreToken = "BISCOLEASH_LORE",
        //         tags = new[]
        //         {
        //             ItemTag.Utility,
        //             ItemTag.Damage
        //         }
        //     };
        //
        //     // var itemDisplayRules =
        //     //     new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
        //     // itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
        //     // itemDisplayRules[0].childName =
        //     //     "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
        //     // itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
        //     // itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
        //     // itemDisplayRules[0].localPos =
        //     //     new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest
        //
        //     var ghostItem = new CustomItem(ghostItemDef, null);
        //
        //     BiscoLeashItemIndex = ItemAPI.Add(biscoLeash); // ItemAPI sends back the ItemIndex of your item
        // }
    }
}

