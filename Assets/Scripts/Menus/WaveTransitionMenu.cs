using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// The wave transition menu (and details menu)
/// Author: Logan (LogChiCha)
/// </summary>
public class WaveTransitionMenu : BaseMenu
{
    // attach the button objects in the inspector to these
     [Header("Wave menu Button References")]
    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;

    [SerializeField] private Button rerollButton;

    [SerializeField] private Button selectButton;

    [SerializeField] private Button nextwaveButton;
    [SerializeField] private Button detailsButton1;
    [SerializeField] private Button detailsButton2;
    [SerializeField] private Button detailsButton3;

    [Header("Details Menu Button References")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button returnButton;

    [Header("Game Object References")] //additional game objects as unity doesnt like the 'Find' calls,
    [SerializeField] private GameObject selectButtonObj;
    [SerializeField] private GameObject nextwaveButtonObj;
    [SerializeField] private GameObject leftButtonObj; //and since the details buttons must be seperate from the rest of the details menu (idk unity is weird)
    [SerializeField] private GameObject rightButtonObj;
    [SerializeField] private GameObject returnButtonObj;
    [SerializeField] private GameObject detailsMenu;


    // VARIABLES
    //button arrays
    private Button[] buttons;
    private Button[] detailbuttons;

    //starting indexes
    private int selectedIndex = -1;
    private int detailSelectedIndex = -1;

    //trashy variables for handle2dnavigation
    private int row1 = 0;
    private int row2 = 1;

    //boolean for details menu
    private bool isDetailsOpen = false;

    //saving a variable to know which open is chosen for future use
    private int chosenIndex = -1;

    //display colours
    private Color orange = new Color(255f/255, 200f/255, 78f/255, 1f);
    private Color blue = new Color(78f/255, 200f/255, 255f/255, 1f);

    // ran on game open (only once)
    protected override void Awake()
    {

        base.Awake();

        //attach any new buttons the this array (be aware of the order for the HandleNavigate method)
        buttons = new[] {option1Button, option2Button, option3Button, rerollButton, selectButton, detailsButton1, detailsButton2, detailsButton3};
        detailbuttons = new[] {leftButton, rightButton, returnButton};

    }

    //function is ran every time menu is open
    protected override void OnOpen()
    {
        //these listeners are in OnOpen rather than Awake as they must be toggled on and off
        buttons[4] = selectButton; //set as 4th due to it swapping with nextwaveButton
       
        // handles select/nextwave
        selectButtonObj.SetActive(true);
        nextwaveButtonObj.SetActive(false);
        
        //toggle off details menu buttons
        leftButtonObj.SetActive(false);
        rightButtonObj.SetActive(false);
        returnButtonObj.SetActive(false);
        detailsMenu.SetActive(false); // (and the menu itself)

        // mass active all listeners for wave menu
        ActiveWaveMenuButtons(true);

        //ensure reset of all pop ups and colours
        ResetDescs();
        ResetColors(orange);

        //reset both movement indexes
        selectedIndex = -1;
        detailSelectedIndex = -1;
    }

    //on every close
    protected override void OnClose()
    {
        if (isDetailsOpen) //closing only details menu
        {
            detailsMenu.SetActive(false);
            isDetailsOpen = false;
            ActiveWaveMenuButtons(true);
            returnButton.onClick.RemoveListener(OnClose);
            leftButtonObj.SetActive(false);
            rightButtonObj.SetActive(false);
            returnButtonObj.SetActive(false);
        }
    }

    // this shit is currently so badly coded, so we should refactor the nav when possible -Logan
    // the way this works is determining what button you are on, and then telling Highlight method which new one to go to
    public override void HandleNavigate(Vector2 direction)
    {
        if (!isDetailsOpen)
        {
            if (direction.y > 0.5f || direction.y < -0.5f) //+up & -down
            {   
                if (selectedIndex == -1) {selectedIndex = 0;}

                if (selectedIndex <= 2)
                {
                    row1=selectedIndex;
                    selectedIndex = row2+2;
                }
                else
                {
                    row2=selectedIndex-2;
                    selectedIndex = row1;
                }
            }
            else if (direction.x > 0.5f ) //+right
            {
                if (selectedIndex <= 2)
                {
                    row1 = (row1 + 1) % 3;
                    selectedIndex = row1;
                }
                else
                {
                    row2 = (row2 % 2) + 1;
                    selectedIndex = row2 + 2;
                }
            }
            else if (direction.x < -0.5f) //-left
            {
                if (selectedIndex == -1) {row1 = 2; selectedIndex = row1;}
                else if (selectedIndex <= 2)
                {
                    row1 = (row1 + 2) % 3;
                    selectedIndex = row1;
                }
                else
                {
                    row2 = (row2 % 2) + 1;
                    selectedIndex = row2 + 2;
                }
            }
            HighlightButton(selectedIndex);
        }
        else //detail menu open
        {
            if (direction.y > 0.5f || direction.y < -0.5f)
            {
                if (detailSelectedIndex == 2)
                    detailSelectedIndex = 0;
                else
                    detailSelectedIndex = 2;
            }
            else if (direction.x < -0.5f || direction.x > 0.5f )
            {
                if (detailSelectedIndex == 1)
                    detailSelectedIndex = 0;
                else
                    detailSelectedIndex = 1;
            }
            HighlightButton(detailSelectedIndex);
        }
    }

    // when the enter/space key is hit on a button, run what would be the same as clicking the button
    public override void HandleSubmit()
    {
            if (!isDetailsOpen && selectedIndex != -1)
                buttons[selectedIndex].onClick.Invoke();
            else if (isDetailsOpen && detailSelectedIndex != -1) // details open
                detailbuttons[detailSelectedIndex].onClick.Invoke();

    }

    // when escape key is hit (just runs OnClose)
    public override void HandleCancel()
    {
        OnClose();
    }

    //highlight whichever button given index is
    private void HighlightButton(int index)
    {
        ResetDescs(); //reset all descs
        if(isDetailsOpen)
            EventSystem.current.SetSelectedGameObject(detailbuttons[index].gameObject);
        else if (index >= 0 && index <= 2)
        {
            GameObject.Find("Description" + (index + 1)).GetComponent<Canvas>().enabled = true; //open a description of on one of the options
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        }
        else if (index >= 0)
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    // Helper Method: disabling all the descriptions in the wave menu
    private void ResetDescs()
    {
        GameObject.Find("Description1").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Description2").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Description3").GetComponent<Canvas>().enabled = false;
    }

    // Helper Method: set all the option background colors to a given color
    private void ResetColors(Color color)
    {
        GameObject.Find("BG1").GetComponent<Image>().color = color;
        GameObject.Find("BG2").GetComponent<Image>().color = color;
        GameObject.Find("BG3").GetComponent<Image>().color = color;
    }

    // Helper Method: for choosing an option, make chosen option disaplyed, and gray out all others
    private void ChosenColors(int index)
    {
        ResetColors(Color.gray);
        GameObject.Find("BG"+(index+1)).GetComponent<Image>().color = blue;
    }

    //Helper Method: A way to mass add or remove the listeners to all the buttons in the wave menu
    private void ActiveWaveMenuButtons(bool able)
    {
        if (able)
        {
            option1Button.onClick.AddListener(OnOption1);
            option2Button.onClick.AddListener(OnOption2);
            option3Button.onClick.AddListener(OnOption3);
            rerollButton.onClick.AddListener(OnReroll);
            selectButton.onClick.AddListener(OnSelect);
            nextwaveButton.onClick.AddListener(OnNextWave);
            detailsButton1.onClick.AddListener(OpenDetails);
            detailsButton2.onClick.AddListener(OpenDetails);
            detailsButton3.onClick.AddListener(OpenDetails);
        }
        else if (!able)
        {
            option1Button.onClick.RemoveListener(OnOption1);
            option2Button.onClick.RemoveListener(OnOption2);
            option3Button.onClick.RemoveListener(OnOption3);
            rerollButton.onClick.RemoveListener(OnReroll);
            selectButton.onClick.RemoveListener(OnSelect);
            nextwaveButton.onClick.RemoveListener(OnNextWave);
            detailsButton1.onClick.RemoveListener(OpenDetails);
            detailsButton2.onClick.RemoveListener(OpenDetails);
            detailsButton3.onClick.RemoveListener(OpenDetails);
        }
        
    }

    // BUTTON METHODS
    // When the player chooses an option, show that button as the selected one
    private void OnOption(int index)
    {
        // set index to option location to make navigation feel more normal
        selectedIndex = index;
        row1 = index;

        Debug.Log("[WaveTransitionMenu] Option "+(index+1)+" button clicked");
        ResetColors(orange);
        HighlightButton(selectedIndex);
        if (selectedIndex >= 0 && selectedIndex <= 2)
        {
           GameObject.Find("BG" + (selectedIndex + 1)).GetComponent<Image>().color = blue;
        }
        chosenIndex = selectedIndex;
    }

    // all three here are to make the listeners to these buttons be able to have different indexes
    private void OnOption1() {OnOption(0);}
    private void OnOption2() {OnOption(1);}
    private void OnOption3() {OnOption(2);}

    // next wave is the way to actual way to continue out of the menu, must select first
    private void OnNextWave()
    {

        Debug.Log("[WaveTransitionMenu] Next wave button clicked, chosen index: " + chosenIndex);
        ActiveWaveMenuButtons(false); 
        MenuManager.Instance.CloseMenu();
    }

    // when the player hits the select button, must then swap out to the nextwave
    private void OnSelect()
    {
        if (chosenIndex != -1) //ensures the player must choose something
        {
            Debug.Log("[WaveTransitionMenu] Select button clicked");
            selectButtonObj.SetActive(false); //swap for nextwaveButton
            nextwaveButtonObj.SetActive(true);

            buttons[4]=nextwaveButton; //swap in buttons array
            HighlightButton(selectedIndex); // for the feeling of menu navigation, the highlight should be on the new nextwaveButton
            option1Button.onClick.RemoveListener(OnOption1); 
            option2Button.onClick.RemoveListener(OnOption2); // remove the ability to choose a new option, no takesies backsies
            option3Button.onClick.RemoveListener(OnOption3);
            rerollButton.onClick.RemoveListener(OnReroll); // and no reroll
            ChosenColors(chosenIndex); // set up new color display
        }

    }

    // reroll (as of right now) simply reset any choice that was made, proper reroll will be added once item rando is in
    private void OnReroll()
    {
        Debug.Log("[WaveTransitionMenu] Reroll button clicked");
        HighlightButton(selectedIndex);
        chosenIndex = -1;
        ResetColors(orange);

    }

    // opening the details menu, cant be a seperate prefab cus unity hates you and i
    private void OpenDetails()
    {
        Debug.Log("[WaveTransitionMenu] Details button clicked");
        //toggle on detail menu functions
        detailsMenu.SetActive(true);
        isDetailsOpen = true;
        leftButtonObj.SetActive(true);
        rightButtonObj.SetActive(true);
        returnButtonObj.SetActive(true);
        returnButton.onClick.AddListener(OnClose);

        //disable the wave menu buttons
        ActiveWaveMenuButtons(false);

    }
}
    
