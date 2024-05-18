using AutoMapper;
using BookParcelEntity = CourierManagementSystem.Infrastructure.Entities.BookParcel;
using BookParcelBO = CourierManagementSystem.Infrastructure.BusinessObjects.BookParcel;
using ItemEntity = CourierManagementSystem.Infrastructure.Entities.Item;
using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;

namespace CourierManagementSystem.Infrastructure.Profiles;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<BookParcelEntity, BookParcelBO>().ReverseMap();
        CreateMap<ItemEntity, ItemBO>().ReverseMap();
    }
}
