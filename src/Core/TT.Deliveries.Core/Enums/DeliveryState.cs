namespace TT.Deliveries.Core.Enums
{
    public enum DeliveryState
    {
        /// <summary>
        /// Defines a newly created delivery
        /// </summary>
        Created = 0,

        /// <summary>
        /// The delivery has been approved by the recipient
        /// </summary>
        Approved,

        /// <summary>
        /// The delivery has been successfully completed
        /// </summary>
        Completed,

        /// <summary>
        /// The delivery has been cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// The delivery has expired - outside permitted access window
        /// </summary>
        Expired
    }
}