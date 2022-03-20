using RemonlineDownloader.BL.Services;

await WriteFunctions.WriteCustomers(124, "customersTest1.json");
await WriteFunctions.WriteOrders(192, "OrdersTest.json");
await WriteFunctions.WriteWarehouse(35, "warehouse.json");
Console.ReadKey();