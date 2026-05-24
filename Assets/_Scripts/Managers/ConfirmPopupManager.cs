using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.UI.Tools_Confirm;
using UnityEngine;
using Utils;

namespace _Scripts.Managers
{
    public class ConfirmPopupManager : MonoBehaviour
    {
        [SerializeField] private ConfirmPopup confirmPopup;
        [SerializeField] private List<ConfirmPopupDataMapping> confirmPopupDataMappings;

        private void Awake()
        {
            TileEventBus.OnToolOutOfUse += HandleToolOutOfUse;
        }

        private void OnDestroy()
        {
            TileEventBus.OnToolOutOfUse -= HandleToolOutOfUse;
        }

        private void HandleToolOutOfUse(ToolType toolType)
        {
            ConfirmPopupData data;
            switch (toolType)
            {
                case ToolType.Shuffle:
                    data = confirmPopupDataMappings[0].data;
                    confirmPopup.Setup(data.title, data.description, data.confirm, data.icon, BuyShuffle);
                    confirmPopup.Show();
                    break;
                case ToolType.AddSlot:
                    data = confirmPopupDataMappings[1].data;
                    confirmPopup.Setup(data.title, data.description, data.confirm, data.icon, BuyAddSlot);
                    confirmPopup.Show();
                    break;
                case ToolType.Return:
                    data = confirmPopupDataMappings[2].data;
                    confirmPopup.Setup(data.title, data.description, data.confirm, data.icon, BuyReturn);
                    confirmPopup.Show();
                    break;
                case ToolType.Quit:
                    data = confirmPopupDataMappings[2].data;
                    confirmPopup.Setup(data.title, data.description, data.confirm, data.icon, Quit);
                    confirmPopup.Show();
                    break;
                case ToolType.Heart:
                    data = confirmPopupDataMappings[2].data;
                    confirmPopup.Setup(data.title, data.description, data.confirm, data.icon, Quit);
                    confirmPopup.Show();
                    break;
            }
        }

        #region Confirm Popup

        private void BuyShuffle()
        {
            
        }

        private void BuyAddSlot()
        {
            
        }

        private void BuyReturn()
        {
            
        }

        private void Quit()
        {
            
        }

        #endregion
    }
}