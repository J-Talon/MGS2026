using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// The main menu
/// Author: Logan (LogChiCha)
/// </summary>
public class MainMenu : BaseMenu
{
    // attach the button objects in the inspector to these
     [Header("Button References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    //starting index
    private int selectedIndex = 0;
    private Button[] buttons;
    private GameObject EventSystem;

    protected override void Awake()
    {
        EventSystem = GameObject.Find("EventSystem");

        base.Awake();

        //attach any new buttons the this array (be aware of the order for the HandleNavigate method)
        buttons = new[] { playButton, settingsButton, creditsButton, quitButton};


        // attach a click action to the button to do what is in the method
        playButton.onClick.AddListener(OnPlay);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
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

    // hiding the menu
    public override void Hide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    //showing the menu
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
        if (direction.y > 0.5f) //up
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
        }
        else if (direction.y < -0.5f) //down
        {
            selectedIndex =  (selectedIndex + 1) % buttons.Length;
        }
        else if (direction.x > 0.5f) //right
        {
        }
        else if (direction.x < -0.5f) //left
        {
        }
        // do the highlight of the new selected button
        HighlightButton(selectedIndex);
        Debug.Log("Nav in Main Menu, selected"+selectedIndex);
    }

    // when the enter/space key is hit on a button, run what would be the same as clicking the button
    public override void HandleSubmit()
    {
        var button = buttons[selectedIndex];
        button.onClick.Invoke();
        Debug.Log("button clicked: " + button.name);
    }

    // when escape key is hit (should close menu, but this is main so no functionality here)
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


   private void OpenSettings()
    {
        Debug.Log("Opening settings menu...");
        MenuManager.Instance.OpenSettings();
    }

    private void OpenCredits()
    {
        Debug.Log("Opening credits menu...");
        MenuManager.Instance.OpenCredits();
    }

    //play button
    private void OnPlay()
    {
        var menuManager = MenuManager.Instance;
        menuManager.CloseMenu();

        //load new scene (scene must be in build settings, go to File > Build Profile > Scene List)
        SceneManager.LoadScene("GridTestScene");
    }
    
    //quit button
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