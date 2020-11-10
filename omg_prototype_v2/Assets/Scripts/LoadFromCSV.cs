/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////
 * LoadFromCSV.cs
 * Loads data from CSV file to game cards.
 ///////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//creates Card objects according to number of entries in deckdata.csv
//assigns object parameters to each Card object
public class LoadFromCSV : MonoBehaviour
{
    [SerializeField] GameObject addressablesManager;
    [SerializeField] TextAsset cardCSV;
    [SerializeField] TextAsset cardCSVCharburner;

    void Awake()
    {
        AddressablesManager addManager = addressablesManager.GetComponent<AddressablesManager>();
        
        string[] data = cardCSV.text.Split(new char[] { '\n' });

        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[0] != "")
            {
                GameObject cardBody = new GameObject();
                cardBody.AddComponent<Image>();
                cardBody.AddComponent<CanvasGroup>();
                cardBody.AddComponent<DragManager>();
                cardBody.name = "ID" + i;
                cardBody.tag = "Draggable";

                //adding Card script component to card
                FactoryCard card = cardBody.AddComponent<FactoryCard>();

                int.TryParse(row[0], out card.id);
                card.factory = row[1];
                card.resource = row[2];
                card.product = row[3];
                int.TryParse(row[4], out card.productValue);
                int.TryParse(row[5], out card.victoryPoints);
                int.TryParse(row[6], out card.buildCost);
                int.TryParse(row[7], out card.halfSun);
                int.TryParse(row[8], out card.reqStone);
                int.TryParse(row[9], out card.reqClay);
                int.TryParse(row[10], out card.reqWheat);
                int.TryParse(row[11], out card.reqCotton);
                int.TryParse(row[12], out card.reqWood);
                int.TryParse(row[13], out card.chainStone);
                int.TryParse(row[14], out card.chainClay);
                int.TryParse(row[15], out card.chainWheat);
                int.TryParse(row[16], out card.chainCotton);
                int.TryParse(row[17], out card.chainWood);
                int.TryParse(row[18], out card.chainCoal);
                int.TryParse(row[19], out card.chainIron);
                int.TryParse(row[20], out card.chainFlour);
                int.TryParse(row[21], out card.chainCattle);
                int.TryParse(row[22], out card.chainCloth);
                int.TryParse(row[23], out card.chainPlank);
                int.TryParse(row[24], out card.chainGlass);
                int.TryParse(row[25], out card.chainBread);
                int.TryParse(row[26], out card.chainLeather);

                if (card.resource.Contains("Stone"))
                {
                    //addManager.AddressablesSpriteStone();
                    cardBody.GetComponent<Image>().sprite = addManager.spriteStone;
                    Debug.Log("Stone here!");
                }

                else if (card.resource.Contains("Clay"))
                {
                    //addManager.AddressablesSpriteClay();
                    cardBody.GetComponent<Image>().sprite = addManager.spriteClay;
                    Debug.Log("Clay here!");
                }

                else if (card.resource.Contains("Wheat"))
                {
                    //addManager.AddressablesSpriteWheat();
                    cardBody.GetComponent<Image>().sprite = addManager.spriteWheat;
                    Debug.Log("Wheat here!");
                }

                else if (card.resource.Contains("Cotton"))
                {
                    //addManager.AddressablesSpriteCotton();
                    cardBody.GetComponent<Image>().sprite = addManager.spriteCotton;
                    Debug.Log("Cotton here!");
                }

                else if (card.resource.Contains("Wood"))
                {
                    //addManager.AddressablesSpriteWood();
                    cardBody.GetComponent<Image>().sprite = addManager.spriteWood;
                    Debug.Log("Wood here!");
                }

                else
                {
                    Debug.Log("Not a resource.");
                }

                cardBody.transform.SetParent(GameObject.FindWithTag("DrawPile").transform);
                cardBody.SetActive(false);
                GameObject.FindWithTag("DeckManager").GetComponent<DeckManager>().drawPile.Add(cardBody);
            }
        }

        string[] dataCharburner = cardCSVCharburner.text.Split(new char[] { '\n' });

        for (int i = 0; i < dataCharburner.Length; i++)
        {
            string[] row = dataCharburner[i].Split(new char[] { ',' });

            if (row[0] != "")
            {
                GameObject cardBody = new GameObject();
                cardBody.AddComponent<Image>();
                cardBody.AddComponent<CanvasGroup>();
                cardBody.name = "IDC" + i;

                //adding Card script component to card
                FactoryCard card = cardBody.AddComponent<FactoryCard>();

                int.TryParse(row[0], out card.id);
                card.factory = row[1];
                card.product = row[2];
                int.TryParse(row[3], out card.productValue);
                int.TryParse(row[4], out card.reqStone);
                int.TryParse(row[5], out card.reqClay);
                int.TryParse(row[6], out card.reqWheat);
                int.TryParse(row[7], out card.reqCotton);
                int.TryParse(row[8], out card.reqWood);

                cardBody.GetComponent<Image>().sprite = addManager.spriteCharburner;
                Debug.Log("Charburner set up!");

                cardBody.SetActive(false);
                GameObject.FindWithTag("DeckManager").GetComponent<DeckManager>().charburnerDeck.Add(cardBody);
            }
        }
    }
}
