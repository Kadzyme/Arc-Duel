using UnityEngine;
using System.Collections.Generic;

public enum PlayerAction
{
    MoveLeft,
    MoveRight,
    Dash,
    Jump,
    Ability1,
    Ability2,
    Ability3,
    InheritAbility,
    SwitchSet
}

[CreateAssetMenu(menuName = "Settings/Input Bindings")]
public class InputBindings : ScriptableObject
{
    [SerializeField] private List<PlayerAction> actions = new List<PlayerAction>();
    [SerializeField] private List<KeyCode> keys = new List<KeyCode>();

    private Dictionary<PlayerAction, KeyCode> bindings;

    private void OnEnable()
    {
        bindings = new Dictionary<PlayerAction, KeyCode>();
        for (int i = 0; i < actions.Count; i++)
        {
            if (!bindings.ContainsKey(actions[i]))
                bindings[actions[i]] = keys[i];
        }
    }

    public KeyCode GetKey(PlayerAction action)
    {
        if (bindings.TryGetValue(action, out KeyCode key))
            return key;
        return KeyCode.None;
    }

    public void SetKey(PlayerAction action, KeyCode key)
    {
        if (bindings.ContainsKey(action))
            bindings[action] = key;
        else
        {
            bindings[action] = key;
            actions.Add(action);
            keys.Add(key);
        }
    }

    public IEnumerable<(PlayerAction action, KeyCode key)> GetAllBindings()
    {
        foreach (var kvp in bindings)
            yield return (kvp.Key, kvp.Value);
    }
}
