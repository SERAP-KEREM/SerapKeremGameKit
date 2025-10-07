using SerapKeremGameKit._Pools;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameKit._Audio
{
    public sealed class AudioPool : BasePool<AudioPlayer>
    {
        protected override AudioPlayer Create()
        {
            GameObject go = new GameObject("AudioPlayer");
            go.transform.SetParent(transform);
            AudioPlayer player = go.AddComponent<AudioPlayer>();
            player.ResetState();
            return player;
        }

        protected override void OnGet(AudioPlayer item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnRecycle(AudioPlayer item)
        {
            item.ResetState();
            item.gameObject.SetActive(false);
        }

        protected override void OnStop(AudioPlayer item)
        {
            item.Stop();
        }
    }
}



