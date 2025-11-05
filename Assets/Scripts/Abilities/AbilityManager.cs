using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilitySet[] abilitySets;
    [SerializeField] private Ability inheritAbility;

    private int currentSetIndex = 0;
    private AbilitySet CurrentSet => abilitySets != null && abilitySets.Length > 0 ? abilitySets[currentSetIndex] : null;

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetKey(PlayerAction.SwitchSet)))
            CycleMode();

        if (CurrentSet != null)
        {
            int slotCount = CurrentSet.SlotCount;
            for (int i = 0; i < slotCount; i++)
            {
                PlayerAction action = GetPlayerActionForSlot(i);
                if (Input.GetKeyDown(InputManager.Instance.GetKey(action)))
                    CurrentSet.GetAbility(i)?.TryUse(gameObject);
            }
        }

        if (inheritAbility != null && Input.GetKeyDown(InputManager.Instance.GetKey(PlayerAction.InheritAbility)))
            inheritAbility.TryUse(gameObject);
    }

    private void CycleMode()
    {
        if (abilitySets == null || abilitySets.Length == 0) return;
        currentSetIndex = (currentSetIndex + 1) % abilitySets.Length;
    }

    public void SetInheritAbility(Ability newAbility) => inheritAbility = newAbility;

    private PlayerAction GetPlayerActionForSlot(int slotIndex)
    {
        return (PlayerAction)((int)PlayerAction.Ability1 + slotIndex);
    }
}
