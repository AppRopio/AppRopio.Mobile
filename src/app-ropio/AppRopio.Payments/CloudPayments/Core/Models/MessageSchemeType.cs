using System.Runtime.Serialization;
namespace AppRopio.Payments.CloudPayments.Core.Models
{
    public enum MessageSchemeType
    {
        /// <summary>
        /// Single message scheme (SMS) 
        /// </summary>
        [EnumMember(Value = "single")]
        Single,

        /// <summary>
        /// Dual message scheme (DMS)
        /// </summary>
        [EnumMember(Value = "dual")]
        Dual
    }
}