using System.Text.Json.Serialization;
using HSB.CSS.Converters;

namespace HSB.CSS.Models;


[JsonConverter(typeof(UserRoleOperationConverter))]
public class UserRoleOperation
{
  #region Properties
  public string Value { get; set; }
  public static UserRoleOperation Add { get { return new UserRoleOperation("add"); } }
  public static UserRoleOperation Delete { get { return new UserRoleOperation("del"); } }
  #endregion

  #region Constructors
  private UserRoleOperation(string value) { this.Value = value; }
  #endregion

  #region Methods
  public override string ToString()
  {
    return this.Value;
  }
  #endregion
}
