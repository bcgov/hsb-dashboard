using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace HSB.API;

/// <summary>
///
/// </summary>
public class XlsExporter : IXlsExporter
{
    #region Variables
    private readonly XSSFWorkbook _workbook;
    private readonly XSSFColor _green = new();
    private readonly ICellStyle _regularStyle;
    private readonly ICellStyle _boldStyle;
    #endregion

    #region Properties
    /// <summary>
    ///
    /// </summary>
    public XlsExporter()
    {
        _workbook = new XSSFWorkbook();
        _green.ARGBHex = "008000";
        _regularStyle = CreateStyle(10, false, false, HorizontalAlignment.Right);
        _boldStyle = CreateStyle(10, true, false, HorizontalAlignment.Right);
    }
    #endregion

    #region Constructors
    #endregion

    #region Methods
    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.FileSystemItem> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var workbook = new XSSFWorkbook();
        var sheet = (XSSFSheet)workbook.CreateSheet(sheetName);

        return workbook;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.OperatingSystemItem> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var workbook = new XSSFWorkbook();
        var sheet = (XSSFSheet)workbook.CreateSheet(sheetName);

        return workbook;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.ServerItem> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var workbook = new XSSFWorkbook();
        var sheet = (XSSFSheet)workbook.CreateSheet(sheetName);
        var rowIndex = 0;

        // Header
        AddHeaders(sheet.CreateRow(rowIndex++), new[] { "Tenant", "Org Code", "Organization", "ServiceNowKey", "Name", "OS", "Capacity (bytes)", "Available Space (bytes)" });

        // Content
        foreach (var item in items)
        {
            AddContent(workbook, sheet.CreateRow(rowIndex++), item);
        }

        return workbook;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.ServerHistoryItemSmall> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var workbook = new XSSFWorkbook();
        var sheet = (XSSFSheet)workbook.CreateSheet(sheetName);
        var rowIndex = 0;

        // Header
        AddHeaders(sheet.CreateRow(rowIndex++), new[] { "Date", "Tenant", "Org Code", "Organization", "ServiceNowKey", "Name", "OS", "Capacity (bytes)", "Available Space (bytes)" });

        // Content
        foreach (var item in items)
        {
            AddContent(workbook, sheet.CreateRow(rowIndex++), item);
        }

        return workbook;
    }

    private static void AddHeaders(IRow row, string[] labels)
    {
        for (var i = 0; i < labels.Length; i++)
        {
            var cell = row.CreateCell(i);
            cell.SetCellValue(labels[i]);
        }
    }

    private static void AddContent(IWorkbook workbook, IRow row, Entities.ServerItem item)
    {
        var numberStyle = workbook.CreateCellStyle();
        var numberFormat = workbook.CreateDataFormat().GetFormat("#,#0");
        numberStyle.DataFormat = numberFormat;

        var cell0 = row.CreateCell(0);
        cell0.SetCellValue(item.Tenant?.Code);
        var cell1 = row.CreateCell(1);
        cell1.SetCellValue(item.Organization?.Code);
        var cell2 = row.CreateCell(2);
        cell2.SetCellValue(item.Organization?.Name);
        var cell3 = row.CreateCell(3);
        cell3.SetCellValue(item.ServiceNowKey);
        var cell4 = row.CreateCell(4);
        cell4.SetCellValue(item.Name);
        var cell5 = row.CreateCell(5);
        cell5.SetCellValue(item.OperatingSystemItem?.Name);
        var cell6 = row.CreateCell(6);
        cell6.SetCellType(CellType.Numeric);
        cell6.CellStyle = numberStyle;
        cell6.SetCellValue(item.Capacity ?? 0);
        var cell7 = row.CreateCell(7);
        cell7.SetCellType(CellType.Numeric);
        cell7.CellStyle = numberStyle;
        cell7.SetCellValue(item.AvailableSpace ?? 0);
    }

    private static void AddContent(IWorkbook workbook, IRow row, Entities.ServerHistoryItem item)
    {
        var numberStyle = workbook.CreateCellStyle();
        var numberFormat = workbook.CreateDataFormat().GetFormat("#,#0");
        numberStyle.DataFormat = numberFormat;

        var dateStyle = workbook.CreateCellStyle();
        var dateFormat = workbook.CreateDataFormat().GetFormat("yyyy/MM/dd");
        dateStyle.DataFormat = dateFormat;

        var cell0 = row.CreateCell(0);
        cell0.SetCellValue(item.CreatedOn.Date);
        cell0.CellStyle = dateStyle;
        var cell1 = row.CreateCell(1);
        cell1.SetCellValue(item.Tenant?.Code);
        var cell2 = row.CreateCell(2);
        cell2.SetCellValue(item.Organization?.Code);
        var cell3 = row.CreateCell(3);
        cell3.SetCellValue(item.Organization?.Name);
        var cell4 = row.CreateCell(4);
        cell4.SetCellValue(item.ServiceNowKey);
        var cell5 = row.CreateCell(5);
        cell5.SetCellValue(item.Name);
        var cell6 = row.CreateCell(6);
        cell6.SetCellValue(item.OperatingSystemItem?.Name);
        var cell7 = row.CreateCell(7);
        cell7.SetCellType(CellType.Numeric);
        cell7.CellStyle = numberStyle;
        cell7.SetCellValue(item.Capacity ?? 0);
        var cell8 = row.CreateCell(8);
        cell8.SetCellType(CellType.Numeric);
        cell8.CellStyle = numberStyle;
        cell8.SetCellValue(item.AvailableSpace ?? 0);
    }

    private static void AddContent(IWorkbook workbook, IRow row, Entities.ServerHistoryItemSmall item)
    {
        var numberStyle = workbook.CreateCellStyle();
        var numberFormat = workbook.CreateDataFormat().GetFormat("#,#0");
        numberStyle.DataFormat = numberFormat;

        var dateStyle = workbook.CreateCellStyle();
        var dateFormat = workbook.CreateDataFormat().GetFormat("yyyy/MM/dd");
        dateStyle.DataFormat = dateFormat;

        var cell0 = row.CreateCell(0);
        cell0.SetCellValue(item.CreatedOn.Date);
        cell0.CellStyle = dateStyle;
        var cell1 = row.CreateCell(1);
        cell1.SetCellValue(item.Tenant?.Code);
        var cell2 = row.CreateCell(2);
        cell2.SetCellValue(item.Organization?.Code);
        var cell3 = row.CreateCell(3);
        cell3.SetCellValue(item.Organization?.Name);
        var cell4 = row.CreateCell(4);
        cell4.SetCellValue(item.ServiceNowKey);
        var cell5 = row.CreateCell(5);
        cell5.SetCellValue(item.Name);
        var cell6 = row.CreateCell(6);
        cell6.SetCellValue(item.OperatingSystemItem?.Name);
        var cell7 = row.CreateCell(7);
        cell7.SetCellType(CellType.Numeric);
        cell7.CellStyle = numberStyle;
        cell7.SetCellValue(item.Capacity ?? 0);
        var cell8 = row.CreateCell(8);
        cell8.SetCellType(CellType.Numeric);
        cell8.CellStyle = numberStyle;
        cell8.SetCellValue(item.AvailableSpace ?? 0);
    }
    #endregion

    #region Helpers
    private ICellStyle CreateStyle(double size, bool bold, bool wrap, HorizontalAlignment horizontalAlignment, XSSFColor? color = null)
    {
        color ??= new XSSFColor
        {
            ARGBHex = "000000"
        };

        IFont font = CreateFont(size, bold, color);
        ICellStyle style = _workbook.CreateCellStyle();
        style.SetFont(font);
        style.WrapText = wrap;
        style.Alignment = horizontalAlignment;
        return style;
    }

    private IFont CreateFont(double size, bool bold, XSSFColor color)
    {
        IFont font = _workbook.CreateFont();
        font.FontName = "Verdana";
        font.FontHeightInPoints = size;
        font.IsBold = bold;
        font.Color = color.Index;
        return font;
    }

    private void AddTitle(XSSFSheet sheet, int index, string? text)
    {
        ICellStyle titleStyle = CreateStyle(14, true, false, HorizontalAlignment.Center);

        IRow titleRow = sheet.CreateRow(index);
        _ = sheet.AddMergedRegion(CellRangeAddress.ValueOf("A1:E1"));
        ICell titleCell = titleRow.CreateCell(0);
        titleCell.CellStyle = titleStyle;
        titleCell.SetCellValue(text);
    }

    private static ICell AddTextCell(IRow row, int cellIndex, ICellStyle style, string? value)
    {
        ICell cellT = row.CreateCell(cellIndex);
        cellT.CellStyle = style;
        cellT.SetCellValue(value);
        return cellT;
    }

    private static ICell AddNumberCell(IRow row, int cellIndex, ICellStyle style, double value)
    {
        ICell cellT = row.CreateCell(cellIndex);
        cellT.CellStyle = style;
        cellT.SetCellValue(value);
        return cellT;
    }

    private static ICell AddFormulaCell(IRow row, int cellIndex, ICellStyle style, string formula)
    {
        ICell cellT = row.CreateCell(cellIndex);
        cellT.CellStyle = style;
        cellT.SetCellType(CellType.Formula);
        cellT.SetCellFormula(formula);
        return cellT;
    }
    #endregion
}
