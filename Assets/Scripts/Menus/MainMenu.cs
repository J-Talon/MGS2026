using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// The main menu
/// </summary>
public class MainMenu : BaseMenu
{
     [Header("Button References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private int selectedIndex = 0;
    private Button[] buttons;

    private GameObject EventSystem;

    protected override void Awake()
    {
        EventSystem = GameObject.Find("EventSystem");

        base.Awake();

        buttons = new[] { playButton, quitButton};


        playButton.onClick.AddListener(OnPlay);
        //EXAMPLE: settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(OnQuit);
    }

    protected override void OnOpen()
    {
        Debug.LogFormat($"Main menu opened");
        StartCoroutine(DelaySelect());
    }

 protected override void OnClose()
    {
    }

    public override void Hide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public override void Show()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public override void HandleNavigate(Vector2 direction)
    {
        if (direction.y > 0.5f)
        {
            selectedIndex = 0; 
            HighlightButton(selectedIndex);
        }
        else if (direction.y < -0.5f)
        {
            selectedIndex = 1;
            HighlightButton(selectedIndex);
        }
        Debug.Log("Nav in Main Menu, selected"+selectedIndex);
    }

    public override void HandleSubmit()
    {
        var button = buttons[selectedIndex];
        button.onClick.Invoke();
        Debug.Log("button clicked: " + button.name);
    }

    public override void HandleCancel()
    {
        //none, is main menu
    }

    private IEnumerator DelaySelect()
    {
        yield return null;
        HighlightButton(selectedIndex);
    }

    private void HighlightButton(int index)
    {
        // work around
        // In the event system game object.. Make sure it cannot send navigation events.. thanks.
        var ev = EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>();
        ev.SetSelectedGameObject(buttons[index].gameObject);
    }

    /* EXAMPLE
   private void OpenSettings()
    {
        Debug.Log("Opening settings menu...");
        MenuManager.Instance.OpenSettings();
    }*/

    private void OnPlay()
    {
        //gameSettings.gameMode = gameModeDropdown.value;
        var menuManager = MenuManager.Instance;
        menuManager.CloseMenu();

        SceneManager.LoadScene("GridTestScene");
    }
    
        private void OnQuit()
    {
        Debug.Log("Quitting game...");
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}