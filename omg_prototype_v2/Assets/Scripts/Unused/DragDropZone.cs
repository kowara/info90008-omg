//old system for reference
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class DragDropZone : MonoBehaviour, IDropHandler
//{

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag != null)
//        {
//            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
//        }
//    }








































//    //    public void OnPointerEnter(PointerEventData eventData)
//    //    {
//    //        //Debug.Log("OnPointerEnter");
//    //        if (eventData.pointerDrag == null)
//    //            return;

//    //        DragManager d = eventData.pointerDrag.GetComponent<DragManager>();
//    //        if (d != null)
//    //        {
//    //            d.placeholderParent = this.transform;
//    //        }
//    //    }

//    //    public void OnPointerExit(PointerEventData eventData)
//    //    {
//    //        //Debug.Log("OnPointerExit");
//    //        if (eventData.pointerDrag == null)
//    //            return;

//    //        DragManager d = eventData.pointerDrag.GetComponent<DragManager>();
//    //        if (d != null && d.placeholderParent == this.transform)
//    //        {
//    //            d.placeholderParent = d.parentToReturnTo;
//    //        }
//    //    }

//    //    public void OnDrop(PointerEventData eventData)
//    //    {
//    //        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

//    //        DragManager d = eventData.pointerDrag.GetComponent<DragManager>();
//    //        if (d != null)
//    //        {
//    //            d.parentToReturnTo = this.transform;
//    //        }

//    //    }
//}
