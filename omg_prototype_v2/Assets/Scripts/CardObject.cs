/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * CardObject.cs
 * Base script for all cards generated in-game.
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Worker,
    Factory,
    Charburner
}
public abstract class CardObject : MonoBehaviour
{
    public int id;
    public GameObject prefab;
    public CardType type;
    public string description;
}

