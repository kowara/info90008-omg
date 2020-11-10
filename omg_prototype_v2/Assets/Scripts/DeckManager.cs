/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * DeckManager.cs
 * Manages all card decks and storages in-game. 
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> drawPile = new List<GameObject>();
    public List<GameObject> discardPile = new List<GameObject>();
    public List<GameObject> playerDeck = new List<GameObject>();
    public List<GameObject> factoryDeck = new List<GameObject>();
    public List<GameObject> marketDeck = new List<GameObject>();
    public List<GameObject> workerDeck = new List<GameObject>();
    public List<GameObject> charburnerDeck = new List<GameObject>();
    public List<GameObject> toBuild = new List<GameObject>();
    public List<GameObject> selectedDeck = new List<GameObject>();
}
