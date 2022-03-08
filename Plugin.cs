using System.Net;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine;
using Utilla;
using BepInEx;
using HarmonyLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine.Rendering;
using Bepinject;
using BepInEx.Configuration;


namespace FlySomewhataAboveAverageSpeed
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>
    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [Description("An above average speed flight mod for PC gorilla tag, Works with HauntedModMenu :D")]
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        //Node rightHandNode = XRNode.RightHand; // 100% mine not stolen
        bool inRoom;
        public static float num = 1000f;
        public static float num2 = 1000f;
        public static Rigidbody RB;
        public static Transform headTransform; // why is the assignmnet here?, tis not anymore
        private FlyFastManager modInstance = null;
        private bool modEnabled = false;
        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
            modEnabled = true;
            if (modInstance != null)
                modInstance.enabled = modEnabled && inRoom;
            ConfigFile file = new ConfigFile(Path.Combine(Paths.ConfigPath, "FlyFast.cfg"), true);
            var data = file.Bind("settings", "Forward Speed", num, "settings description");
            num = (data.Value > num) ? num : data.Value; // sets max to default value if its above it sets it to default\
            var data2 = file.Bind("settings", "Up Speed", num2, "settings description");
            num2 = (data2.Value > num2) ? num2 : data2.Value; // sets max to default value


        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/
            modEnabled = false;
            if (modInstance != null)
                modInstance.enabled = modEnabled;
            HarmonyPatches.RemoveHarmonyPatches();
            // delete this CI mod status can cause this to break, tis deleted
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */

            // asign your variables here
            headTransform = Player.Instance.headCollider.gameObject.transform;
            RB = Player.Instance.bodyCollider.attachedRigidbody;

        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            modInstance = gameObject.AddComponent<FlyFastManager>();
            modInstance.enabled = modEnabled;
            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            GameObject.Destroy(modInstance);
            inRoom = false;

        }
    }
}