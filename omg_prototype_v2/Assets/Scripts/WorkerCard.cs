/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * WorkerCard.cs
 * Worker card generated in-game.
 * Child of CardObject.cs.
///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCard : CardObject
{
    public bool isEfficient;
    
    void Awake()
    {
        type = CardType.Worker;
        isEfficient = true;
    }
}
