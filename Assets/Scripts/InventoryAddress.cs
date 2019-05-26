public class InventoryAddress
{
    public InventorySection InventorySection;

    public int EquipmentIndex;

    public int StoragePage;
    public int StorageColumn;
    public int StorageRow;

    public static InventoryAddress NewEquipmentAddress(int equipmentIndex)
    {
        return new InventoryAddress()
        {
            InventorySection = InventorySection.Equipment,
            EquipmentIndex = equipmentIndex
        };
    }

    public static InventoryAddress NewStorageAddress(int storagePage, int storageColumn, int storageRow)
    {
        return new InventoryAddress()
        {
            InventorySection = InventorySection.Storage,
            StoragePage = storagePage,
            StorageColumn = storageColumn,
            StorageRow = storageRow
        };
    }
}
