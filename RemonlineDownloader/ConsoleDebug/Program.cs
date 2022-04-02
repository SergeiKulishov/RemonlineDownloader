using RemonlineDownloader.BL.Services;
using RemskladDesktop;

// await WriteFunctions.WriteCustomers(124, "customersTest1.json"); 
// await WriteFunctions.WriteOrders(192, "MainServiceTest.json" ); // Моя мастерская
// await WriteFunctions.WriteOrders(16, "TradeIdTest.json", 64963); // /ТрейдИн
// await WriteFunctions.WriteOrders(4, "OptTest.json", 109365); // ОПТ
// await WriteFunctions.WriteOrders(5, "RealizationTest.json", 100544); // Реализация
//await WriteFunctions.WriteOrdersForHelloClient(5, "RealizationHelloCLient.csv"); // Реализация
await WriteFunctions.WriteOrdersForHelloClient(5, "TradeIdTestHelloClinet.json", 64963); // /ТрейдИн

// await WriteFunctions.WriteWarehouse(35, "warehouse.json");

//var locations = await ConnectionWithRemonline.GetLacations();


Console.WriteLine();

Console.ReadKey();