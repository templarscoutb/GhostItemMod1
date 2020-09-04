using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;


namespace GhostItemMod1
{
    public class GhostBehav : BaseUnityPlugin
    {

        public bool teleporterActive = false;
        public void Awake()
        {

            // Register all the hooks
            On.RoR2.HealthComponent.TakeDamage += orig_TakeDamage;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += orig_OnTPInteractionBegin;
            On.RoR2.SceneDirector.PlaceTeleporter += orig_OnTPPlace;
      //      On.RoR2.CaptainDefenseMatrixController.TryGrantItem += orig_CDMController;
        }
        //Using captains defensive script to spawn Ghost when charter spawns in
    //    private void orig_CDMController(On.RoR2.CaptainDefenseMatrixController.orig_TryGrantItem orig, CaptainDefenseMatrixController self)
    //    {
     //       var characterBody = self.GetFieldValue<CharacterBody>("characterBody");
            
     //       if (characterBody.inventory.GetItemCount(GhostItem.GhostIndex) >= 0)
     //       {
     //           characterBody.master.inventory.GiveItem(GhostItem.GhostIndex);
     //       }
     //       orig(self);
    //    }

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
