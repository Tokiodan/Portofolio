using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; 
        startPosition = transform.position;

        transform.SetParent(transform.root); 
        transform.SetAsLastSibling(); 

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target != null && target.CompareTag("Slot"))
        {
           
            transform.SetParent(target.transform, false); 
            transform.position = target.transform.position;
        }
        else
        {
            
            transform.SetParent(originalParent, false);
            transform.position = startPosition;
        }

        canvasGroup.blocksRaycasts = true; 
        transform.localScale = Vector3.one; 
    }
}
