namespace HSB.Models.ServiceNow;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResponseModel<T>
{
    #region Properties
    /// <summary>
    /// get/set - The response result.
    /// </summary>
    public T? Result { get; set; }
    #endregion
}
