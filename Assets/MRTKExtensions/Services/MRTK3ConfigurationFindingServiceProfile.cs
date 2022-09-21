
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace MRTKExtensions.Services
{
    [CreateAssetMenu(menuName = "MRTK3ConfigurationFindingServiceProfile",
        fileName = "MRTK3ConfigurationFindingServiceProfile", order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class MRTK3ConfigurationFindingServiceProfile : BaseServiceProfile<IServiceDataProvider>
    {
        [SerializeField]
        private float doubleClickTime = 0.2f;

        public float DoubleClickTime => doubleClickTime;

        [SerializeField]
        private float pinchThreshold = 0.95f;

        public float PinchThreshold => pinchThreshold;

    }
}
