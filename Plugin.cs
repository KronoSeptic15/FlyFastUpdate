using System.IO;
using System;
using System.ComponentModel;
using UnityEngine;
using Utilla;
using BepInEx;
using GorillaLocomotion;
using BepInEx.Configuration;

namespace FlySomewhataAboveAverageSpeed{
    [Description("An above average speed flight mod for PC gorilla tag, Works with HauntedModMenu :D")]
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin{
        bool inRoom;
        public static float num = 75f;
        public static float num2 = 1000f;
        public static Rigidbody RB;
        public static Transform headTransform;
        private FlyFastManager modInstance = null;
        private bool modEnabled = false;
        void OnEnable(){
            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
            modEnabled = true;
            if (modInstance != null)
                modInstance.enabled = modEnabled && inRoom;
            ConfigFile file = new ConfigFile(Path.Combine(Paths.ConfigPath, "FlyFast.cfg"), true);
            var data = file.Bind("settings", "Forward Speed", num, "settings description");
            num = (data.Value > num) ? num : data.Value; // sets max to default value if its above it sets it to default
            var data2 = file.Bind("settings", "Up Speed", num2, "settings description");
            num2 = (data2.Value > num2) ? num2 : data2.Value; // sets max to default value
        }
        void OnDisable(){
            modEnabled = false;
            if (modInstance != null)
                modInstance.enabled = modEnabled;
            HarmonyPatches.RemoveHarmonyPatches();
        }
        void OnGameInitialized(object sender, EventArgs e){
            headTransform = Player.Instance.headCollider.gameObject.transform;
            RB = Player.Instance.bodyCollider.attachedRigidbody;
        }
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode){
            modInstance = gameObject.AddComponent<FlyFastManager>();
            modInstance.enabled = modEnabled;
            inRoom = true;
        }
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode){
            GameObject.Destroy(modInstance);
            inRoom = false;
        }
    }
}
