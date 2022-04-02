using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemonlineDownloader.Core.Models.Orders;
using RemskladDesktop;
using RemskladDesktop.Orders;
using RemskladDesktop.Reporter;

namespace RemonlineDownloader.BL.Services
{
    public class WriteFunctions
    {
        public static async Task WriteOrders(int pageCount, string fileName, int locationId = 22732)
        {
            var ordersPagesJson = "[";
            string str = "";
            try
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    str = await ConnectionWithRemonline.GetRawJsonOrders(i, locationId);
                    var separatedObject = JsonConvert.DeserializeObject<SeparatorRoot>(str);
                    foreach (var ojb in separatedObject.data)
                    {
                        ordersPagesJson += ojb;
                        ordersPagesJson += ",";
                    }
                    Console.WriteLine($"Page : {i}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                throw;
            }

            var report = ordersPagesJson.Remove(ordersPagesJson.Length - 1);
            report += "]";
            await Reporter.WriteReportInCSVAsync(report, fileName);
        }

        public static async Task WriteCustomers(int pageCount, string fileName)
        {
            var customersPagesJson = "[";
            string str = "";
            try
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    str = await ConnectionWithRemonline.GetRawJsonCustomers(i);
                    var separatedObject = JsonConvert.DeserializeObject<SeparatorRoot>(str);
                    foreach (var ojb in separatedObject.data)
                    {
                        customersPagesJson += ojb;
                        customersPagesJson += ",";
                    }

                    Console.WriteLine($"Page : {i}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                throw;
            }

            var report = customersPagesJson.Remove(customersPagesJson.Length - 1);
            report += "]";
            await Reporter.WriteReportInCSVAsync(report, fileName);
        }

        public static async Task WriteWarehouse(int pageCount, string fileName)
        {
            var itemsPagesJson = "[";
            string str = "";
            try
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    str = await ConnectionWithRemonline.GetItems(i);
                    var separatedObject = JsonConvert.DeserializeObject<SeparatorRoot>(str);
                    foreach (var ojb in separatedObject.data)
                    {
                        itemsPagesJson += ojb;
                        itemsPagesJson += ",";
                    }

                    Console.WriteLine($"Page : {i}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                throw;
            }

            var report = itemsPagesJson.Remove(itemsPagesJson.Length - 1);
            report += "]";
            await Reporter.WriteReportInCSVAsync(report, fileName);
        }
        
        public static async Task WriteOrdersForHelloClient(int pageCount, string fileName, int locationId = 22732)
        {
            var orders = new List<Order>(); 
            try
            {
                for (var i = 1; i <= pageCount; i++)
                {
                    var str = await ConnectionWithRemonline.GetRawJsonOrders(i, locationId);
                    var pageOfOrders = JsonConvert.DeserializeObject<Root>(str);
                    pageOfOrders.data.ForEach(x=>orders.Add(x));
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                throw;
            }

            var report = ProcessOrdersToJson(orders);
            
            await Reporter.WriteReportInCSVAsync(report, fileName);
        }

        private static string ProcessOrdersToCSV(List<Order> listOfOrders)
        {
            var csv = new StringBuilder();

            foreach (var order in listOfOrders)
            {
                var createdAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(order?.created_at)).Date;
                var closedAt = new DateTime();
                if (order?.closed_at is not null)
                {
                    closedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(order?.closed_at)).Date;
                }
                
                var orderString = 
                    $"{createdAt};{order.id};{order.status.name};{closedAt};RemOnline;RemOnline;" +
                    $"{order.payed};{order.malfunction.Trim()};{order.appearance.Trim()};{order.kindof_good};{order.serial};{order.brand};" +
                    $"{order.model};{order.manager_notes.Trim()};{order.engineer_notes.Trim()};{order.client.name};{order.client?.phone?.FirstOrDefault()};";
                csv.AppendLine(orderString);
            }

            return csv.ToString();
        } 
        
        private static string ProcessOrdersToJson(List<Order> listOfOrders)
        {
            var orderJson = new List<string>();

            foreach (var order in listOfOrders)
            {
                var createdAt1 = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(order?.created_at)).Date;
                var createdAt = createdAt1.ToShortDateString();
                string closedAt = "";
                if (order?.closed_at is not null)
                {
                    var closedAt1 = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(order?.closed_at)).Date;
                    closedAt = closedAt1.ToShortDateString();
                }

                var shortOrder = new ShortOrder()
                {
                    CreationDate = createdAt,
                    OrderId = order.id,
                    Status = order.status.name,
                    ClosingDate = closedAt,
                    Manager = "RemOnline",
                    Paid = order.payed,
                    Mailfunction = order.malfunction,
                    Appearence = order.appearance,
                    KingOfGood = order.kindof_good,
                    serial = order.serial,
                    Brand = order.brand,
                    Model = order.model,
                    ManagerNotes = order.manager_notes,
                    EngeneerNotes = order.engineer_notes,
                    ClientName = order.client.name,
                    ClientPhone = order?.client?.phone?.FirstOrDefault()
                };
                var obj = JsonConvert.SerializeObject(shortOrder);
                orderJson.Add(obj);
            }

            return ProcessListOfOrders(orderJson);
        }

        private static string ProcessListOfOrders(List<string> listOfOrders)
        {
            var itemsPagesJson = "[";
            string str = "";
            try
            {
                foreach (var pageCount in listOfOrders)
                {
                    itemsPagesJson += pageCount; 
                    itemsPagesJson += ",";
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                throw;
            }

            var report = itemsPagesJson.Remove(itemsPagesJson.Length - 1);
            report += "]";
            return report;
        }
    }
}
