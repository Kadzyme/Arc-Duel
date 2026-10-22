using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUIController : MonoBehaviour
{

    //[SerializeField] private SettingsController settings;
    [SerializeField] private string gameSceneName = "Game";

    private VisualElement root;
    private Button playButton, settingsButton, quitButton;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");

        playButton.clicked += OnPlay;
        //settingsButton.clicked += OnSettings;
        quitButton.clicked += OnQuit;
    }

    private void OnDisable()
    {
        playButton.clicked -= OnPlay;
        //settingsButton.clicked -= OnSettings;
        quitButton.clicked -= OnQuit;
    }

    private void OnPlay() => SceneManager.LoadScene(gameSceneName);

    //private void OnSettings() => settings.Show();

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
