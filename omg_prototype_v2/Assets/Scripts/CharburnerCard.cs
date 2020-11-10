/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * CharburnerCard.cs
 * Charburner card generated in-game.
 * Child of CardObject.cs.
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharburnerCard : CardObject
{
    public string factory;
    public string product;
    public int productCount;
    public int productValue;
    public int reqStone;
    public int reqClay;
    public int reqWheat;
    public int reqCotton;
    public int reqWood;

    void Awake()
    {
        type = CardType.Charburner;
        productCount = 0;
    }
}