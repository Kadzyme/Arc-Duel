using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputBindings Bindings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBindings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public KeyCode GetKey(PlayerAction action) => Bindings.GetKey(action);

    public void SetKey(PlayerAction action, KeyCode key)
    {
        Bindings.SetKey(action, key);
        PlayerPrefs.SetInt(action.ToString(), (int)key);
        PlayerPrefs.Save();
    }

    private void LoadBindings()
    {
        foreach (PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
        {
            if (PlayerPrefs.HasKey(action.ToString()))
            {
                KeyCode key = (KeyCode)PlayerPrefs.GetInt(action.ToString());
                Bindings.SetKey(action, key);
            }
        }
    }
}
