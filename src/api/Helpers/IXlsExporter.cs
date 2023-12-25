using NPOI.XSSF.UserModel;

namespace HSB.API;

/// <summary>
///
/// </summary>
public interface IXlsExporter
{
    #region Methods
    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.FileSystemItem> items);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.OperatingSystemItem> items);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    XSSFWorkbook GenerateExcel(string sheetName, IEnumerable<Entities.ServerItem> items);
    #endregion
}
