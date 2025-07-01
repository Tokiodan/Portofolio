using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Vector2 battlefieldCardSize = new Vector2(150, 225);

    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard draggedCard = eventData.pointerDrag?.GetComponent<DraggableCard>();

        if (draggedCard != null)
        {
           
            draggedCard.transform.SetParent(transform, false);
            draggedCard.transform.position = transform.position; 
            draggedCard.transform.localScale = Vector3.one; 

          
            RectTransform rt = draggedCard.GetComponent<RectTransform>();
            rt.sizeDelta = battlefieldCardSize; 

            Debug.Log("Card placed in slot: " + gameObject.name); 
        }
    }
}
