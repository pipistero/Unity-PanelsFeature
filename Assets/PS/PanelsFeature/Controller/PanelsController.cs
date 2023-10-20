using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PS.PanelsFeature.Panels;

namespace PS.PanelsFeature.Controller
{
    public class PanelsController<T> where T : Enum
    {
        private readonly int PanelSortingOrderStep;

        private readonly Dictionary<T, AbstractPanel<T>> _panels;
        private readonly List<AbstractPanel<T>> _openedPanels;
        
        public PanelsController(IEnumerable<AbstractPanel<T>> panels, int panelSortingOrderStep)
        {
            PanelSortingOrderStep = panelSortingOrderStep;
            
            _panels = panels.ToDictionary(panel => panel.Type);
            _openedPanels = new List<AbstractPanel<T>>();
            
            InternalInit();
        }

        public Task OpenPanel(T panelType)
        {
            ValidatePanelType(panelType);

            int orderLayer = _openedPanels.Count == 0 ? 0 : _openedPanels.Last().GetOrderLayer();
            orderLayer += PanelSortingOrderStep;
            
            AbstractPanel<T> panelToOpen = _panels[panelType];

            _openedPanels.Add(panelToOpen);
            
            panelToOpen.OnOpen();
            panelToOpen.SetOrderLayer(orderLayer);

            return panelToOpen.Open();
        }

        public Task OpenPanelExtraordinary(T panelType, int orderLayer)
        {
            ValidatePanelType(panelType);
            
            AbstractPanel<T> panelToOpen = _panels[panelType];

            _openedPanels.Add(panelToOpen);

            panelToOpen.OnOpen();
            panelToOpen.SetOrderLayer(orderLayer);

            return panelToOpen.Open();
        }
        
        public Task ClosePanel(T panelType)
        {
            ValidatePanelType(panelType);
            
            AbstractPanel<T> panelToClose = _panels[panelType];

            _openedPanels.Remove(panelToClose);

            panelToClose.OnClose();
            panelToClose.SetOrderLayer(0);

            return panelToClose.Close();
        }

        #region Internal methods

        private void InternalInit()
        {
            foreach (var panel in _panels)
            {
                panel.Value.InternalInit();
            }
        }
        
        private void ValidatePanelType(T panelType)
        {
            if (_panels.ContainsKey(panelType) == false)
                throw new ArgumentException($"Panel ({panelType}) not found");
        }

        #endregion
        
    }
}