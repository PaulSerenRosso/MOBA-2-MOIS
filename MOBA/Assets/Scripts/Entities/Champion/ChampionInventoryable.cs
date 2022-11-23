using Entities.Inventory;
using Photon.Pun;

namespace Entities.Champion
{
    public partial class Champion : IInventoryable
    {
        public Item[] items = new Item[3];

        public Item[] GetItems()
        {
            return items;
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= items.Length) return null;
            return items[index];
        }

        public void RequestAddItem(byte index) { }

        [PunRPC]
        public void SyncAddItemRPC(byte index) { }

        [PunRPC]
        public void AddItemRPC(byte index) { }

        public event GlobalDelegates.ByteDelegate OnAddItem;
        public event GlobalDelegates.ByteDelegate OnAddItemFeedback;

        public void RequestRemoveItem(byte index) { }

        public void RequestRemoveItem(Item item) { }

        [PunRPC]
        public void SyncRemoveItemRPC(byte index) { }

        [PunRPC]
        public void RemoveItemRPC(byte index) { }

        public event GlobalDelegates.ByteDelegate OnRemoveItem;
        public event GlobalDelegates.ByteDelegate OnRemoveItemFeedback;
        public void RequestActivateItem(byte index)
        {
            
        }

        public void SyncActivateItemRPC(byte index)
        {
            
        }

        public void ActivateItemRPC(byte index)
        {
            
        }

        public event GlobalDelegates.BoolDelegate OnActivateItem;
        public event GlobalDelegates.BoolDelegate OnActivateItemFeedback;
    }
}