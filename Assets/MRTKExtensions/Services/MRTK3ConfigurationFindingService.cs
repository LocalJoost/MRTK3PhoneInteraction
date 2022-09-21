using MRTKExtensions.Services.Interfaces;
using RealityCollective.ServiceFramework.Services;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.Events;

namespace MRTKExtensions.Services
{
    [System.Runtime.InteropServices.Guid("dd1c8edc-335b-4510-872a-ced01fca424a")]
    public class MRTK3ConfigurationFindingService : BaseServiceWithConstructor, IMRTK3ConfigurationFindingService
    {
        public MRTK3ConfigurationFindingService(string name, uint priority,
            MRTK3ConfigurationFindingServiceProfile profile)
            : base(name, priority)
        {
            this.profile = profile;
        }

        private readonly MRTK3ConfigurationFindingServiceProfile profile;
        
        private float leftToggleTime;
        private float rightToggleTime;
        
        public ControllerLookup ControllerLookup => controllerLookup;

        private bool leftHandTriggerStatus = false;
        private bool rightHandTriggerStatus = false;

        private HandsAggregatorSubsystem handsAggregatorSubsystem;
        
        private ControllerLookup controllerLookup;

        public ArticulatedHandController LeftHand => (ArticulatedHandController)controllerLookup.LeftHandController;
        public ArticulatedHandController RightHand => (ArticulatedHandController)controllerLookup.RightHandController;
        
        /// <inheritdoc />
        public override void Start()
        {
            GetHandControllerLookup();
            handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        }


        private void GetHandControllerLookup()
        {
            if (controllerLookup == null)
            {
                ControllerLookup[] lookups =
                    GameObject.FindObjectsOfType(typeof(ControllerLookup)) as ControllerLookup[];
                if (lookups.Length == 0)
                {
                    Debug.LogError(
                        "Could not locate an instance of the ControllerLookup class in the hierarchy. It is recommended to add ControllerLookup to your camera rig.");
                }
                else if (lookups.Length > 1)
                {
                    Debug.LogWarning(
                        "Found more than one instance of the ControllerLookup class in the hierarchy. Defaulting to the first instance.");
                    controllerLookup = lookups[0];
                }
                else
                {
                    controllerLookup = lookups[0];
                }
            }
        }

        public UnityEvent<bool> LeftHandStatusTriggered { get; } = new UnityEvent<bool>();

        public UnityEvent<bool> RightHandStatusTriggered { get; } = new UnityEvent<bool>();

        /// <inheritdoc />
        public override void Update()
        {
            if (!TryUpdateByTrigger())
            {
                TryUpdateByPinch();
            }
        }

        private bool TryUpdateByTrigger()
        {
            var newStatus = GetIsTriggered(LeftHand);
            if (TryInvokeTriggerEvent(LeftHandStatusTriggered, newStatus, ref leftHandTriggerStatus,
                    ref leftToggleTime))
            {
                return true;
            }
            
            newStatus = GetIsTriggered(RightHand);
            return TryInvokeTriggerEvent(RightHandStatusTriggered, newStatus, ref rightHandTriggerStatus,
                ref rightToggleTime);
        }

        private bool GetIsTriggered(ArticulatedHandController hand)
        {
            return hand.currentControllerState.selectInteractionState.value > profile.PinchThreshold;
        }

        private void TryUpdateByPinch()
        {
            if (handsAggregatorSubsystem != null)
            {
                var newStatus = TryUpdateByPinch(LeftHand);
                TryInvokeTriggerEvent(LeftHandStatusTriggered, newStatus, ref leftHandTriggerStatus,
                    ref leftToggleTime);
                
                newStatus = TryUpdateByPinch(RightHand);
                TryInvokeTriggerEvent(RightHandStatusTriggered, newStatus, ref rightHandTriggerStatus,
                    ref rightToggleTime);
            }
        }

        private bool TryInvokeTriggerEvent(UnityEvent<bool> handEvent, bool newStatus, ref bool currentStatus, ref float lastInvokeTime)
        {
            if (Time.time - lastInvokeTime > profile.DoubleClickTime && newStatus != currentStatus && Time.time > 2f)
            {
                lastInvokeTime = Time.time;
                currentStatus = newStatus;
                handEvent.Invoke(newStatus);
                return true;
            }

            return false;
        }

        private bool TryUpdateByPinch(ArticulatedHandController handController)
        {
            var progressDetectable =
                handsAggregatorSubsystem.TryGetPinchProgress(handController.HandNode,
                    out bool isReadyToPinch,
                    out bool isPinching,
                    out float pinchAmount);
            return progressDetectable && isPinching && pinchAmount > profile.PinchThreshold;
        }
    }
}