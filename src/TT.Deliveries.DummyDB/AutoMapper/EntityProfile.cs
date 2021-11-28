using AutoMapper;
using TT.Deliveries.DummyDB.Entities;

namespace TT.Deliveries.DummyDB.AutoMapper
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Core.Entities.Delivery, Delivery>()
                .ForMember(d => d.AccessWindowDetails, opt => opt.MapFrom(src => src.AccessWindow.ToJson()))
                .ForMember(d => d.RecipientDetails, opt => opt.MapFrom(src => src.Recipient.ToJson()))
                .ForMember(d => d.State, opt => opt.MapFrom(src => (byte)src.State));

            CreateMap<Core.Entities.Order, Order>()
               .ForMember(d => d.Details, opt => opt.MapFrom(src => src.OrderDetails.ToJson()));
        }
    }
}
