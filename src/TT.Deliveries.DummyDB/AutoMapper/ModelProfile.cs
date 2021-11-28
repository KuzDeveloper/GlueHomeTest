using AutoMapper;
using TT.Deliveries.DummyDB.Entities;

namespace TT.Deliveries.DummyDB.AutoMapper
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Delivery, Core.Entities.Delivery>()
                .ForMember(d => d.AccessWindow, opt => opt.MapFrom(src => Core.Entities.AccessWindow.FromJson(src.AccessWindowDetails)))
                .ForMember(d => d.Recipient, opt => opt.MapFrom(src => Core.Entities.DeliveryRecipient.FromJson(src.RecipientDetails)))
                .ForMember(d => d.State, opt => opt.MapFrom(src => (Core.Enums.DeliveryState)src.State));

            CreateMap<Order, Core.Entities.Order>()
                .ForMember(d => d.OrderDetails, opt => opt.MapFrom(src => Core.Entities.OrderDetails.FromJson(src.Details)));
        }
    }
}
