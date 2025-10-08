using System;
using UnityEngine;

namespace SerapKeremGameKit._Particles
{
    [Serializable]
    public sealed class ParticleData
    {
        public string Key;
        public ParticleSystem Prefab;
        public float Duration = 1f;
    }
}


