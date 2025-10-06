// Data/AppSettings.cs
using System.IO;
using System.Windows.Forms;
namespace MarketAnalyzerApp.Data;

public static class AppSettings
{
    public static readonly string DatasetPath =
        Path.Combine(Application.StartupPath, "Data", "amazon_small_dataset.csv");
    public const int DataLimit = 100000;
}
