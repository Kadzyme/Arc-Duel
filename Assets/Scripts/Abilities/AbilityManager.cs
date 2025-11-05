using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Main Abilities")]
    [SerializeField] private KeyCode[] slotKeys = { KeyCode.Q, KeyCode.E, KeyCode.R };
    [SerializeField] private KeyCode switchKey = KeyCode.LeftShift;
    [SerializeField] private AbilitySet[] abilitySets;

    [Header("Inherit Ability")]
    [SerializeField] private KeyCode inheritAbilityKey = KeyCode.C;
    [SerializeField] private Ability inheritAbility;

    private int currentSetIndex = 0;
    private AbilitySet CurrentSet => abilitySets != null && abilitySets.Length > 0 ? abilitySets[currentSetIndex] : null;

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
            CycleMode();

        for (int i = 0; i < slotKeys.Length; i++)
        {
            if (Input.GetKeyDown(slotKeys[i]))
                CurrentSet?.GetAbility(i)?.TryUse(gameObject);
        }

        if (inheritAbility != null && Input.GetKeyDown(inheritAbilityKey))
        {
            inheritAbility.TryUse(gameObject);
        }
    }

    private void CycleMode()
    {
        if (abilitySets == null || abilitySets.Length == 0) return;
        currentSetIndex = (currentSetIndex + 1) % abilitySets.Length;
    }

    public void RebindSlot(int slotIndex, KeyCode newKey)
    {
        if (slotIndex < 0 || slotIndex >= slotKeys.Length) return;
        slotKeys[slotIndex] = newKey;
    }

    public void SetInheritAbility(Ability newAbility)
    {
        inheritAbility = newAbility;
    }

    public void SetInheritAbilityKey(KeyCode newKey)
    {
        inheritAbilityKey = newKey;
    }
}
