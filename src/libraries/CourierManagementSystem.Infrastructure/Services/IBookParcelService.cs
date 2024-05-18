using CourierManagementSystem.Infrastructure.BusinessObjects;

namespace CourierManagementSystem.Infrastructure.Services;

public interface IBookParcelService
{
    Task CreateBookParcel(BookParcel bookParcel);
    Task<(int total, int displayTotal, IList<BookParcel> records)>
        GetBookParcelAsync(int pageIndex,
        int pageSize,
        string searchText,
        string orderBy);
    Task<(int total, int totalDisplay, IList<BookParcel> bookParcels)> GetActiveBookParcelsAsync(int pageIndex, int pageSize, string searchText, string orderBy);
    (int total, int totalDisplay, IList<BookParcel> bookParcels) GetActiveBookParcels(int pageIndex, int pageSize, string searchText, string orderBy);
    Task<BookParcel> GetBookParcelByIdAsync(Guid id);
    BookParcel GetBookParcelById(Guid id);
    Task UpdateBookParcelAsync(BookParcel bookParcel);
    Task UpdateBookParcelAndItemAsync(BookParcel bookParcel);
    void UpdateBookParcel(BookParcel bookParcel);
    (int total, int totalDisplay, IList<BookParcel> trashedBookParcels) GetTrashedBookParcels(
                int pageIndex, int pageSize, string searchText, string orderBy);
    void DeleteBookParcel(Guid id);
}
