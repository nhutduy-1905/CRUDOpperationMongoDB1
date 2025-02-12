using CRUDOpperationMongoDB1.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CRUDOpperationMongoDB1.Services
{
    public  static class TicketMappings
    {
        private static DateTime? toDate;

        //private readonly IMongoCollection<Items> _itemsCollection;
        public static Ticket ToTicket(this TicketDTO dto)
        {
            var dateRange = dto.TravelDateRange.Split(" - ");
            DateTime fromDate = DateTime.ParseExact(dateRange[0], "dd/MM/yyyy", null);
            DateTime? value = dateRange.Length > 1 ? DateTime.ParseExact(dateRange[1], "dd/MM/yyyy", null) : null;
          

            return new Ticket
            {
                TicketType = dto.TicketType,
                FromAddress = dto.FromAddress,
                ToAddress = dto.ToAddress,
                FromDate = fromDate,
                ToDate = toDate,
                Quantity = dto.Quantity,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };
        }

        public static TicketDTO ToTicketDTO(this Ticket ticket)
        {
            string travelDateRange = ticket.ToDate.HasValue
                ? $"{ticket.FromDate:dd/MM/yyyy} - {ticket.ToDate:dd/MM/yyyy}"
                : $"{ticket.FromDate:dd/MM/yyyy}";

            return new TicketDTO
            {
                TicketType = ticket.TicketType,
                FromAddress = ticket.FromAddress,
                ToAddress = ticket.ToAddress,
                TravelDateRange = travelDateRange,
                Quantity = ticket.Quantity,
                CustomerName = ticket.CustomerName,
                CustomerPhone = ticket.CustomerPhone
            };
        }
    }

    // code cũ
    //public ItemService(IOptions<MongoDbSettings> mongoDbSettings)
    //{
    //    var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
    //    var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
    //    _itemsCollection = mongoDatabase.GetCollection<Items>("Items");

    //}


    //// Lấy tất cả các mục và chuyển đổi chúng sang DTO
    //public async Task<List<ItemDTO>> GetAsync()
    //{
    //    var items = await _itemsCollection.Find(_ => true).ToListAsync();
    //    var itemDtos = items.Select(item => new ItemDTO
    //    {
    //        Name = item.Name,
    //        Price = item.Price,
    //        Description = item.Description
    //    }).ToList();

    //    return itemDtos;
    //}
    //// Lấy 1 mục cụ thể và chuyển đổi nó sang DTO
    //public async Task<ItemDTO?> GetAsync(string id)
    //{
    //    var objectId = new ObjectId(id);
    //    var item = await _itemsCollection.Find(x => x.Id == objectId).FirstOrDefaultAsync();
    //    if (item == null) return null;

    //    // Chuyển từ Item sang ItemDTO
    //    var itemDto = new ItemDTO
    //    {
    //        Id = item.Id,
    //        Name = item.Name,
    //        Price = item.Price,
    //        Description = item.Description
    //    };

    //    return itemDto;
    //}


    //// Lấy tất cả các mục


    ////test
    ////public async Task<List<Items>> GetAsync() =>
    ////   await _itemsCollection.Find(_ => true).ToListAsync();

    ////// Lấy 1 mục dựa trên id
    ////public async Task<Items?> GetAsync(string id)
    ////{
    //// Chuyển đổi string sang ObjectId
    ////    var objectId = new ObjectId(id);
    ////    return await _itemsCollection.Find(x => x.Id == objectId).FirstOrDefaultAsync();
    ////}

    //// Tạo mục mới
    //public async Task<Items> CreateAsync(CreateItemDTO dto)
    //{
    //    // Chuyển đổi CreateItemDTO sang Items
    //    var newItem = new Items
    //    {
    //        Name = dto.Name,
    //        Price = dto.Price,
    //        Description = dto.Description,
    //        Brand = dto.Brand,
    //        Color = dto.Color
    //    };

    //    // Lưu Items vào MongoDB và lấy lại đối tượng mới với Id được tự động gán
    //    await _itemsCollection.InsertOneAsync(newItem);

    //    // Trả về đối tượng Items sau khi đã được lưu, bao gồm cả ObjectId
    //    return newItem;
    //}



    //// Cập nhật mục
    //public async Task UpdateAsync(string id, Items updatedItem)
    //{
    //    var objectId = new ObjectId(id);  // Chuyển đổi string sang ObjectId
    //    await _itemsCollection.ReplaceOneAsync(x => x.Id == objectId, updatedItem);
    //}

    //// Xóa mục
    //public async Task RemoveAsync(string id)
    //{
    //    var objectId = new ObjectId(id);  // Chuyển đổi string sang ObjectId
    //    await _itemsCollection.DeleteOneAsync(x => x.Id == objectId);
    //}

    //internal object Find(Func<object, bool> value)
    //{
    //    throw new NotImplementedException();
    //}

}

