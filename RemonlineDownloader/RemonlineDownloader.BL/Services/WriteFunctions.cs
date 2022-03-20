using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemonlineDownloader.Core.Models.Orders;
using RemskladDesktop;
using RemskladDesktop.Reporter;

namespace RemonlineDownloader.BL.Services
{
    public class WriteFunctions
    {
        public static async Task WriteOrders(int pageCount, string fileName)
        {
            var ordersPagesJson = "[";
            string str = "";
            try
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    str = await ConnectionWithRemonline.GetRawJsonOrders(i);
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
    }
}
