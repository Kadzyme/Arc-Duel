using UnityEngine;

public enum ElementMode { Fire, Water, Air, Earth }

[CreateAssetMenu(menuName = "Abilities/Ability Set")]
public class AbilitySet : ScriptableObject
{
    [SerializeField] private ElementMode mode;
    [SerializeField] private Ability[] abilities;

    public ElementMode Mode => mode;

    public Ability GetAbility(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= abilities.Length) return null;
        return abilities[slotIndex];
    }

    public int SlotCount => abilities.Length;
}
