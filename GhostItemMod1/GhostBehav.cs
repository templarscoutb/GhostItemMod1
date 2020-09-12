using System;
using BepInEx;
using EntityStates;
using On.EntityStates.BrotherMonster;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Serialization;
using TeleporterInteraction = On.RoR2.TeleporterInteraction;


namespace GhostItemMod1
{
    public class GhostBehav : MonoBehaviour
    {

        public bool respawnActive = true;
        

        public void Awake()
        {

            // Register all the hooks
            On.RoR2.HealthComponent.TakeDamage += orig_TakeDamage;
            
            On.RoR2.TeleporterInteraction.ChargingState.OnEnter += orig_TPCharging;
            On.RoR2.TeleporterInteraction.IdleState.GetInteractability += orig_TPBase;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter += orig_TPCharged;
            
            On.RoR2.GoldshoresMissionController.Start += orig_GSMC;
            On.RoR2.ArenaMissionController.OnEnable += orig_ARMC;
            On.RoR2.ArtifactTrialMissionController.Awake += orig_ARTMC;
            On.RoR2.MoonMissionController.OnEnable += orig_MMC;
        }
        

        private void EnteringDarknessZone()
        {
            Chat.AddMessage("Guardian! You have entered a darkness zone!");
        }

        private void ExitDarknessZone()
        {
            Chat.AddMessage("Guardian! You have exited the darkness zone.");
        }

        private Interactability orig_TPBase(TeleporterInteraction.IdleState.orig_GetInteractability orig, BaseState self, Interactor activator)
        {
            //When teleporter is Idle, Ghost can revive player
            Chat.AddMessage("Base function has ran");
            respawnActive = true;
            orig(self, activator);
            return Interactability.Available;
        }

        private void orig_TPCharging(TeleporterInteraction.ChargingState.orig_OnEnter orig, BaseState self)
        {
            //When teleporter is charging, Ghost cannot revive player
            respawnActive = false;
            Chat.AddMessage("Charing function has ran");
           // EnteringDarknessZone();
            orig(self);
        }

        private void orig_TPCharged(TeleporterInteraction.ChargedState.orig_OnEnter orig, BaseState self)
        {
            //When teleporter is charged, Ghost can revive again
            respawnActive = true;
            Chat.AddMessage("Charged function has ran");
           // ExitDarknessZone();
            orig(self);
        }

        

        private void orig_GSMC(On.RoR2.GoldshoresMissionController.orig_Start orig, GoldshoresMissionController self)
        {
            //If in the Golden Shore Ghost cannot respawn you
            Chat.AddMessage("Gold function has ran");
            respawnActive = false;
            orig(self);
            //  EnteringDarknessZone();

        }

        private void orig_ARMC(On.RoR2.ArenaMissionController.orig_OnEnable orig, ArenaMissionController self)
        {
            //If in the Null Realm Ghost cannot respawn you
            respawnActive = false;
           // EnteringDarknessZone();
            orig(self);
       
        }

        private void orig_ARTMC(On.RoR2.ArtifactTrialMissionController.orig_Awake orig, ArtifactTrialMissionController self)
        {
            //If in the artificial realm Ghost cannot respawn you
            respawnActive = false;
           // EnteringDarknessZone();
            orig(self);
   
        }

        private void orig_MMC(On.RoR2.MoonMissionController.orig_OnEnable orig, MoonMissionController self)
        {
            //If on the Moon GHost cannot respawn you
            respawnActive = false;
          //  EnteringDarknessZone();
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
