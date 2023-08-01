using Microsoft.EntityFrameworkCore;
using todo_api_sqlserver.Models;

namespace todo_api_sqlserver.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todo { get; set; }
    }
}
