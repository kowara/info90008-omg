using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DragManager : MonoBehaviour
{
    //constant variables for tracking draggable and card slot GameObjects' tags
    public const string DRAGGABLE_TAG = "Draggable";
    public const string DRAGGABLE_WORKER_TAG = "DraggableWorker";
    public const string DROPPABLE_TAG = "Droppable";
    public const string DROPPABLE_WORKER_TAG = "DroppableWorker";
    public const string DROPPABLE_BUILD_TAG = "DroppableBuild";
    public const string DROPPABLE_FACTORY_TAG = "DroppableFactory";

    //GameObject containers for accessing the GameManager and AddressablesManager scripts in them
    private GameObject gameManager;
    private GameObject addManager;

    //bool to check whether the draggable GameObject is currently being dragged
    private bool dragging = false;

    //coordinates of the draggable GameObject's original position
    private Vector2 originalPosition;

    //names of the draggable GameObject's original and destination position's parent GameObject
    private string originalParent;
    private string destinationParent;

    //Transforms and Images of the draggable GameObjects
    private Transform objectToDrag;
    private Transform workerToDrag;
    private Image objectToDragImage;
    private Image workerToDragImage;

    //GameObject for temporarily storing draggable GameObjects while they are being dragged around
    private GameObject tempStorage;

    //List for storing the results of the raycast that is cast whenever the left mouse button is clicked
    List<RaycastResult> hitObjects = new List<RaycastResult>(); 

    private void Awake()
    {
        //sets up the temporary storage for storing draggable GameObjects while they are being dragged around
        tempStorage = GameObject.Find("TempStorage");
        tempStorage.GetComponent<Transform>().SetParent(GameObject.Find("GameGUI").GetComponent<Transform>());
        tempStorage.name = "TempStorage";

        //assigns the GameManager and AddressablesManager GameObjects
        gameManager = GameObject.Find("GameManager");
        addManager = GameObject.Find("AddressablesManager");
    }

    private void Update()
    {
        //checks if left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            objectToDrag = GetDraggableTransformUnderMouse();
            workerToDrag = GetDraggableWorkerTransformUnderMouse();

            //if the mouse is clicked on Resource card GameObject
            if (objectToDrag != null)
            {
                dragging = true;
                originalParent = objectToDrag.transform.parent.name;
                objectToDrag.SetAsLastSibling();
                originalPosition = objectToDrag.position;
                objectToDragImage = objectToDrag.GetComponent<Image>();
                objectToDragImage.raycastTarget = false;
                objectToDrag.GetComponent<Transform>().SetParent(tempStorage.GetComponent<Transform>());
            }

            //if the mouse is clicked on worker card GameObject
            else if (workerToDrag != null)
            {
                if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseTwo)
                {
                    dragging = true;
                    originalParent = workerToDrag.transform.parent.name;
                    workerToDrag.SetAsLastSibling();
                    originalPosition = workerToDrag.position;
                    workerToDragImage = workerToDrag.GetComponent<Image>();
                    workerToDragImage.raycastTarget = false;
                    workerToDrag.GetComponent<Transform>().SetParent(tempStorage.GetComponent<Transform>());
                }
            }
        }

        //if the mouse is being dragged after clicking either a Resource or worker card GameObject
        if (dragging)
        {
            if (objectToDrag != null)
            {
                objectToDrag.position = Input.mousePosition;
            }

            else if (workerToDrag != null)
            {
                workerToDrag.position = Input.mousePosition;
            }
        }

        //checks if left mouse button is lifted up
        if (Input.GetMouseButtonUp(0))
        {

            //if the card dragged is a Resource card
            if (objectToDrag != null)
            {
                Transform objectToReplace = GetDraggableTransformUnderMouse();
                Transform objectToSlotIn = GetDroppableTransformUnderMouse();

                //if there is already a card occupying the player deck's slot
                if (objectToReplace != null && objectToReplace.CompareTag(DRAGGABLE_TAG))
                {
                    destinationParent = objectToReplace.transform.parent.name;
                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                    objectToReplace.position = originalPosition;
                }

                //if there are no cards occupying the destination slot
                else if (objectToSlotIn != null)
                {
                    //allow in Phase 2 and 4
                    //if card is a player deck card and the destination slot is the player deck's slot
                    if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_TAG))
                    {
                        destinationParent = objectToSlotIn.transform.name;
                        objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                        objectToDrag.position = objectToSlotIn.position;
                    }

                    //allow in Phase 2
                    else if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseTwo)
                    {
                        //if card is a player deck card and the destination slot is the to-build slot
                        if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_BUILD_TAG))
                        {
                            destinationParent = objectToSlotIn.transform.name;
                            objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                            objectToDrag.position = objectToSlotIn.position;
                        }

                        else
                        {
                            objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                            objectToDrag.position = originalPosition;
                        }
                    }

                    ////allow in Phase 4
                    //else if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseFour)
                    //{
                    //    //if card is a player deck card and the destination slot is the factory deck's slot
                    //    if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_FACTORY_TAG))
                    //    {
                    //        destinationParent = objectToSlotIn.transform.name;
                    //        objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                    //        objectToDrag.position = objectToSlotIn.position;
                    //    }

                    //    //if card is a player deck card and the destination slot is the to-build slot
                    //    if (objectToDrag.CompareTag(DRAGGABLE_TAG) && objectToSlotIn.CompareTag(DROPPABLE_BUILD_TAG))
                    //    {
                    //        destinationParent = objectToSlotIn.transform.name;
                    //        objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                    //        objectToDrag.position = objectToSlotIn.position;
                    //    }

                    //    else
                    //    {
                    //        objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                    //        objectToDrag.position = originalPosition;
                    //    }
                    //}
                
                    else
                    {
                        objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                        objectToDrag.position = originalPosition;
                    }
                }

                else
                {
                    objectToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                    objectToDrag.position = originalPosition;
                }

                objectToDragImage.raycastTarget = true;
                objectToDrag = null;
                originalParent = null;
                destinationParent = null;
            }

            //if the card dragged is a worker card
            else if (workerToDrag != null)
            {
                //allow in Phase 2
                if (gameManager.GetComponent<GameManager>().currentPhase == Phase.PhaseTwo)
                {
                    Transform workerToReplace = GetDraggableWorkerTransformUnderMouse();
                    Transform objectToSlotIn = GetDroppableTransformUnderMouse();

                    //if there is already a card occupying the worker deck's slot
                    if (workerToReplace != null && workerToReplace.CompareTag(DRAGGABLE_WORKER_TAG))
                    {
                        destinationParent = workerToReplace.transform.parent.name;
                        workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                        workerToDrag.position = workerToReplace.position;
                        workerToReplace.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                        workerToReplace.position = originalPosition;
                    }

                    //if there are no cards occupying the destination slot
                    else if (objectToSlotIn != null)
                    {
                        //if card is a worker deck card and the destination slot is the worker deck's slot
                        if (workerToDrag.CompareTag(DRAGGABLE_WORKER_TAG) && objectToSlotIn.CompareTag(DROPPABLE_WORKER_TAG))
                        {
                            destinationParent = objectToSlotIn.transform.name;
                            workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                            workerToDrag.position = objectToSlotIn.position;
                            workerToDrag.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().spriteWorker;
                            workerToDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(35f, 20f);
                            workerToDrag.transform.localPosition = new Vector2(0f, 0f);
                        }

                        //if card is a worker deck card and the destination slot is the factory deck's slot
                        else if (workerToDrag.CompareTag(DRAGGABLE_WORKER_TAG) && objectToSlotIn.CompareTag(DROPPABLE_FACTORY_TAG))
                        {
                            destinationParent = objectToSlotIn.transform.name;
                            workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(destinationParent).GetComponent<Transform>());
                            workerToDrag.position = objectToSlotIn.position;
                            workerToDrag.GetComponent<Image>().sprite = addManager.GetComponent<AddressablesManager>().factoryWorker;
                            workerToDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(11f, 14f);
                            workerToDrag.transform.localPosition = new Vector2(-12f, 17f);
                        }

                        else
                        {
                            workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                            workerToDrag.position = originalPosition;
                        }
                    }

                    else
                    {
                        workerToDrag.GetComponent<Transform>().SetParent(GameObject.Find(originalParent).GetComponent<Transform>());
                        workerToDrag.position = originalPosition;
                    }

                    workerToDragImage.raycastTarget = true;
                    workerToDrag = null;
                    originalParent = null;
                    destinationParent = null;
                }
            }

            dragging = false;
        }
    }

    //returns the topmost GameObject detected by the raycast, i.e. the first GameObject in the raycast result's List
    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0)
        {
            return null;
        }

        return hitObjects.First().gameObject;
    }

    //gets the Transform of the draggable Resource card GameObject under the mouse
    private Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && (clickedObject.CompareTag(DRAGGABLE_TAG)))
        {
            return clickedObject.transform;
        }

        return null;
    }

    //gets the Transform of the draggable worker card GameObject under the mouse
    private Transform GetDraggableWorkerTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && (clickedObject.CompareTag(DRAGGABLE_WORKER_TAG)))
        {
            return clickedObject.transform;
        }

        return null;
    }

    //gets the Transform of the card slot GameObject under the mouse
    private Transform GetDroppableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && (clickedObject.CompareTag(DROPPABLE_TAG) 
            || clickedObject.CompareTag(DROPPABLE_WORKER_TAG)
            || clickedObject.CompareTag(DROPPABLE_FACTORY_TAG) 
            || clickedObject.CompareTag(DROPPABLE_BUILD_TAG)))
        {
            return clickedObject.transform;
        }

        return null;
    }
}

//old code for reference
    //public Transform parentToReturnTo = null;
    //public Transform placeholderParent = null;

    //GameObject placeholder = null;

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnBeginDrag");

    //    placeholder = new GameObject();
    //    placeholder.transform.SetParent(this.transform.parent);
    //    LayoutElement le = placeholder.AddComponent<LayoutElement>();
    //    le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
    //    le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
    //    le.flexibleWidth = 0;
    //    le.flexibleHeight = 0;

    //    placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

    //    parentToReturnTo = this.transform.parent;
    //    placeholderParent = parentToReturnTo;
    //    this.transform.SetParent(this.transform.parent.parent);

    //    GetComponent<CanvasGroup>().blocksRaycasts = false;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    this.transform.position = eventData.position;

    //    if (placeholder.transform.parent != placeholderParent)
    //        placeholder.transform.SetParent(placeholderParent);

    //    int newSiblingIndex = placeholderParent.childCount;

    //    for (int i = 0; i < placeholderParent.childCount; i++)
    //    {
    //        if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
    //        {

    //            newSiblingIndex = i;

    //            if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
    //                newSiblingIndex--;

    //            break;
    //        }
    //    }

    //    placeholder.transform.SetSiblingIndex(newSiblingIndex);

    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnEndDrag");
    //    this.transform.SetParent(parentToReturnTo);
    //    this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
    //    GetComponent<CanvasGroup>().blocksRaycasts = true;

    //    Destroy(placeholder);

    //    //EventSystem.current.RaycastAll(eventData)
    //    //events that should happen when dropping card onto place:
    //    //1. assign Worker card to Production Building (Worker on PB) --> flip to 'Efficient' or 'Sloppy'
    //    //2. place Production Building to the 'To Build' field
    //    //3. place Resource cards from hand onto Production Buildings --> each PB has an 'production activation condition'
    //    //4. same with 4 --> each PB has a 'chain activation condition'
    //    //5. place Goods cards onto 'To Build' field to build new PB --> otherwise, return PB card to hand
    //}
//}