using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilitySet abilitySet;
    [SerializeField] private Ability blinkAbility;

    private void Update()
    {
        if (abilitySet != null)
        {
            int slotCount = abilitySet.SlotCount;
            for (int i = 0; i < slotCount; i++)
            {
                PlayerAction action = GetPlayerActionForSlot(i);
                if (Input.GetKeyDown(InputManager.Instance.GetKey(action)))
                    abilitySet.GetAbility(i)?.TryUse(gameObject);
            }
        }

        if (blinkAbility != null && Input.GetKeyDown(InputManager.Instance.GetKey(PlayerAction.InheritAbility)))
            blinkAbility.TryUse(gameObject);
    }

    private PlayerAction GetPlayerActionForSlot(int slotIndex)
    {
        return (PlayerAction)((int)PlayerAction.Ability1 + slotIndex);
    }
}