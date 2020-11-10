//old system for reference
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System.Linq;

//public class ProduceManager : MonoBehaviour
//{
//    //constant variables for tracking selectable resources and products (as coins), and producible factory GameObjects' tags
//    public const string SELECTABLE_TAG = "Selectable";
//    public const string SELECTABLE_REMOVE_TAG = "SelectableRemove";
//    public const string PRODUCIBLE_TAG = "Producible";
//    public const string PRODUCIBLE_CHAIN_TAG = "ProducibleChain";
//    public const string SELECTABLE_COIN_TAG = "SelectableCoin";
//    public const string SELECTABLE_COIN_USED_TAG = "SelectableCoinUsed";

//    //GameObject containers for accessing the GameManager and AddressablesManager scripts in them
//    private GameObject gameManager;
//    private GameObject addManager;

//    //Dictionary to store the following info (which are stored in SelectedCardInfo class):
//    //  - card ID
//    //  - card original position
//    //  - card original parent
//    //  - card resource (value defaults to 1; in future iterations, add another int value pair to Dictionary if more than 1)
//    private Dictionary<int, string> selectedCardInfo = new Dictionary<int, string>();

//    //bool to check whether the selectable GameObject is currently being selected
//    private bool selected = false;

//    //coordinates of the draggable GameObject's original position
//    private Vector2 originalPosition;

//    //names of the draggable GameObject's original and destination position's parent GameObject
//    private string originalParent;
//    private string destinationParent;

//    //Transforms and Images of the draggable GameObjects
//    private Transform objectToSelect;
//    private Image objectToSelectImage;

//    //GameObject for temporarily storing draggable GameObjects while they are being dragged around
//    private GameObject tempStorage;

//    //List for storing the results of the raycast that is cast whenever the left mouse button is clicked
//    List<RaycastResult> hitObjects = new List<RaycastResult>();

//    private void Awake()
//    {
//        //sets up the temporary storage for storing draggable GameObjects while they are being dragged around
//        tempStorage = GameObject.Find("TempStorage");
//        tempStorage.GetComponent<Transform>().SetParent(GameObject.Find("GameGUI").GetComponent<Transform>());
//        tempStorage.name = "TempStorage";
        
//        //assigns the GameManager and AddressablesManager GameObjects
//        gameManager = GameObject.Find("GameManager");
//        addManager = GameObject.Find("AddressablesManager");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //checks if left mouse button is pressed down
//        if (Input.GetMouseButtonUp(0))
//        {
//            if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseFour)
//            {
//                GameObject clickedObject = GetObjectUnderMouse();
//                objectToSelect = GetSelectableTransformUnderMouse();

//                //if the mouse is clicked on Resource card GameObject
//                if (objectToSelect != null)
//                {
//                    int selectedID = gameObject.GetComponent<FactoryCard>().id;
//                    string selectedResource = gameObject.GetComponent<FactoryCard>().resource;

//                    if (selected == false)
//                    {
//                        selected = true;
//                        selectedCardInfo.Add(selectedID, selectedResource);
//                        GameObject selectedCardOutline = new GameObject();
//                        selectedCardOutline.name = $"OutlineID{selectedID}";
//                        selectedCardOutline.AddComponent<Image>();
//                        //selectedCardOutline.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().cardOutline;
//                        selectedCardOutline.transform.SetParent(objectToSelect);
//                        selectedCardOutline.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 40f);
//                        selectedCardOutline.transform.localPosition = new Vector2(0f, 0f);
//                    }

//                    else if (selected)
//                    {
//                        selected = false;
//                        selectedCardInfo.Remove(selectedID);
//                        Destroy(gameObject.transform.GetChild(0));                 
//                    }
//                }
//            }
//        }

//        ////checks if left mouse button is lifted up
//        //if (Input.GetMouseButtonUp(0))
//        //{

//        //    //if the card dragged is a Resource card
//        //    if (objectToDrag != null)
//        //    {
//        //        Transform objectToReplace = GetDraggableTransformUnderMouse();
//        //        Transform objectToSlotIn = GetDroppableTransformUnderMouse();

//        //        //if there is already a card occupying the player deck's slot
//        //        if (objectToReplace != null && objectToReplace.CompareTag(DRAGGABLE_TAG))
//        //        {
//        //            destinationParent = objectToReplace.transform.parent.name;
//        //            objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //            objectToDrag.position = objectToReplace.position;
//        //            objectToReplace.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //            objectToReplace.position = originalPosition;
//        //        }

//        //        //if there are no cards occupying the destination slot
//        //        else if (objectToSlotIn != null)
//        //        {
//        //            //allow in Phase 2 and 4
//        //            //if card is a player deck card and the destination slot is the player deck's slot
//        //            if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_TAG))
//        //            {
//        //                destinationParent = objectToSlotIn.transform.name;
//        //                objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                objectToDrag.position = objectToSlotIn.position;
//        //            }

//        //            //allow in Phase 2
//        //            else if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseTwo)
//        //            {
//        //                //if card is a player deck card and the destination slot is the to-build slot
//        //                if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_BUILD_TAG))
//        //                {
//        //                    destinationParent = objectToSlotIn.transform.name;
//        //                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                    objectToDrag.position = objectToSlotIn.position;
//        //                }

//        //                else
//        //                {
//        //                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                    objectToDrag.position = originalPosition;
//        //                }
//        //            }

//        //            //allow in Phase 4
//        //            else if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseFour)
//        //            {
//        //                //if card is a player deck card and the destination slot is the factory deck's slot
//        //                if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_FACTORY_TAG))
//        //                {
//        //                    destinationParent = objectToSlotIn.transform.name;
//        //                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                    objectToDrag.position = objectToSlotIn.position;
//        //                }

//        //                //if card is a player deck card and the destination slot is the to-build slot
//        //                if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_BUILD_TAG))
//        //                {
//        //                    destinationParent = objectToSlotIn.transform.name;
//        //                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                    objectToDrag.position = objectToSlotIn.position;
//        //                }

//        //                else
//        //                {
//        //                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                    objectToDrag.position = originalPosition;
//        //                }
//        //            }

//        //            else
//        //            {
//        //                objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                objectToDrag.position = originalPosition;
//        //            }
//        //        }

//        //        else
//        //        {
//        //            objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //            objectToDrag.position = originalPosition;
//        //        }

//        //        objectToDragImage.raycastTarget = true;
//        //        objectToDrag = null;
//        //        originalParent = null;
//        //        destinationParent = null;
//        //    }

//        //    //if the card dragged is a worker card
//        //    else if (workerToDrag != null)
//        //    {
//        //        //allow in Phase 2
//        //        if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseTwo)
//        //        {
//        //            Transform workerToReplace = GetDraggableWorkerTransformUnderMouse();
//        //            Transform objectToSlotIn = GetDroppableTransformUnderMouse();

//        //            //if there is already a card occupying the worker deck's slot
//        //            if (workerToReplace != null && workerToReplace.CompareTag(DRAGGABLE_WORKER_TAG))
//        //            {
//        //                destinationParent = workerToReplace.transform.parent.name;
//        //                workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                workerToDrag.position = workerToReplace.position;
//        //                workerToReplace.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                workerToReplace.position = originalPosition;
//        //            }

//        //            //if there are no cards occupying the destination slot
//        //            else if (objectToSlotIn != null)
//        //            {
//        //                //if card is a worker deck card and the destination slot is the worker deck's slot
//        //                if (workerToDrag.CompareTag(DRAGGABLE_WORKER_TAG) && objectToSlotIn.CompareTag(DROPPABLE_WORKER_TAG))
//        //                {
//        //                    destinationParent = objectToSlotIn.transform.name;
//        //                    workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                    workerToDrag.position = objectToSlotIn.position;
//        //                    workerToDrag.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().spriteWorker;
//        //                    workerToDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(35f, 20f);
//        //                    workerToDrag.transform.localPosition = new Vector2(0f, 0f);
//        //                }

//        //                //if card is a worker deck card and the destination slot is the factory deck's slot
//        //                else if (workerToDrag.CompareTag(DRAGGABLE_WORKER_TAG) && objectToSlotIn.CompareTag(DROPPABLE_FACTORY_TAG))
//        //                {
//        //                    destinationParent = objectToSlotIn.transform.name;
//        //                    workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
//        //                    workerToDrag.position = objectToSlotIn.position;
//        //                    workerToDrag.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().factoryWorker;
//        //                    workerToDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(11f, 14f);
//        //                    workerToDrag.transform.localPosition = new Vector2(-12f, 17f);
//        //                }

//        //                else
//        //                {
//        //                    workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                    workerToDrag.position = originalPosition;
//        //                }
//        //            }

//        //            else
//        //            {
//        //                workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
//        //                workerToDrag.position = originalPosition;
//        //            }

//        //            workerToDragImage.raycastTarget = true;
//        //            workerToDrag = null;
//        //            originalParent = null;
//        //            destinationParent = null;
//        //        }
//        //    }

//        //    dragging = false;
//        //}
//    }

//    //returns the topmost GameObject detected by the raycast, i.e. the first GameObject in the raycast result's List
//    private GameObject GetObjectUnderMouse()
//    {
//        var pointer = new PointerEventData(EventSystem.current);
//        pointer.position = Input.mousePosition;
//        EventSystem.current.RaycastAll(pointer, hitObjects);

//        if (hitObjects.Count <= 0)
//        {
//            return null;
//        }

//        return hitObjects.First().gameObject;
//    }

//    //gets the Transform of the selectable Resource card GameObject under the mouse
//    private Transform GetSelectableTransformUnderMouse()
//    {
//        GameObject clickedObject = GetObjectUnderMouse();

//        if (clickedObject != null && (clickedObject.CompareTag(SELECTABLE_TAG)))
//        {
//            return clickedObject.transform;
//        }

//        return null;
//    }

//    //gets the Transform of the selectable coin card GameObject under the mouse
//    private Transform GetSelectedCoinTransformUnderMouse()
//    {
//        GameObject clickedObject = GetObjectUnderMouse();

//        if (clickedObject != null && (clickedObject.CompareTag(SELECTABLE_COIN_TAG)))
//        {
//            return clickedObject.transform;
//        }

//        return null;
//    }

//    //gets the Transform of the producible factory GameObject under the mouse
//    private Transform GetProducibleTransformUnderMouse()
//    {
//        GameObject clickedObject = GetObjectUnderMouse();

//        if (clickedObject != null && (clickedObject.CompareTag(PRODUCIBLE_TAG)))
//        {
//            return clickedObject.transform;
//        }

//        return null;
//    }

//    //gets the Transform of the producible factory (for chain production) GameObject under the mouse
//    private Transform GetProducibleChainTransformUnderMouse()
//    {
//        GameObject clickedObject = GetObjectUnderMouse();

//        if (clickedObject != null && (clickedObject.CompareTag(PRODUCIBLE_CHAIN_TAG)))
//        {
//            return clickedObject.transform;
//        }

//        return null;
//    }
//}



///* FACTORY PRODUCTION ACTIVATION
// * 1. click on card
// * 2. register in Dictionary:
// *      - card ID (grab from script)
// *      - req. values (grab from script)
// *      - card original position
// *      - card original parent
// *      
// * 3. when mouse up, keep position on mouse
// * 
// * 4. when mouse down:
// *      - if tag is DroppableFactory && meets build requirements (i.e. has a worker child && covers production reqs):
// *          // +1 to product storage
// *          // put used cards in discard pile
// *          // remove used cards from Dictionary
// *      - if tag is Draggable:
// *          // add it to Dictionary
// *      - if tag is DroppableReturn:
// *          // return all cards to their original position
// *          // remove all cards from Dictionary
// *      - else keep position on mouse
// *      
// * CHAIN PRODUCTION
// * 5. when building is activated, chain mode = true
// *      - ...and allow consecutive productions as long as player supplies the factory with the right resources
// */

///* TO-BUILD BUILDING
// * 1. click on to-build card to bring up building cost
// * 2. if player has sufficient funds (i.e. enough in the product storages), show "Build with x coins?" prompt
// *      - else tell them that funds are insufficient
// * 3. if player decides to build, prompt to remove right amount of coins from product storage:
// *      - generate Build button (disabled until x coins have been reached)
// *      - generate Cancel Transaction button
// *      - when mouse down:
// *          - if tag is DraggableProduct:
// *              // +1 to coin use counter
// *              // change tag to UsedProduct (so they can be removed from gameplay later)
// *          - if tag is DroppableReturn:
// *              // reset coin use counter
// *              // re-enable tag in all products
// * 4. click on "Cancel Transaction" to cancel the build decision and return to normal gameplay
// * 5. otherwise, once x coins has been reached:
// *      - remove products tagged UsedProduct
// *      - add building to factory deck
// */