using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RebindUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject buttonPrefab;

    private Dictionary<PlayerAction, Button> buttons = new Dictionary<PlayerAction, Button>();

    private void Start()
    {
        foreach (var binding in InputManager.Instance.Bindings.GetAllBindings())
        {
            GameObject btnObj = Instantiate(buttonPrefab, contentParent);
            Button btn = btnObj.GetComponent<Button>();
            Text text = btnObj.GetComponentInChildren<Text>();
            text.text = $"{binding.action}: {binding.key}";

            PlayerAction action = binding.action;
            btn.onClick.AddListener(() => StartRebind(action, text));

            buttons[action] = btn;
        }
    }

    private void StartRebind(PlayerAction action, Text displayText)
    {
        displayText.text = $"{action}: ...";
        StartCoroutine(WaitForKey(action, displayText));
    }

    private System.Collections.IEnumerator WaitForKey(PlayerAction action, Text displayText)
    {
        bool keyPressed = false;
        while (!keyPressed)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    InputManager.Instance.SetKey(action, key);
                    displayText.text = $"{action}: {key}";
                    keyPressed = true;
                    break;
                }
            }
            yield return null;
        }
    }
}
