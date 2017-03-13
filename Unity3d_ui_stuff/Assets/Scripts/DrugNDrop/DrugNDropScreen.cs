using System;
using System.Text;
using DrugNDrop.UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrugNDrop
{
    public class DrugNDropScreen : MonoBehaviour, IHasChanged {

        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private Transform _activeSlotsContainer;
        [SerializeField] private Transform[] _slots;
        [SerializeField] private Transform[] _activeSlots;
        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private Button _actriveSlotsButton;
        [SerializeField] private Button _inventoryButton;

        public Transform[] Slots
        {
            get { return _slots; }
        }

        public Transform[] ActiveSlots
        {
            get { return _activeSlots; }
        }

        private enum State { InActiveSlots, InInventory }

        private State _currentState = State.InActiveSlots;
        
        private void OnEnable()
        {
            ChangeState(State.InActiveSlots);

            _actriveSlotsButton.onClick.AddListener(() => ChangeState(State.InActiveSlots));
            _inventoryButton.onClick.AddListener(() => ChangeState(State.InInventory));
        }

        private void ChangeState(State state)
        {
            _currentState = state;

            switch (state)
            {
                case State.InActiveSlots:
                    _activeSlotsContainer.gameObject.SetActive(true);
                    _slotsContainer.gameObject.SetActive(false);
                    break;
                case State.InInventory:
                    _activeSlotsContainer.gameObject.SetActive(false);
                    _slotsContainer.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        private void Start()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                var item = Instantiate(_itemPrefab);
                item.GetComponent<Item>().Init(this);
                item.transform.SetParent(_slots[i], false);
            }

            HasChanged();
        }

        private void OnDisable()
        {
            _actriveSlotsButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
        }

        public void HasChanged()
        {
            var builder = new StringBuilder();
            builder.Append(" * ");
            for (int i = 0; i < _activeSlots.Length; i++)
            {
                var item = _activeSlots[i].GetComponent<Slot>().CurrentItem;
                if (item)
                {
                    builder.Append(item.GetComponent<Item>().Name);
                    builder.Append(" * ");
                }
            }
        }
    }

    namespace UnityEngine.EventSystems
    {
        public interface IHasChanged : IEventSystemHandler
        {
            void HasChanged();
        }
    }
}
