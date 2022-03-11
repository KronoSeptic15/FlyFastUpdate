using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

namespace FlySomewhataAboveAverageSpeed
{
    class FlyFastManager : MonoBehaviour
    {
        bool Thumb1;
        bool Thumb2;
        bool trigger;
        bool grip;
        void Update()
        {
            if (Plugin.RB is null || Plugin.headTransform is null)
            {
                Debug.Log("something is null or not in a room");
                return;
            }
                List<InputDevice> list = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
                list[0].TryGetFeatureValue(CommonUsages.triggerButton, out trigger);
                list[0].TryGetFeatureValue(CommonUsages.primaryButton, out Thumb1);
                list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out Thumb2);
                list[0].TryGetFeatureValue(CommonUsages.gripButton, out grip);
            if (trigger) // are you pressing trigger button + in a modded room?
                {
                    GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * Plugin.num; // current speed is equal to where you are looking * delta time * config
                }
            if (grip && (Thumb1 || Thumb2))
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.up * Time.deltaTime * Plugin.num2; // should make you fly up similar to an old quest mod
            }
            if(Thumb2)
            {
               
                Thumb2 = false;
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(0.0f, 0.149f, 0.0f);
            }
        }
    }
}
