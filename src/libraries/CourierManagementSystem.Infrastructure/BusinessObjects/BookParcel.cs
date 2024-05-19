using CourierManagementSystem.Infrastructure.Entities;
using CourierManagementSystem.Infrastructure.Enums;
using DevSkill.Data;

namespace CourierManagementSystem.Infrastructure.BusinessObjects;

public class BookParcel
{
    public Guid Id { get; set; }

    public string BookParcelFromName { get; set; } = string.Empty;
    public string BookParcelFromPhoneNumber { get; set; } = string.Empty;
    public string BookParcelFromAddress { get; set; } = string.Empty;

    public string BookParcelToName { get; set; } = string.Empty;
    public string BookParcelToPhoneNumber { get; set; } = string.Empty;
    public string BookParcelToAddress { get; set; } = string.Empty;

    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool? IsActive { get; set; }
    public Status? Status { get; set; }
    public string? TracId { get; set; }

    public IList<Item> Items { get; set; }
}
