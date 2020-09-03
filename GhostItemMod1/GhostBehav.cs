using System;
using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using UnityEngine;
using CaptainDefenseMatrixController = On.RoR2.CaptainDefenseMatrixController;

namespace GhostItemMod1
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin("com.Paymon.GhostItem", "GhostItem", "1.0.0")]
    [R2APISubmoduleDependency(nameof(AssetPlus), nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    public class GhostBehav : BaseUnityPlugin
    {

        public bool teleporterActive = false;
        public void Awake()
        {

            // Register all the hooks
            On.RoR2.HealthComponent.TakeDamage += orig_TakeDamage;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += orig_OnTPInteractionBegin;
            On.RoR2.SceneDirector.PlaceTeleporter += orig_OnTPPlace;
            On.RoR2.CaptainDefenseMatrixController.TryGrantItem += orig_CDMController;
            
            //console command
            R2API.Utils.CommandHelper.AddToConsoleWhenReady();
        }
        private void orig_CDMController(On.RoR2.CaptainDefenseMatrixController.orig_TryGrantItem orig)
        {
            
        }

        private void orig_OnTPPlace(On.RoR2.SceneDirector.orig_PlaceTeleporter orig, SceneDirector self)
        {
            // When the teleporter is placed on a level, set teleporter Active to false and call orig
            teleporterActive = false;
            orig(self);
        }

        private void orig_OnTPInteractionBegin(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig,
            TeleporterInteraction self, Interactor activator)
        {
            // When the teleporter is interacted with, set teleporter Active to true and call orig
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
                characterBody.inventory.GetItemCount(GhostItem.GhostIndex) > 0 && // Check whether or not they have the Ghost
                                                                        
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
    }
}