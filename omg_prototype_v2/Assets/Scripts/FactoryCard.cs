/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * FactoryCard.cs
 * Factory/Resource card generated in-game.
 * Child of CardObject.cs.
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryCard : CardObject
{
    public string factory;      public string resource;     public string product;  public int productCount;
    public int productValue;    public int victoryPoints;   public int buildCost;   public int halfSun;
    public int reqStone;        public int reqClay;         public int reqWheat;    public int reqCotton;    public int reqWood;
    public int chainStone;      public int chainClay;       public int chainWheat;  public int chainCotton;  public int chainWood;
    public int chainCoal;       public int chainIron;       public int chainFlour;  public int chainCattle;  public int chainCloth;
    public int chainPlank;      public int chainGlass;      public int chainBread;  public int chainLeather;

    void Awake()
    {
        type = CardType.Factory;
        productCount = 0;
    }
}