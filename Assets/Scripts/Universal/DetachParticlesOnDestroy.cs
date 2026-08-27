using UnityEngine;

public class DetachParticlesOnDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        var partSystem = GetComponent<ParticleSystem>();
        if (gameObject.scene.isLoaded && partSystem != null)
        {
            partSystem.transform.SetParent(null);

            var main = partSystem.main;
            main.stopAction = ParticleSystemStopAction.Destroy;

            partSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
