namespace TT.Deliveries.Core.Interfaces
{
    public interface IUrlProviderTemplates
    {
        string GetDeliveryUrlTemplate { get; }

        string UpdateDeliveryUrlTemplate { get; }

        string CancelDeliveryUrlTemplate { get; }
    }
}
