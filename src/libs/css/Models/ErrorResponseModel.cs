namespace HSB.CSS.Models;

public class ErrorResponseModel
{
    #region Properties
    public string Message { get; set; } = "";
    #endregion

    #region Constructors
    public ErrorResponseModel() { }

    public ErrorResponseModel(string message)
    {
        this.Message = message;
    }
    #endregion
}
