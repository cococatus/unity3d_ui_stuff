using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DrugNDrop
{
    public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Text _name;
        [SerializeField] private Image _icon;
        [SerializeField] private Slider _nitroSlider;
        [SerializeField] private GameObject _locker;
        [SerializeField] private CanvasGroup _canvasGroup;

        private static GameObject _draggedItem;

        public static GameObject DraggedItem { get { return _draggedItem; } }

        public string Name { get { return _name.text; } }

        private DrugNDropScreen _screen;
        private Canvas _canvas;
        private Vector3 _startPosition;
        private Transform _startParent;

        public void Init(DrugNDropScreen screen)
        {
            _name.text = String.Format("{0}", "Name");
            _nitroSlider.value = Mathf.Clamp(Random.Range(50,445), 0, 445);
            _canvas = screen.GetComponentInParent<Canvas>();
            _screen = screen;

            if (_canvas == null)
                Debug.LogError("canvas is null");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;

            _startPosition = transform.position;
            _startParent = transform.parent;
            _draggedItem = gameObject;
            transform.parent = _canvas.transform;

        }

        private Vector3 newPos;
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(this.GetComponent<RectTransform>(), eventData.position, eventData.enterEventCamera, out newPos);
            transform.position = newPos;

            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var noActiveSlots = _screen.ActiveSlots.Any(x => x.gameObject == eventData.pointerCurrentRaycast.gameObject);
            var noSlots = _screen.Slots.Any(x => x.gameObject == eventData.pointerCurrentRaycast.gameObject);

            if (!noActiveSlots && !noSlots)
            {
                transform.SetParent(_startParent);
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _draggedItem = null;
                _canvasGroup.blocksRaycasts = true;

                if (transform.parent == _canvas)
                {
                    transform.position = _startPosition;
                }
            }
        }
    }
}
