using AutoMapper;
using CourierManagementSystem.Infrastructure.BusinessObjects;
using CourierManagementSystem.Web.Areas.Operator.Models;

namespace CourierManagementSystem.Web.Areas.Operator.Profiles;

public class OperatorProfile: Profile
{
    public OperatorProfile()
    {
        CreateMap<BookParcelCreateModel, BookParcel>().ReverseMap();
    }
}
