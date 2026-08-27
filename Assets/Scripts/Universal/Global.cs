using UnityEngine;

public static class Global
{
    public static void SafeDestroyProjectile(GameObject projectileObject)
    {
        if (projectileObject == null) return;

        ParticleSystem partSystem = projectileObject.GetComponentInChildren<ParticleSystem>();

        if (partSystem != null)
        {
            partSystem.transform.SetParent(null, true);

            var main = partSystem.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            partSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Object.Destroy(projectileObject);
    }
}
