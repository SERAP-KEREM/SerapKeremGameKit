using DG.Tweening;
using SerapKeremGameKit._Audio;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private string _audioKey;
    [SerializeField] private AudioManager _audioManager;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _audioManager.Play(_audioKey);
        }
    }
}
