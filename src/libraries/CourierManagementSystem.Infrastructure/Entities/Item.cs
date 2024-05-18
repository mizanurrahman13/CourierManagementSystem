using DevSkill.Data;

namespace CourierManagementSystem.Infrastructure.Entities;

public class Item: IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public double Total { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public Guid BookParcelId { get; set; }
    public BookParcel? ParcelOrder { get; set; }
}
