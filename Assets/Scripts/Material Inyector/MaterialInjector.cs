using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Session1{
    [Serializable, VolumeComponentMenu("Session1/Material Injector")]
    public class MaterialInjector : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter weight = new ClampedFloatParameter(1, 0, 1);
        public MaterialParameter1 material = new MaterialParameter1();
        public ClampedFloatParameter iteraction = new ClampedFloatParameter(0,0,30);
        public bool IsActive() => weight.value > 0.1f;
        public bool IsTileCompatible() => false;
    }
}

