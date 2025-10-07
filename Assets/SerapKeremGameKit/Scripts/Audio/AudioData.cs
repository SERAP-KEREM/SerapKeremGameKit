using System;
using UnityEngine;

namespace SerapKeremGameKit._Audio
{
    [Serializable]
    public sealed class AudioData
    {
        public string Key;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(0.1f, 3f)] public float Pitch = 1f;
        public bool Loop = false;
        public bool Spatial = false;
        public float MinDistance = 1f;
        public float MaxDistance = 20f;
    }
}


