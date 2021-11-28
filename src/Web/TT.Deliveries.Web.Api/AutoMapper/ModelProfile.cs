using AutoMapper;
using SharedCodeLibrary.Interfaces;
using System;
using TT.Deliveries.Web.Api.Contracts;

namespace TT.Deliveries.Web.Api.AutoMapper
{
    public class ModelProfile : Profile
    {
        public ModelProfile(IDateTimeProvider dateTimeProvider)
        {
            CreateMap<AccessWindow, Core.Entities.AccessWindow>()
                .ForMember(d => d.EndTime, opt => opt.MapFrom(src => dateTimeProvider.ConvertFrom(src.EndTime)))
                .ForMember(d => d.StartTime, opt => opt.MapFrom(src => dateTimeProvider.ConvertFrom(src.StartTime)));

            CreateMap<DeliveryRecipient, Core.Entities.DeliveryRecipient>();
            CreateMap<Order, Core.Entities.Order>()
                .ConstructUsing(o => CreateOrderFrom(o));

            CreateMap<Delivery, Core.Entities.Delivery>()
                .ForMember(d => d.State, opt => opt.MapFrom(src => Enum.Parse<Core.Enums.DeliveryState>(src.State, true)));
        }

        private Core.Entities.Order CreateOrderFrom(Order o)
        {
            if (o == null)
            {
                return null;
            }

            return new Core.Entities.Order()
            {
                Id = o.Id,
                OrderDetails = new Core.Entities.OrderDetails()
                {
                    OrderNumber = o.OrderNumber,
                    Sender = o.Sender
                }
            };
        }
    }
}
