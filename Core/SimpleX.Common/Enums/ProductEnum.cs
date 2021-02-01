namespace SimpleX.Common.Enums
{
    public enum TaxRate
    {
        None = 0,
        Low = 10,
        High = 30
    }

    public enum ProductMovementType
    {
        DeliveredToCustomer = 10,
        ReturnedFromCustomer = 11,
        DeliveryDeletedByUser = 12,
        ReceivedFromSupplier = 20,
        ReturnedToSupplier = 21,
        ReceptionDeletedByUser = 22,
        StockInventory = 100,
        StockIn = 110,
        StockOut = 120
    }
}
