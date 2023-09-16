namespace MitraBackend.Models;

public class Report
{
    public int ReportId { get; set; }
    public String ReportType { get; set; }
    public Byte[] UserReport { get; set; }
}