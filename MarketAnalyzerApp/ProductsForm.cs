using MarketAnalyzerApp.Data;
using MarketAnalyzerApp.Services;
using System.Windows.Forms.DataVisualization.Charting;

namespace MarketAnalyzerApp;

public partial class ProductsForm : Form
{
    private readonly IProductRepository _repo;
    private readonly IProductSearch _search;
    private List<Product> _products = new();

    public ProductsForm() : this(new CsvProductRepository(), new BrandSearchService()) { }

    // DI/testing constructor
    public ProductsForm(IProductRepository repo, IProductSearch search)
    {
        InitializeComponent();
        _repo = repo;
        _search = search;
    }

    private void ProductsForm_Load(object sender, EventArgs e)
    {
        CustomizeGridAppearance();
        _products = _repo.GetAll().ToList();
        RefreshGrid();

        ConfigureChart();
        PricesChart.Visible = false;
    }

    private void CustomizeGridAppearance()
    {
        ProductsGrid.AutoGenerateColumns = false;
        ProductsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        ProductsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        ProductsGrid.RowHeadersVisible = false;

        var titleCol = new DataGridViewTextBoxColumn
        {
            HeaderText = "Title",
            DataPropertyName = "Title"
        };
        ProductsGrid.Columns.Add(titleCol);

        var brandCol = new DataGridViewTextBoxColumn
        {
            HeaderText = "Brand",
            DataPropertyName = "Brand"
        };
        ProductsGrid.Columns.Add(brandCol);
    }

    private async void SearchText_TextChanged(object sender, EventArgs e)
    {
        int lengthBeforePause = SearchText.Text.Length;
        await Task.Delay(300);
        int lengthAfterPause = SearchText.Text.Length;

        if (lengthBeforePause == lengthAfterPause)
            RefreshGrid();
    }

    private void RefreshGrid()
    {
        var filtered = _search.ByBrand(_products, SearchText.Text);
        ProductsGrid.DataSource = filtered;
    }

    private void RefreshChart(Product selectedProduct)
    {
        PricesChart.Visible = true;

        PricesChart.Series["price"].Points.Clear();
        PricesChart.Series["price"].Points.AddXY("dec 2021", Calculations.ConvertToDecimal(selectedProduct.Dec2021Price));
        PricesChart.Series["price"].Points.AddXY("may 2022", Calculations.ConvertToDecimal(selectedProduct.May2022Price)); // fixed "rice" -> "price"
        PricesChart.Series["price"].Points.AddXY("jul 2022", Calculations.ConvertToDecimal(selectedProduct.Jul2022Price));
        PricesChart.Series["price"].Points.AddXY("sep 2022", Calculations.ConvertToDecimal(selectedProduct.Sep2022Price));
        PricesChart.Series["price"].Points.AddXY("oct 2022", Calculations.ConvertToDecimal(selectedProduct.Oct2022Price));

        foreach (DataPoint point in PricesChart.Series["price"].Points)
            point.Label = point.YValues[0].ToString();
    }

    private void ConfigureChart()
    {
        PricesChart.Series.Clear();
        PricesChart.ChartAreas.Clear();

        PricesChart.ChartAreas.Add(new ChartArea());
        PricesChart.Series.Add(new Series("price")
        {
            ChartType = SeriesChartType.Area
        });
    }

    private void ProductsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        var selectedProduct = (Product)ProductsGrid.Rows[e.RowIndex].DataBoundItem;
        RefreshChart(selectedProduct);
    }

    private void CalculateChangeButton_Click(object sender, EventArgs e)
    {
        var selectedRows = ProductsGrid.SelectedRows;
        if (selectedRows.Count == 0) return;

        var selectedProducts = new List<Product>();
        foreach (DataGridViewRow row in selectedRows)
            selectedProducts.Add((Product)row.DataBoundItem);

        string message = Calculations.CalculateChangeInPrice(selectedProducts);
        MessageBox.Show(message);
    }

    private void BiggestIncreaseBtn_Click(object sender, EventArgs e)
    {
        var brandsWithHighestPriceIncrease = Calculations.CalculateBrandsPriceChange(_products, SortType.Increase);

        string message = "";
        foreach (var brand in brandsWithHighestPriceIncrease)
            message += $"{brand.Key} increased price for {brand.Value}%\n\n";

        MessageBox.Show(message, "top brands with highest price increase between dec 2021 and oct 2022");
    }

    private void BiggestDecreaseBtn_Click(object sender, EventArgs e)
    {
        var brandsWithHighestPriceIncrease = Calculations.CalculateBrandsPriceChange(_products, SortType.Decrease);

        string message = "";
        foreach (var brand in brandsWithHighestPriceIncrease)
            message += $"{brand.Key} dropped price for {brand.Value}%\n\n";

        MessageBox.Show(message, "top brands with highest price decrease between dec 2021 and oct 2022");
    }
}
