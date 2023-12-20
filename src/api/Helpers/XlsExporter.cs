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
    public XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.ConfigurationItem> items)
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

        return workbook;
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
