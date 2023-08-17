using System.ComponentModel;

namespace Authentication.Domain.Enums;
public enum EUserType : ushort
{
    [Description("SysManager")]
    SysManager = 1,

    [Description("Manager")]
    Manager,

    [Description("Client")]
    Client
}
