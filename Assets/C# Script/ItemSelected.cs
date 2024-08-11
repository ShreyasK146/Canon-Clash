
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class ItemSelected : MonoBehaviour
{
    /// <summary>
    /// here also lots of code repeated code are added for multiplayer
    /// </summary>

    private static ItemSelected currentlySelected;
    private Button itemButton;
    public Item item;
    [HideInInspector] public bool itemSelected = false;
    private GridItems gridItems;
    private GameMechanics gameMechanics;
    private TextMeshProUGUI itemText;
    private Color originalTextColor;
    private Color selectedTextColor = new Color(0.0f, 0.3451f, 1.0f, 1.0f);
    private float selectedFontSize = 20f;
    private float originalFontSize = 15f;


    //private Button randomize;
    private void Start()
    {

        // to highlight the selected item
        gameMechanics = GameObject.Find("GameMechanics").GetComponent<GameMechanics>();
        if (gameMechanics.offlineMode)
        {
            //randomize = GameObject.Find("Randomize").GetComponent<Button>();
            itemButton = GetComponent<Button>();

            itemText = itemButton.GetComponentInChildren<TextMeshProUGUI>();
            originalTextColor = itemText.color;


            itemButton.onClick.AddListener(OnButtonClick);
            gridItems = GameObject.Find("InventoryUI").GetComponent<GridItems>();
        }

        else
        {
            if (PhotonNetwork.IsConnected)
            {
               
                itemButton = GetComponent<Button>();

                itemText = itemButton.GetComponentInChildren<TextMeshProUGUI>();
                originalTextColor = itemText.color;

                itemButton.onClick.AddListener(OnButtonClick2);
                gridItems = GameObject.Find("InventoryUI").GetComponent<GridItems>();
            }
        }

    }
    // when the item is selected 
    private void OnButtonClick()
    {
       // Debug.Log("-----" + gridItems.playerTurnToSelect);
        if (gridItems.playerTurnToSelect && !gridItems.randomize.gameObject.activeSelf)
        {
            //Debug.Log("hellooooo2");

            if (currentlySelected != null)
            {
                // Reset the color and font of the previously selected item
                currentlySelected.ResetOriginalColorAndFont();
            }


            itemText.color = selectedTextColor;
            itemText.fontSize = selectedFontSize;

            currentlySelected = this;

            GameObject.Find("SelectButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("SelectButton").GetComponent<Button>().onClick.AddListener(OnItemSelected2);
        }

    }
    public void OnItemSelected()
    {
        //Debug.Log("helloloooo3");



        gridItems.PopulatePlayerGrid(gameObject);
        itemText.color = originalTextColor;
        itemText.fontSize = originalFontSize;
    }


    private void OnButtonClick2()
    {

       // Debug.Log("i got clicked");
        //Debug.Log("is master = " + PhotonNetwork.IsMasterClient + "\n" + "player1turntoselect = " + gridItems.player1TurnToSelect + "\n" + "player2turntoselect= " + gridItems.player2TurnToSelect + "\n" + "randomize not active? = " + !gridItems.randomize.gameObject.activeSelf);
        if (PhotonNetwork.IsMasterClient && gridItems.player1TurnToSelect && !gridItems.randomize.gameObject.activeSelf && !gridItems.player1buttonselected)
        {
            if (currentlySelected != null)
            {
                // Reset the color and font of the previously selected item
                currentlySelected.ResetOriginalColorAndFont();
            }

            itemText.color = selectedTextColor;
            itemText.fontSize = selectedFontSize;

            currentlySelected = this;



            GameObject.Find("SelectButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("SelectButton").GetComponent<Button>().onClick.AddListener(OnItemSelected2);

        }
        else if (!PhotonNetwork.IsMasterClient && gridItems.player2TurnToSelect && !gridItems.randomize.gameObject.activeSelf && !gridItems.player2buttonselected)
        {
            if (currentlySelected != null)
            {
                // Reset the color and font of the previously selected item
                currentlySelected.ResetOriginalColorAndFont();
            }

            itemText.color = selectedTextColor;
            itemText.fontSize = selectedFontSize;

            currentlySelected = this;


            GameObject.Find("SelectButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("SelectButton").GetComponent<Button>().onClick.AddListener(OnItemSelected2);

        }
    }

    public void OnItemSelected2()
    {
       // Debug.Log("even i got clicked");

        if (gridItems.player1TurnToSelect)
            gridItems.player1buttonselected = true;
        else if (gridItems.player2TurnToSelect)
            gridItems.player2buttonselected = true;

        gridItems.PopulatePlayerGrid(gameObject);
        itemText.color = originalTextColor;
        itemText.fontSize = originalFontSize;
    }

    private void ResetOriginalColorAndFont()
    {
        itemText.color = originalTextColor;
        itemText.fontSize = originalFontSize;
    }

    
}

