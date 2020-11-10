/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * SelectManager.cs
 * Manages all card selection actions in Phase 4.
 ///////////////////////////////////////////////////////////*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public const string SELECTABLE_TAG = "Selectable";
    public const string SELECTABLE_MARKET_TAG = "SelectableMarket";
    public const string PRODUCIBLE_TAG = "Producible";
    public const string CHAINABLE_TAG = "Chainable";

    private GameObject gameManager;
    private GameObject addManager;
    private GameObject deckManager;
    private GameObject selectedCardOutline;

    private bool selected = false;
    private int selectedID;
    private string selectedResource;
    private string selectedProduct;

    private int playerStone = 0;
    private int playerClay = 0;
    private int playerWheat = 0;
    private int playerCotton = 0;
    private int playerWood = 0;

    //use in future iteration for production chains
    //private int playerCoal = 0;
    //private int playerIron = 0;
    //private int playerFlour = 0;
    //private int playerCattle = 0;
    //private int playerCloth = 0;
    //private int playerPlank = 0;
    //private int playerGlass = 0;
    //private int playerBread = 0;
    //private int playerLeather = 0;

    private Dictionary<int, string> selectedCardInfo = new Dictionary<int, string>();
    //private Dictionary<int, FactoryActivationRequirements> factoryActiveReq;
    //private Dictionary<int, FactoryChainRequirements> factoryChainReq;

    void Awake()
    {
        //assigns the GameManager and AddressablesManager GameObjects
        gameManager = GameObject.Find("GameManager");
        addManager = GameObject.Find("AddressablesManager");
        deckManager = GameObject.Find("DeckManager");
        //factoryActiveReq = gameManager.GetComponent<GameManager>().factoryActiveReq;
        //factoryChainReq = gameManager.GetComponent<GameManager>().factoryChainReq;

        selectedCardInfo = gameManager.GetComponent<GameManager>().selectedCardInfo;
        selectedID = gameObject.GetComponent<FactoryCard>().id;
        selectedResource = gameObject.GetComponent<FactoryCard>().resource;
        selectedProduct = gameObject.GetComponent<FactoryCard>().product;

        gameObject.AddComponent<Button>();
        Button selectButton = gameObject.GetComponent<Button>();

        selectButton.onClick.AddListener(SelectCard);

        if (gameObject.tag == SELECTABLE_TAG || gameObject.tag == SELECTABLE_MARKET_TAG)
        {
            selectedCardOutline = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().cardOutline);
            selectedCardOutline.name = $"OutlineID{selectedID}";
            selectedCardOutline.transform.SetParent(gameObject.transform);
            selectedCardOutline.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 40f);
            selectedCardOutline.transform.localPosition = new Vector2(0f, 0f);
            selectedCardOutline.SetActive(false);
        }

        else
        {
            Debug.Log("Not a resource. No card selection outline required.");
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //determines which action to take depending on whether selected card is a resource or factory
    private void SelectCard()
    {        
        if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseFour)
        {
            if (gameObject.tag == SELECTABLE_TAG || gameObject.tag == SELECTABLE_MARKET_TAG)
            {
                SelectResource();
            }

            else if (gameObject.tag == PRODUCIBLE_TAG)
            {
                Produce();
            }

            else if (gameObject.tag == CHAINABLE_TAG)
            {
                Chain();
            }
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //selects or deselects the clicked resource card
    private void SelectResource()
    {
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        List<GameObject> selectedDeck = deckManager.GetComponent<DeckManager>().selectedDeck;

        if (selected == false)
        {
            if (gameObject.tag == SELECTABLE_TAG)
            {
                selectedDeck.Add(gameObject);
                playerDeck.Remove(gameObject);
            }

            else
            {
                Debug.Log("This card is in the marketplace. Still available for use (and selected)!");
            }

            selectedCardOutline.SetActive(true);
            selectedCardInfo.Add(selectedID, selectedResource);
            selected = true;

            foreach (KeyValuePair<int, string> selectedCard in selectedCardInfo)
            {
                Debug.Log("Key: " + selectedCard.Key + ", Value: " + selectedCard.Value);
            }
        }

        else if (selected)
        {
            if (gameObject.tag == SELECTABLE_TAG)
            {
                playerDeck.Add(gameObject);
                selectedDeck.Remove(gameObject);
            }

            else
            {
                Debug.Log("This card is in the marketplace. Still available for use (and deselected).");
            }

            selectedCardOutline.SetActive(false);
            selectedCardInfo.Remove(selectedID);
            selected = false;

            foreach (KeyValuePair<int, string> selectedCard in selectedCardInfo)
            {
                Debug.Log("Key: " + selectedCard.Key + ", Value: " + selectedCard.Value);
            }
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //+1 Resource to factory storage if production activation requirements are met
    private void Produce()
    {
        List<GameObject> marketDeck = deckManager.GetComponent<DeckManager>().marketDeck;
        List<GameObject> discardPile = deckManager.GetComponent<DeckManager>().discardPile;
        List<GameObject> selectedDeck = deckManager.GetComponent<DeckManager>().selectedDeck;

        foreach (KeyValuePair<int, string> selectedCard in selectedCardInfo)
        {
            string resource = selectedCard.Value;
            switch (resource)
            {
                case "Stone":
                    playerStone += 1;
                    break;
                case "Clay":
                    playerClay += 1;
                    break;
                case "Wheat":
                    playerWheat += 1;
                    break;
                case "Cotton":
                    playerCotton += 1;
                    break;
                case "Wood":
                    playerWood += 1;
                    break;
                default:
                    Debug.Log("Not a resource!");
                    break;
            }
        }

        int remainingStone = gameObject.GetComponent<FactoryCard>().reqStone - playerStone;
        int remainingClay = gameObject.GetComponent<FactoryCard>().reqClay - playerClay;
        int remainingWheat = gameObject.GetComponent<FactoryCard>().reqWheat - playerWheat;
        int remainingCotton = gameObject.GetComponent<FactoryCard>().reqCotton - playerCotton;
        int remainingWood = gameObject.GetComponent<FactoryCard>().reqWood - playerWood;

        //if all resource requirements have been fulfilled, +1 Resource to factory storage
        //...you can also use more than the requirements...
        //...but why would you want to spend more resources than necessary, though?
        if (remainingStone <= 0 && remainingClay <= 0 && remainingWheat <= 0 && remainingCotton <= 0 && remainingWood <= 0)
        {
            for (int i = 0; i < 2; i++)
            {
                var newProduct = new GameObject();
                newProduct.name = $"{gameObject.GetComponent<FactoryCard>().name}_Storage_Product";
                newProduct.AddComponent<Image>();
                newProduct.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().product;
                newProduct.transform.SetParent(GameObject.Find($"{gameObject.GetComponent<FactoryCard>().name}_Storage").GetComponent<Transform>());
            }

            var inProduction = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().inProduction);
            inProduction.name = $"{gameObject.GetComponent<FactoryCard>().name}_In_Production";
            inProduction.transform.SetParent(GameObject.Find($"{gameObject.GetComponent<FactoryCard>().name}").GetComponent<Transform>());
            inProduction.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 13f);
            inProduction.GetComponent<RectTransform>().localPosition = new Vector2(0f, -19f);

            Debug.Log($"{gameObject.GetComponent<FactoryCard>().name}_Storage");

            int selectedDeckLength = selectedDeck.Count;

            for (int i = selectedDeckLength - 1; i >= 0; i--)
            {
                selectedDeck[i].GetComponent<SelectManager>().selected = false;
                Destroy(selectedDeck[i].GetComponent<SelectManager>().selectedCardOutline);

                selectedDeck[i].transform.SetParent(GameObject.FindWithTag("DiscardPile").GetComponent<Transform>());
                selectedDeck[i].GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().spriteClosed;
                selectedDeck[i].GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 55f);
                selectedDeck[i].GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
                selectedDeck[i].tag = "Untagged";

                discardPile.Add(selectedDeck[i]);
                selectedDeck.Remove(selectedDeck[i]);
            }

            foreach (GameObject card in marketDeck)
            {
                card.GetComponent<SelectManager>().selected = false;
                card.GetComponent<SelectManager>().selectedCardOutline.SetActive(false);
            }

            selectedCardInfo.Clear();
            playerStone = 0;
            playerClay = 0;
            playerWheat = 0;
            playerCotton = 0;
            playerWood = 0;
            gameObject.tag = CHAINABLE_TAG;
        }

        else
        {
            playerStone = 0;
            playerClay = 0;
            playerWheat = 0;
            playerCotton = 0;
            playerWood = 0;
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //+1 Resource to factory storage if production chain requirements are met
    //is a repeatable action
    private void Chain()
    {
        Debug.Log("Chain stuff goes here!");
    }
}
