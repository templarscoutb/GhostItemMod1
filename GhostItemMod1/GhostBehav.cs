using System;
using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Serialization;



namespace GhostItemMod1
{
    public class GhostBehav : MonoBehaviour
    {

        public bool respawnActive = true;

        public void Awake()
        {

            // Register all the hooks
            On.RoR2.HealthComponent.TakeDamage += orig_TakeDamage;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += orig_OnInteraction;
            On.RoR2.TeleporterInteraction.OnEnable += orig_Enable;
            On.RoR2.GoldshoresMissionController.OnEnable += orig_GSMC;
            On.RoR2.ArenaMissionController.OnEnable += orig_ARMC;
            On.RoR2.ArtifactTrialMissionController.Awake += orig_ARTMC;
            On.RoR2.MoonMissionController.OnEnable += orig_MMC;
        }
        

        private void EnteringDarknessZone()
        {
            Chat.AddMessage("Guardian! You have entered a darkness zone!");
        }
        


        private void orig_OnInteraction(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, TeleporterInteraction self, Interactor activator)
        {
            //Cannot respawn during teleporter event
            respawnActive = false;
            EnteringDarknessZone();
            orig(self, activator);
        }

        private void orig_Enable(On.RoR2.TeleporterInteraction.orig_OnEnable orig, TeleporterInteraction self)
        {
            respawnActive = true;
            Chat.AddMessage("Guardian! You are exiting the darkness zone.");
            orig(self);
        }

        private void orig_GSMC(On.RoR2.GoldshoresMissionController.orig_OnEnable orig, GoldshoresMissionController self)
        {
            //If in the Golden Shore Ghost cannot respawn you
            orig(self);
            respawnActive = false;
            EnteringDarknessZone();

        }

        private void orig_ARMC(On.RoR2.ArenaMissionController.orig_OnEnable orig, ArenaMissionController self)
        {
            //If in the Null Realm Ghost cannot respawn you
            respawnActive = false;
            EnteringDarknessZone();
            orig(self);
       
        }

        private void orig_ARTMC(On.RoR2.ArtifactTrialMissionController.orig_Awake orig, ArtifactTrialMissionController self)
        {
            //If in the artificial realm Ghost cannot respawn you
            respawnActive = false;
            EnteringDarknessZone();
            orig(self);
   
        }

        private void orig_MMC(On.RoR2.MoonMissionController.orig_OnEnable orig, MoonMissionController self)
        {
            //If on the Moon GHost cannot respawn you
            respawnActive = false;
            EnteringDarknessZone();
            orig(self);

        }

        //Revival Function
        private void orig_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            // Run the original damage code to figure out whether or not they're actually dead
            orig(self, info);
            var characterBody = self.body;
            // If the teleporter hasn't been touched yet that level Check whether or not they have the Ghost and make sure they're dead before respawning them
            if (respawnActive && characterBody.inventory.GetItemCount(GhostItem.GhostIndex) > 0 && characterBody.master.IsDeadAndOutOfLivesServer())
            {
                //Respawn player at position of death
                characterBody.master.Respawn(characterBody.footPosition, new Quaternion()); 
                
                // Remove item or whatever penalty here
            }
        }
    }
}
