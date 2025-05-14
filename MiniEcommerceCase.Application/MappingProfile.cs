using AutoMapper;
using MiniEcommerceCase.Application.DTOs.Requests.Order;
using MiniEcommerceCase.Application.DTOs.Responses.Order;
using MiniEcommerceCase.Domain.Entities;
using MiniEcommerceCase.Domain.Enums;

namespace MiniEcommerceCase.Application
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<CreateOrderRequestDto, Order>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse<PaymentMethod>(src.PaymentMethod)));

            CreateMap<Order, CreateOrderResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Order, OrderListItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));
        }
    }
}
