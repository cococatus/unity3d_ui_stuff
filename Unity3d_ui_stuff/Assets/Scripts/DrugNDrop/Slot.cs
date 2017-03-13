using DrugNDrop.UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrugNDrop
{
    public class Slot : MonoBehaviour, IDropHandler {

        public GameObject CurrentItem
        {
            get
            {
                if (transform.childCount > 0)
                {
                    return transform.GetChild(0).gameObject;
                }
                return null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!CurrentItem && Item.DraggedItem)
            {
                Item.DraggedItem.transform.SetParent(transform, false);
                ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
            }
        }
    }
}
