using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using RemskladDesktop.Cashbox;
using RemskladDesktop.Orders;

namespace RemskladDesktop
{
    public class ConnectionWithRemonline
    {
        protected static string responseToken;

        public static async Task PostRequestAsync()
        {
            WebRequest request = WebRequest.Create("https://api.remonline.ru/token/new");
            request.Method = "POST";
            string data = "api_key=a49d338c1466430e9568ca6ea77c5cda";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string token = await reader.ReadToEndAsync();
                    ConnectionWithRemonline.responseToken = token;
                }
            }
            response.Close();
            Console.WriteLine("Запрос выполнен...");
        }

        private static async Task<string> GetToken()
        {
            await PostRequestAsync();
            var jsonResponceToken = ConnectionWithRemonline.responseToken;
            ResponceToken responceToken = JsonConvert.DeserializeObject<ResponceToken>(jsonResponceToken);
            return responceToken.token;
        }

        public static async Task<string> GetPageFromRemonline(string token, int page)
        {
            string url = $"https://api.remonline.ru/warehouse/goods/28208?page={page}&token={token}";
            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }

        public static async Task<List<Item>> GetCollectionOfItems(int pageCount = 29)
        {
            List<Item> ListItem = new List<Item>();
            for (int i = 1; i <= pageCount; i++)
            {
                string responceMessage = await ConnectionWithRemonline.GetPageFromRemonline(await ConnectionWithRemonline.GetToken(), i);
                Item thing = JsonConvert.DeserializeObject<Item>(responceMessage);
                ListItem.Add(thing);
            }
            return ListItem;
        }

        public static Dictionary<string, Datum> GetItemByArticle(IEnumerable<Item> ListofItems, IEnumerable<string> arrayOfArticles)
        {
            Dictionary<string, Datum> ItemsfromWarehouse = new Dictionary<string, Datum>();

            foreach (var s in ListofItems)
            {
                foreach (var p in s.data)
                {
                    try
                    {
                        foreach (string i in arrayOfArticles)
                        {
                            if (p.article == i)
                            {
                                ItemsfromWarehouse.Add(p.article, p);
                            }

                        }
                    }
                    catch (System.Exception)
                    {
                        if (p.article == null)
                        {
                            System.Console.WriteLine("У этой позиции отсутствует артикль :");
                            System.Console.WriteLine(p.title);

                        }
                        else
                        {
                            System.Console.WriteLine("У этой позиции отсутствует артикль или найдено совпадение:");
                            System.Console.WriteLine(p.title);
                            throw;
                        }
                    }

                }
            }
            return ItemsfromWarehouse;
        }

        public static async Task<Dictionary<int, Cashbox.Cashbox>> GetCashboxInfo()
        {
            string token = await GetToken();
            string url = $"https://api.remonline.ru/cashbox/?token={token}";
            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);


                var Boxes = JsonConvert.DeserializeObject<CashboxesFromRemOnline>(response);
                Dictionary<int, Cashbox.Cashbox> GSCashboxes = new Dictionary<int, Cashbox.Cashbox>();
                foreach (var box in Boxes.data)
                {
                    GSCashboxes.Add(box.id, box);
                }
                return GSCashboxes;
            }
        }


        public static async Task<string> GetOrders(int pages = 1 , int locationId = 22732)
        {
            string token = await GetToken();
            string url = $"https://api.remonline.ru/order/" +
                         $"?token={token}" + $"&branches[]={locationId}&page={pages}";
            //  $"&statuses[]=115411&statuses[]=115410&statuses[]=310524&statuses[]=395193&statuses[]=115412" +
            //$"&statuses[]=115438&statuses[]=117552&statuses[]=143928&statuses[]=161929&statuses[]=343366&statuses[]=115414&statuses[]=117556&statuses[]=115413&statuses[]=123194";//&statuses[]=115415";


            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }

        public static async Task<string> GetCustomers(int pages = 1)
        {
            string token = await GetToken();
            string url = $"https://api.remonline.ru/clients/?token={token}&page={pages}";

            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }



        public static async Task<string> GetStatuses()
        {
            string token = await GetToken();
            string url = $"https://api.remonline.ru/statuses/?token={token}";


            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }

        public static async Task<string> GetLacations()
        {
            string token = await GetToken();
            string url = $"https://api.remonline.ru/branches/?token={token}";


            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }

        public static async Task<Root> GetListOfOrders(int page = 1 , int locationId = 22732)
        {
            var OrdersJson = await GetOrders(page, locationId);
            var orders = JsonConvert.DeserializeObject<Orders.Root>(OrdersJson);

            return orders;
        }

        public static async Task<string> GetItems(int page)
        {
            var token = await GetToken();
            string url = $"https://api.remonline.ru/warehouse/goods/28208?page={page}&token={token}";
            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }

        public static async Task<string> GetRawJsonOrders(int page , int locationId = 22732)
        {
            return await GetOrders(page, locationId);
        }

        public static async Task<string> GetRawJsonCustomers(int page)
        {
            return await GetCustomers(page);
        }

        public static async Task<string> GetRawJsonItems(int page)
        {
            return await GetItems(page);
        }

        public static async Task<string> GetRawJsonComments()
        {
            var token = await GetToken();
            string url = $"https://web.remonline.app/app/orders/table/5380199?token={token}";
            using (var webClient = new WebClient())
            {
                string response = await webClient.DownloadStringTaskAsync(url);
                return response;
            }
        }
    }
}