/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/
 
/*////////////////////////////////////////////////////////////
 * GameManager.cs
 * Manages all game phase actions (including game setup).
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Phase
{
    None,
    Setup,
    PhaseOne,
    PhaseTwo,
    PhaseThree,
    PhaseFour,
    EndOfRound
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject deckManager;
    [SerializeField] GameObject addManager;
    public Phase currentPhase = Phase.None;

    private int sunCount = 0;
    private int cardsToDiscardCount = 0;

    private int playerVictoryPoints = 0;

    private Dictionary<string, int> playerResources = new Dictionary<string, int>();

    public Dictionary<int, string> selectedCardInfo = new Dictionary<int, string>();

    void Start()
    {
        //sets up the draw pile
        //prepares the draw pile as a Button for use in Phase 2 and 3
        int startingDrawPileSize = GameObject.FindWithTag("DeckManager").GetComponent<DeckManager>().drawPile.Count;
        GameObject.FindWithTag("DrawPile").AddComponent<Button>();
        GameObject.FindWithTag("DrawPile").GetComponent<Button>().onClick.AddListener(DealMarket);
        GameObject.FindWithTag("DrawPile").GetComponent<Button>().enabled = false;

        //shuffles the draw pile
        Shuffle(GameObject.FindWithTag("DeckManager").GetComponent<DeckManager>().drawPile, startingDrawPileSize);

        //proceeds to Setup
        currentPhase = Phase.Setup;
        PhaseTracker(currentPhase);

        //adds resource types to dictionary...or hashtable, in this case
        playerResources.Add("Stone", 0);
        playerResources.Add("Clay", 0);
        playerResources.Add("Wheat", 0);
        playerResources.Add("Cotton", 0);
        playerResources.Add("Wood", 0);
        playerResources.Add("Coal", 0);
        playerResources.Add("Iron", 0);
        playerResources.Add("Flour", 0);
        playerResources.Add("Cattle", 0);
        playerResources.Add("Cloth", 0);
        playerResources.Add("Plank", 0);
        playerResources.Add("Glass", 0);
        playerResources.Add("Bread", 0);
        playerResources.Add("Leather", 0);
        playerResources.Add("Brick", 0);
        playerResources.Add("Rations", 0);
        playerResources.Add("Shoes", 0);
        playerResources.Add("Shirt", 0);
        playerResources.Add("Beef", 0);
        playerResources.Add("Tools", 0);
        playerResources.Add("Barrel", 0);
        playerResources.Add("Window", 0);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //shuffles the card deck based on the Fisher-Yates shuffle algorithm
    public void Shuffle(List<GameObject> cardDeck, int startingDrawPileSize)
    {
        Debug.Log("Shuffling cards...");
        for (int i = startingDrawPileSize - 1; i >= 0; --i)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            var tmp = cardDeck[i];
            cardDeck[i] = cardDeck[r];
            cardDeck[r] = tmp;
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //tracks which phase the game is currently on
    private void PhaseTracker(Phase currentPhase)
    {
        switch (currentPhase)
        {
            case Phase.Setup:
                GameObject.FindWithTag("CurrentPhase").GetComponent<Text>().text = "SETUP";
                StartCoroutine(StartingCards());
                break;
            case Phase.PhaseOne:
                GameObject.FindWithTag("CurrentPhase").GetComponent<Text>().text = "1";
                DiscardAllCardsPrompt();
                break;
            case Phase.PhaseTwo:
                GameObject.FindWithTag("CurrentPhase").GetComponent<Text>().text = "2";
                MarketSetup();
                break;
            case Phase.PhaseThree:
                GameObject.FindWithTag("CurrentPhase").GetComponent<Text>().text = "3";
                MarketSetup();
                break;
            case Phase.PhaseFour:
                GameObject.FindWithTag("CurrentPhase").GetComponent<Text>().text = "4";
                ActivateProductionProcesses();
                break;
            case Phase.EndOfRound:
                //implement in future iterations, please
                break;
            default:
                break;
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //deals Worker, Charburner, 5 Resource cards, and 7 Goods cards on Charburner to player
    private IEnumerator StartingCards()
    {
        yield return new WaitForSeconds(0.5f);
        
        List<GameObject> factoryDeck = deckManager.GetComponent<DeckManager>().factoryDeck;
        List<GameObject> drawPile = deckManager.GetComponent<DeckManager>().drawPile;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        List<GameObject> workerDeck = deckManager.GetComponent<DeckManager>().workerDeck;

        //deals 1 Worker card
        GameObject startingWorkerCard = new GameObject();
        startingWorkerCard.name = "Worker_Starting";
        startingWorkerCard.tag = "DraggableWorker";
        startingWorkerCard.AddComponent<Image>();
        startingWorkerCard.AddComponent<WorkerCard>();
        startingWorkerCard.AddComponent<DragManager>();
        startingWorkerCard.GetComponent<DragManager>().enabled = false;
        startingWorkerCard.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().spriteWorker;
        startingWorkerCard.transform.SetParent(GameObject.Find("WorkerSlot_1").GetComponent<Transform>());
        startingWorkerCard.GetComponent<RectTransform>().sizeDelta = new Vector2(35f, 20f);
        startingWorkerCard.transform.localPosition = new Vector2(0f, 0f);
        workerDeck.Add(startingWorkerCard);
        
        yield return new WaitForSeconds(0.5f);

        //deals 1 Charburner card
        GameObject startingCharburnerCard = deckManager.GetComponent<DeckManager>().charburnerDeck[Random.Range(0,3)];
        factoryDeck.Add(startingCharburnerCard);
        startingCharburnerCard.SetActive(true);
        startingCharburnerCard.name = "Factory_Charburner";
        startingCharburnerCard.tag = "DroppableFactory";
        startingCharburnerCard.transform.SetParent(GameObject.FindWithTag("FactoryDeck").GetComponent<Transform>());
        startingCharburnerCard.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 55f);

        //sets up goods storage on Charburner
        GameObject productStorage = new GameObject();
        productStorage.name = "Factory_Charburner_Storage";
        productStorage.AddComponent<GridLayoutGroup>();
        productStorage.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 55f);
        productStorage.transform.position = new Vector2(35f, 0f);
        productStorage.transform.SetParent(GameObject.Find("Factory_Charburner").GetComponent<Transform>());
        productStorage.GetComponent<GridLayoutGroup>().cellSize = new Vector2(5f, 5f);
        productStorage.GetComponent<GridLayoutGroup>().spacing = new Vector2(1f, 1f);

        yield return new WaitForSeconds(0.5f);

        //deals 5 Resource cards to player
        for (int i = 0; i < 5; i++)
        {
            playerDeck.Add(drawPile[i]);
            GameObject playerDeckCard = playerDeck[i];
            playerDeckCard.SetActive(true);
            playerDeckCard.GetComponent<DragManager>().enabled = false;
            playerDeckCard.transform.SetParent(GameObject.Find("Slot_" + i.ToString()).GetComponent<Transform>());
            playerDeckCard.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 40f);
            playerDeckCard.transform.localPosition = new Vector2(0f, 0f);
            drawPile.Remove(drawPile[i]);

            yield return new WaitForSeconds(0.3f);
        }

        //deals 7 Goods cards to Charburner
        for (int i = 0; i < 7; i++)
        {
            GameObject newProduct = new GameObject();
            newProduct.name = "Factory_Charburner_Storage_Product";
            newProduct.AddComponent<Image>();
            newProduct.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().product;
            newProduct.transform.SetParent(GameObject.Find("Factory_Charburner_Storage").GetComponent<Transform>());
            playerVictoryPoints += 7;

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        //proceeds to Phase One
        currentPhase = Phase.PhaseOne;
        PhaseTracker(currentPhase);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //asks player if they want to discard all cards in their hand or not
    private void DiscardAllCardsPrompt()
    {
        GameObject discardAllPrompt = new GameObject();
        discardAllPrompt.name = "DiscardAllPrompt";
        discardAllPrompt.GetComponent<Transform>().SetParent(GameObject.Find("GameGUI").GetComponent<Transform>());

        discardAllPrompt.AddComponent<Text>();
        discardAllPrompt.GetComponent<Text>().font = addManager.GetComponent<AddressablesManager>().font;
        discardAllPrompt.GetComponent<Text>().text = "Discard all the cards in hand?";
        discardAllPrompt.GetComponent<Text>().color = Color.white;
        discardAllPrompt.GetComponent<Text>().fontSize = 30;
        discardAllPrompt.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 40f);
        discardAllPrompt.transform.localPosition = new Vector2(0f, 45f);

        GameObject discardAllYesButton = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().button);
        GameObject discardAllNoButton = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().button);

        discardAllYesButton.name = "DiscardAllYesButton";
        discardAllNoButton.name = "DiscardAllNoButton";
        discardAllYesButton.transform.SetParent(GameObject.Find("GameGUI").transform);
        discardAllNoButton.transform.SetParent(GameObject.Find("GameGUI").transform);
        discardAllYesButton.transform.GetChild(0).GetComponent<Text>().text = "Yes";
        discardAllNoButton.transform.GetChild(0).GetComponent<Text>().text = "No";
        discardAllYesButton.transform.localPosition = new Vector2(-40f, 0f);
        discardAllNoButton.transform.localPosition = new Vector2(40f, 0f);

        discardAllYesButton.GetComponent<Button>().onClick.AddListener(DiscardPromptSelection);
        discardAllNoButton.GetComponent<Button>().onClick.AddListener(NoDiscardPromptSelection);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    
    //either starts DiscardAllCards coroutine or skips to card drawing
    void DiscardPromptSelection()
    {
        Destroy(GameObject.Find("DiscardAllPrompt"));
        Destroy(GameObject.Find("DiscardAllYesButton"));
        Destroy(GameObject.Find("DiscardAllNoButton"));
        StartCoroutine(DiscardAllCards());
    }

    void NoDiscardPromptSelection()
    {
        Destroy(GameObject.Find("DiscardAllPrompt"));
        Destroy(GameObject.Find("DiscardAllYesButton"));
        Destroy(GameObject.Find("DiscardAllNoButton"));
        StartCoroutine(RedrawCards());
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //discard all cards in player deck and redraw the same amount as discarded
    private IEnumerator DiscardAllCards()
    {
        yield return new WaitForSeconds(0.1f);
        
        List<GameObject> discardPile = deckManager.GetComponent<DeckManager>().discardPile;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        cardsToDiscardCount = playerDeck.Count;

        GameObject.FindWithTag("DiscardPile").GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().spriteClosed;

        //discard all cards in player deck
        for (int i = cardsToDiscardCount - 1; i >= 0; i--)
        {
            GameObject currentCardToDiscard = playerDeck[i];
            currentCardToDiscard.transform.SetParent(GameObject.FindWithTag("DiscardPile").GetComponent<Transform>());
            currentCardToDiscard.SetActive(false);
            discardPile.Add(playerDeck[i]);
            playerDeck.Remove(playerDeck[i]);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RedrawCards());
    }
    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //draws the correct amount of cards as discarded previously + 2 additional cards
    private IEnumerator RedrawCards()
    {
        yield return new WaitForSeconds(0.1f);

        //draws same amount of cards as discarded
        StartCoroutine(DrawCards(cardsToDiscardCount));
        yield return new WaitForSeconds(0.3f * cardsToDiscardCount);

        //draws 2 cards
        StartCoroutine(DrawCards(2));
        yield return new WaitForSeconds(0.3f * 2);

        //proceeds to Phase Two
        currentPhase = Phase.PhaseTwo;
        PhaseTracker(currentPhase);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //draw x amount of Resource cards
    private IEnumerator DrawCards(int cardsToDraw)
    {
        List<GameObject> drawPile = deckManager.GetComponent<DeckManager>().drawPile;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;

        for (int i = cardsToDraw - 1; i >= 0; i--)
        {
            int currentSlotToAddTo = playerDeck.Count;
            GameObject currentCardToDraw = drawPile[i];
            currentCardToDraw.SetActive(true);
            currentCardToDraw.GetComponent<DragManager>().enabled = false;
            currentCardToDraw.transform.SetParent(GameObject.Find("Slot_" + currentSlotToAddTo.ToString()).GetComponent<Transform>());
            currentCardToDraw.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 40f);
            currentCardToDraw.transform.localPosition = new Vector2(0f, 0f);
            playerDeck.Add(drawPile[i]);
            drawPile.Remove(drawPile[i]);

            yield return new WaitForSeconds(0.3f);
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //sets up draw pile as a button for marketplace actions
    private void MarketSetup()
    {
        List<GameObject> workerDeck = deckManager.GetComponent<DeckManager>().workerDeck;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        sunCount = 0;

        for (int i = 0; i < workerDeck.Count; i++)
        {
            workerDeck[i].GetComponent<DragManager>().enabled = false;
        }

        for (int i = 0; i < playerDeck.Count; i++)
        {
            playerDeck[i].GetComponent<DragManager>().enabled = false;
        }

        GameObject.FindWithTag("DrawPile").GetComponent<Button>().enabled = true;
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //deals Resource cards to marketplace until 2 half-suns are revealed
    private void DealMarket()
    {
        List<GameObject> drawPile = deckManager.GetComponent<DeckManager>().drawPile;
        List<GameObject> marketDeck = deckManager.GetComponent<DeckManager>().marketDeck;

        if (sunCount < 2 && drawPile.Count > 0)
        {
            GameObject currentCardInMarket = drawPile[0];
            currentCardInMarket.SetActive(true);
            currentCardInMarket.GetComponent<DragManager>().enabled = false;
            currentCardInMarket.transform.SetParent(GameObject.FindWithTag("MarketDeck").GetComponent<Transform>());
            currentCardInMarket.GetComponent<RectTransform>().sizeDelta = new Vector2(25f, 35f);

            marketDeck.Add(drawPile[0]);
            marketDeck[0].GetComponent<DragManager>().enabled = false;

            sunCount += drawPile[0].GetComponent<FactoryCard>().halfSun;
            Debug.Log("Current sun count is: " + sunCount);

            if (drawPile[0].GetComponent<FactoryCard>().halfSun == 1)
            {
                GameObject halfSunSprite = new GameObject();
                halfSunSprite.AddComponent<Image>();
                halfSunSprite.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().halfSun;
                halfSunSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(57f, 29f);

                if (currentPhase == Phase.PhaseTwo)
                {
                    halfSunSprite.transform.SetParent(GameObject.FindWithTag("SunriseGrid").GetComponent<Transform>());
                }

                else if (currentPhase == Phase.PhaseThree)
                {
                    halfSunSprite.transform.SetParent(GameObject.Find("SunsetGrid").GetComponent<Transform>());
                }
            }

            drawPile.Remove(drawPile[0]);

            //proceeds to worker and to-build assignments
            if (sunCount == 2 && currentPhase == Phase.PhaseTwo)
            {
                GameObject.FindWithTag("DrawPile").GetComponent<Button>().enabled = false;
                ActivateAssignWorkerBuild();
            }

            //proceeds to Phase Four
            else if (sunCount == 2 && currentPhase == Phase.PhaseThree)
            {   
                currentPhase = Phase.PhaseFour;
                PhaseTracker(currentPhase);
            }
        }

        else if (sunCount < 2 && drawPile.Count == 0)
        {
            Debug.Log("The number of cards in the Market are " + marketDeck.Count + " card(s)");
            Debug.Log("The deck is empty! Please refill!");
            //do deck refill stuff here
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    
    //activates drag-and-drop for Phase 2:
    //  - Worker -> Factory, only one per Factory
    //  - Player deck cards -> To Build, only one
    private void ActivateAssignWorkerBuild()
    {
        List<GameObject> workerDeck = deckManager.GetComponent<DeckManager>().workerDeck;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        List<GameObject> marketDeck = deckManager.GetComponent<DeckManager>().marketDeck;

        for (int i = 0; i < marketDeck.Count; i++)
        {
            marketDeck[i].tag = "Untagged";
        }

        for (int i = 0; i < workerDeck.Count; i++)
        {
            workerDeck[i].GetComponent<DragManager>().enabled = true;
        }

        for (int i = 0; i < playerDeck.Count; i++)
        {
            playerDeck[i].GetComponent<DragManager>().enabled = true;
        }

        //sets up button for proceeding to Phase 3
        //player doesn't have to assign all workers to factories + assign a building to build
        GameObject toPhaseThreeButton = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().button);    
        toPhaseThreeButton.name = "ToPhaseThreeButton";
        toPhaseThreeButton.transform.SetParent(GameObject.Find("GameGUI").transform);
        toPhaseThreeButton.transform.GetChild(0).GetComponent<Text>().text = "Continue";
        toPhaseThreeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(95f, 25f);
        toPhaseThreeButton.transform.localPosition = new Vector2(101f, 118f);
        toPhaseThreeButton.GetComponent<Button>().onClick.AddListener(CompletePhaseTwo);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //completes Phase 2 and continues to Phase 3
    //add stuff that happens here
    private void CompletePhaseTwo()
    {
        List<GameObject> workerDeck = deckManager.GetComponent<DeckManager>().workerDeck;
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        List<GameObject> toBuild = deckManager.GetComponent<DeckManager>().toBuild;
        
        Destroy(GameObject.Find("ToPhaseThreeButton"));

        for (int i = 0; i < workerDeck.Count; i++)
        {
            workerDeck[i].GetComponent<DragManager>().enabled = false;
            workerDeck[i].tag = "Untagged";
        }

        GameObject toBuildStorage = GameObject.FindWithTag("DroppableBuild");

        if (toBuildStorage.transform.childCount > 0)
        {
            string toBuildName = toBuildStorage.transform.GetChild(0).name;
            int toBuildIndex = -1;

            for (int i = 0; i < playerDeck.Count; i++)
            {
                if (playerDeck[i].name == toBuildName)
                {
                    toBuildIndex = i;
                    Debug.Log("Current index: " + toBuildIndex);
                    break;
                }

                else
                {
                    toBuildIndex = -1;
                }
            }

            Debug.Log("Name of the child object: " + toBuildName);
            Debug.Log("Index of the building to build: " + toBuildIndex);

            if (toBuildIndex >= 0)
            {
                toBuild.Add(playerDeck[toBuildIndex]);
                playerDeck.Remove(playerDeck[toBuildIndex]);
            }

            else if (toBuildIndex < 0)
            {
                Debug.Log("The building you're looking for doesn't exist!");
            }
        }

        else
        {
            Debug.Log("No building to build selected!");
        }

        currentPhase = Phase.PhaseThree;
        PhaseTracker(currentPhase);
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //activates selectables and producibles for Phase 4:
    //  - Player deck cards -> Factory --> + 1 Resource if conditions are met
    //  - Player deck cards -> To Build --> add Factory to FactoryDeck if conditions are met
    private void ActivateProductionProcesses()
    {
        List<GameObject> playerDeck = deckManager.GetComponent<DeckManager>().playerDeck;
        List<GameObject> marketDeck = deckManager.GetComponent<DeckManager>().marketDeck;
        List<GameObject> factoryDeck = deckManager.GetComponent<DeckManager>().factoryDeck;

        GameObject.FindWithTag("DroppableBuild").transform.tag = "Untagged";

        if (GameObject.Find("BuildSlot").transform.childCount > 0)
        {
            GameObject.Find("BuildSlot").transform.GetChild(0).tag = "ToBuild";
        }
        
        else
        {
            Debug.Log("No building to build!");
        }

        //marks available resources in marketplace and player deck as usable for factory production
        for (int i = 0; i < playerDeck.Count; i++)
        {
            playerDeck[i].GetComponent<DragManager>().enabled = false;
            playerDeck[i].tag = "Selectable";
            playerDeck[i].AddComponent<SelectManager>();
        }

        for (int i = 0; i < marketDeck.Count; i++)
        {
            marketDeck[i].GetComponent<DragManager>().enabled = false;
            marketDeck[i].tag = "SelectableMarket";
            marketDeck[i].AddComponent<SelectManager>();
        }

        //sets up button for ending Phase 4, i.e. end current Round
        GameObject endRoundButton = (GameObject)Instantiate(addManager.GetComponent<AddressablesManager>().button);
        endRoundButton.name = "EndRoundButton";
        endRoundButton.transform.SetParent(GameObject.Find("GameGUI").transform);
        endRoundButton.transform.GetChild(0).GetComponent<Text>().text = "End Round";
        endRoundButton.GetComponent<RectTransform>().sizeDelta = new Vector2(95f, 25f);
        endRoundButton.transform.localPosition = new Vector2(101f, 118f);
        endRoundButton.GetComponent<Button>().onClick.AddListener(EndRound);

        foreach (GameObject factory in factoryDeck)
        {
            factory.AddComponent<SelectManager>();
            
            if (factory.GetComponent<Transform>().childCount > 1)
            {
                factory.tag = "Producible";
            }

            else
            {
                Debug.Log("Factory ID" + factory.GetComponent<FactoryCard>().id + " has no worker assigned. Cannot produce!");
                factory.tag = "Untagged";
            }
        }
    }

    /*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //ends current Round
    private void EndRound()
    {
        currentPhase = Phase.EndOfRound;
        PhaseTracker(currentPhase);
    }
}

/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/