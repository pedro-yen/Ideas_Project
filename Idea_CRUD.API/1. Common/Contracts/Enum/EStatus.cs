using System.ComponentModel;

namespace Backend.Challenge._1._Common.Contracts.Enum
{
    public enum EStatus
    {
        [Description("On Track")]
        ONTRACK = 1,
        [Description("At Risk")]
        ATRISK = 2,
        [Description("Off Track")]
        OFFTRACK =3,
    }
}
