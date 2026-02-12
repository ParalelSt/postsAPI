using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Entities;

namespace PostsAPI.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): 
        base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; } = null!;
};
