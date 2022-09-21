using UnityEngine;

namespace PhoneInteractionDemo
{
    public class TestObjectController : MonoBehaviour
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
    }
}
