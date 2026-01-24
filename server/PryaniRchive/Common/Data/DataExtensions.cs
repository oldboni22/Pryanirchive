using Microsoft.EntityFrameworkCore;

namespace Common.Data;

public static class DataExtensions
{
    extension(DbContextOptionsBuilder optionsBuilder)
    {
        public DbContextOptionsBuilder AddTimeStampInterceptor()
        {
            return optionsBuilder
                .AddInterceptors(new TimeStampInterceptor());
        }
    }
}
