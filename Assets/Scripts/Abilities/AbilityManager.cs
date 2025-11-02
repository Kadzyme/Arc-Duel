using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private KeyCode[] slotKeys = { KeyCode.Q, KeyCode.E, KeyCode.R };
    [SerializeField] private KeyCode switchKey = KeyCode.LeftShift;
    [SerializeField] private AbilitySet[] abilitySets;

    private int currentSetIndex = 0;
    private AbilitySet CurrentSet => abilitySets[currentSetIndex];

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
            CycleMode();

        for (int i = 0; i < slotKeys.Length; i++)
        {
            if (Input.GetKeyDown(slotKeys[i]))
                CurrentSet?.GetAbility(i)?.TryUse(gameObject);
        }
    }

    private void CycleMode()
    {
        if (abilitySets.Length == 0) return;
        currentSetIndex = (currentSetIndex + 1) % abilitySets.Length;
        Debug.Log($"Switched to mode: {CurrentSet.Mode}");
    }

    public void RebindSlot(int slotIndex, KeyCode newKey)
    {
        if (slotIndex < 0 || slotIndex >= slotKeys.Length) return;
        slotKeys[slotIndex] = newKey;
    }
}
