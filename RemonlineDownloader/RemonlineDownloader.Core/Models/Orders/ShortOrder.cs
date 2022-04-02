namespace RemonlineDownloader.Core.Models.Orders;

public class ShortOrder
{
    public long OrderId { get; set; }
    public string Status { get; set; }
    public string CreationDate { get; set; }
    public string ClosingDate { get; set; }
    public string Manager { get; set; }
    public double Paid { get; set; }
    public string Mailfunction { get; set; }
    public string Appearence { get; set; }
    public string KingOfGood { get; set; }
    public string serial { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string ManagerNotes { get; set; }
    public string EngeneerNotes { get; set; }
    public string ClientName { get; set; }
    public string ClientPhone { get; set; }
}