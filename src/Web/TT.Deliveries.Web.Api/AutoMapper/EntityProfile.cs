using AutoMapper;
using SharedCodeLibrary.Interfaces;
using TT.Deliveries.Core.Interfaces;
using TT.Deliveries.Web.Api.Contracts;
using TT.Deliveries.Web.Api.Interfaces;

namespace TT.Deliveries.Web.Api.AutoMapper
{
    public class EntityProfile : Profile
    {
        public EntityProfile(IUrlsProvider urlsProvider,
            IDateTimeProvider dateTimeProvider,
            IActionsBuilder actionsBuilder)
        {
            CreateMap<Core.Entities.AccessWindow, AccessWindow>()
                .ForMember(d => d.EndTime, opt => opt.MapFrom(src => dateTimeProvider.ConvertToDateString(src.EndTime)))
                .ForMember(d => d.StartTime, opt => opt.MapFrom(src => dateTimeProvider.ConvertToDateString(src.StartTime)));

            CreateMap<Core.Entities.Delivery, Delivery>()
                .ForMember(d => d.Self, opt => opt.MapFrom(src => urlsProvider.GetDeliveryUrl(src.Id)))
                .ForMember(d => d.State, opt => opt.MapFrom(src => src.State.ToString()))
                .ForMember(d => d.Actions, opt => opt.MapFrom(src => actionsBuilder.GetActions(src.Id, src.State)));
            CreateMap<Core.Entities.Delivery, string>()
                .ConvertUsing(src => urlsProvider.GetDeliveryUrl(src.Id));

            CreateMap<Core.Entities.DeliveryRecipient, DeliveryRecipient>();
            CreateMap<Core.Entities.Order, Order>()
                .ForMember(d => d.OrderNumber, opt => opt.MapFrom(src => src.OrderDetails.OrderNumber))
                .ForMember(d => d.Sender, opt => opt.MapFrom(src => src.OrderDetails.Sender));
        }
    }
}
