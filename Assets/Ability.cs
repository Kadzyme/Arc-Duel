using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] private string abilityName = "New Ability";
    [SerializeField] private Sprite icon;
    [SerializeField] private float cooldown = 1f;

    private float lastUseTime;

    public string AbilityName => abilityName;
    public Sprite Icon => icon;
    public float Cooldown => cooldown;
    public bool IsReady => Time.realtimeSinceStartup >= lastUseTime + cooldown;

    private void OnEnable()
    {
        if (lastUseTime > Time.realtimeSinceStartup)
            lastUseTime = -cooldown;
    }

    public void TryUse(GameObject user)
    {
        if (!IsReady) return;
        Use(user);
        lastUseTime = Time.realtimeSinceStartup;
    }

    protected abstract void Use(GameObject user);
}
