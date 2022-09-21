using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MRTKExtensions.Input
{
    public class TapRaycastActivator : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference tapReference;

        private void Start()
        {
            tapReference.action.performed += OnTap;
        }

        private void OnTap(InputAction.CallbackContext ctx)
        {
            ProcessTapValue(ctx.ReadValue<Vector2>());
        }

        private void ProcessTapValue(Vector2 touchPos)
        {
            //https://learn.unity.com/tutorial/placing-and-manipulating-objects-in-ar#605103a5edbc2a6c32bf5663
            var ray = CameraCache.Main.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out var hit))
            {
                var hitGameObject = hit.transform.gameObject;
                var controller = 
                    hitGameObject.GetComponent<ITapRayCastActivatable>() ?? 
                    hitGameObject.GetComponentInParent<ITapRayCastActivatable>();
                controller?.Activate();
            }
        }
    }
}