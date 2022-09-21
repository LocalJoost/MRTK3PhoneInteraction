using System;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit
{
    public static class CameraCache
    {
        [Obsolete("CameraCache.Main is deprecated, please use Camera.main instead. Unity have solved the performance hit for that")]
        public static Camera Main => Camera.main;
    }
}