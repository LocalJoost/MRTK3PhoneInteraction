using Microsoft.MixedReality.Toolkit;
using MRTKExtensions.Services.Interfaces;
using MRTKExtensions.Utilities;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

namespace PhoneInteractionDemo
{
    public class TapToPlaceInViewDirection : MonoBehaviour
    {
        [SerializeField]
        private GameObject worldObject;

        [SerializeField]
        private GameObject sceneObjects;
        
        [SerializeField]
        private float maxDistance = 2f;
        
        [SerializeField]
        private float distanceTrigger = 0.2f;

        [SerializeField]
        private float speed = 1.0f;

        [SerializeField]
        private float delay = 0.5f;

        [SerializeField]
        private AudioSource confirmSound;
        
        private float startTime;
        private bool isJustEnabled;
        private Vector3 lastMoveToLocation;
        private IMRTK3ConfigurationFindingService m3Cfg;
        private bool isActive = true;
        private bool isBusy;

        
        private void Start()
        {
            sceneObjects.SetActive(false);
            m3Cfg = ServiceManager.Instance.GetService<IMRTK3ConfigurationFindingService>();
        
            m3Cfg.LeftHandStatusTriggered.AddListener(OnHandTrigger);
            m3Cfg.RightHandStatusTriggered.AddListener(OnHandTrigger);
            
            startTime = Time.time + delay;
            isJustEnabled = true;
        }
        
        
        private void OnHandTrigger(bool status)
        {
            if (status)
            {
                InitialLocationComplete(); 
            }
        }
        
        private void Update()
        {
            if( isActive)
            { var wTransform = worldObject.transform;
                var camPosition = CameraCache.Main.transform.position;
                wTransform.rotation = Quaternion.LookRotation(wTransform.position -
                                                              new Vector3(camPosition.x,
                                                                  worldObject.transform.position.y,
                                                                  camPosition.z));
            }

            if (isBusy || startTime > Time.time)
            {
                return;
            }
            
            if (isActive)
            {
                var hitPoint = LookingDirectionHelpers.GetPositionInLookingDirection(maxDistance);

                if ((hitPoint - lastMoveToLocation).magnitude > distanceTrigger || isJustEnabled)
                {
                    isJustEnabled = false;
                    {
                        isBusy = true;
                        var newPos = hitPoint;
                        LeanTween.move(worldObject, newPos,
                            2.0f * (hitPoint - lastMoveToLocation).magnitude / speed).setEaseInOutSine().setOnComplete(() => isBusy = false);
                        lastMoveToLocation = newPos;
                    }
                }
            }
        }

        private void InitialLocationComplete()
        {
            if (isActive)
            {
                LeanTween.cancel(worldObject);
                isActive = false;
                sceneObjects.SetActive(true);
                confirmSound.Play();
            }
        }
    }
}