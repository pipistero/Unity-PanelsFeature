using System;
using System.Threading.Tasks;
using PS.PanelsFeature.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace PS.PanelsFeature.Panels
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class AbstractPanel<T> : MonoBehaviour, IPanel
        where T : Enum
    {
        [Header("Type")] 
        [SerializeField] private T _panelType;

        public T Type => _panelType;

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        #region Internal methods

        public void InternalInit()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _canvas.overrideSorting = true;
            _canvasGroup.interactable = false;
            
            SetOrderLayer(0);
            
            gameObject.SetActive(false);
        }

        public void SetOrderLayer(int orderLayer)
        {
            _canvas.sortingOrder = orderLayer;
        }

        public int GetOrderLayer()
        {
            return _canvas.sortingOrder;
        }
        
        #endregion

        #region Abstract methods

        public abstract void OnOpen();
        public abstract void OnClose();

        #endregion

        #region Virtual methods

        public virtual Task Open()
        {
            gameObject.SetActive(true);

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;

            return Task.CompletedTask;
        }

        public virtual Task Close()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;

            gameObject.SetActive(false);

            return Task.CompletedTask;
        }

        #endregion
    }
}