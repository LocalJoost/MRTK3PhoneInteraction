using MRTKExtensions.Input;
using UnityEngine;

namespace PhoneInteractionDemo
{
    public class TestObjectController : MonoBehaviour, ITapRayCastActivatable
    {
        [SerializeField]
        private GameObject cube;

        [SerializeField]
        private Color selectColor;

        [SerializeField]
        private GameObject gazeSphere;

        private Color originalColor;

        private Renderer cubeRender;

        private void Start()
        {
            cubeRender = cube.GetComponent<Renderer>();
            originalColor = cubeRender.materials[0].color;
            gazeSphere.SetActive(false);
        }

        public void Activate(bool status)
        {
            cubeRender.materials[0].color = status ? selectColor : originalColor;
        }

        public void GazeActivate(bool status)
        {
            gazeSphere.SetActive(status);
        }

        private bool lastTapStatus = false;
        private float lastInvokeTime = float.MinValue;

        public void Activate()
        {
            if (Time.time - lastInvokeTime > 0.2f)
            {
                lastInvokeTime = Time.time;
                lastTapStatus = !lastTapStatus;
                Activate(lastTapStatus);
            }
        }
    }
}
