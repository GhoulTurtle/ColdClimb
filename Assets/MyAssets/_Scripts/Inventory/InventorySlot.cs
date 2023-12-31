using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using ColdClimb.Item.Equipped;
using ColdClimb.Item;
using ColdClimb.Global;

namespace ColdClimb.Inventory{
    // UI class that detects when the cursor is over its gameobject and calls OnSlotSelected from the InventoryUI 
    public class InventorySlot : MonoBehaviour, ISelectHandler{   
        public InventoryItem ItemInSlot {get; set;}
        public Image ItemImage {get; private set;}
        public bool IsEquippmentSlot => isEquipableSlot;

        [Header("Stack Text Color Options")]
        [SerializeField] private Color stackTextDefaultColor = Color.white;
        [SerializeField] private Color stackTextFullColor = Color.green;

        [Header("Slot Type")]
        [SerializeField] private bool isEquipableSlot = false;

        private InventoryUIController inventoryUIController;
        private TMP_Text stackText;
        private Image InstancedImage;

        public void OnSelect(BaseEventData eventData){
            inventoryUIController.OnSlotSelectedAction(this);
        }

        public void OnSlotClicked(){
            if(ItemInSlot.ItemData == null && GameManager.CurrentState != GameState.CombineItemScreen) return;
            inventoryUIController.OnViableSlotClickedAction(this);
        }

        public void SetupSlot(InventoryUIController controller, InventoryItem item){
            inventoryUIController = controller;
            ItemInSlot = item;
            if(stackText == null) stackText = GetComponentInChildren<TMP_Text>();
            DrawSlotVisual();
        }

        public void DrawSlotVisual(){
            if(InstancedImage != null) Destroy(InstancedImage.gameObject);

            if (ItemInSlot.ItemData == null){
                stackText.enabled = false;
                return;
            }

            if(!ItemInSlot.ItemData.IsItemStackable){
                if(ItemInSlot.ItemData.ItemType == ItemType.Gun){
                    stackText.enabled = true;
                    GunEquipableItem gunEquipable = (GunEquipableItem)ItemInSlot.ItemData;
                    stackText.text = gunEquipable.GunStats.currentAmmo.ToString();
                    stackText.color = gunEquipable.GunStats.currentAmmo == gunEquipable.GunStats.maxAmmo ? stackTextFullColor : stackTextDefaultColor;
                }
                else{
                    stackText.enabled = false;
                }
            } 
            else{
            stackText.enabled = true;
            stackText.text = "X" + ItemInSlot.CurrentStackSize;
            stackText.color = ItemInSlot.IsFull() ? stackTextFullColor : stackTextDefaultColor;
            } 

            ItemImage = ItemInSlot.ItemData.Image;
            InstancedImage = Instantiate(ItemImage, transform.position, Quaternion.identity, transform);
        }
    }
}
