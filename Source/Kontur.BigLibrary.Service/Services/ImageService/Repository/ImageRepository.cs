using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.ImageService.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public ImageRepository(IDbConnectionFactory connectionFactory) => this.connectionFactory = connectionFactory;

        public async Task<Image> GetAsync(int id, CancellationToken cancellation)
        {
            using var db = await connectionFactory.OpenAsync(cancellation);
            
            return await db.QueryFirstOrDefaultAsync<Image>(getImageSql, new {Id = id});
        }

        public async Task<int?> GetMaxImageIdAsync(CancellationToken cancellation)
        {
            using var db = await connectionFactory.OpenAsync(cancellation);
            
            return await db.ExecuteScalarAsync<int>(getMaxImageIdSql);
        }

        public async Task<Image> SaveAsync(Image image, CancellationToken cancellation)
        {
            using var db = await connectionFactory.OpenAsync(cancellation);
            
            var parameters = new
            {
                image.Id,
                image.Data,
                image.IsDeleted
            };

            return await db.QueryFirstOrDefaultAsync<Image>(saveImageSql, parameters);
        }

        private static readonly string getImageSql = @"
            select * from book_image 
                where id = @Id;";

        private static readonly string saveImageSql = @"
            insert into book_image(id, data, is_deleted) 
                values(@Id,
                       @Data,
                       @IsDeleted)
            on conflict (id)
            do update set data = @Data,
                          is_deleted = @IsDeleted
            returning *;";

        private static readonly string getMaxImageIdSql = @"select max(id) from book_image;";
    }
}