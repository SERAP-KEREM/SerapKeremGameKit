using SerapKeremGameKit._Particles;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] private string _particleKey;
    [SerializeField] private ParticleManager _particleManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _particleManager.PlayParticle(_particleKey, gameObject.transform.position, gameObject.transform);

        }
    }
}
