using Microsoft.EntityFrameworkCore;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TreeNode> TreeNodes { get; set; }
    public DbSet<ExceptionJournal> ExceptionJournals { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TreeNode
        modelBuilder.Entity<TreeNode>(entity =>
        {
            entity.HasIndex(e => new { e.TreeName, e.ParentId, e.Name })
                  .IsUnique()
                  .HasDatabaseName("IX_TreeNode_TreeName_ParentId_Name");

            entity.HasOne(e => e.Parent)
                  .WithMany(e => e.Children)
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ExceptionJournal
        modelBuilder.Entity<ExceptionJournal>(entity =>
        {
            entity.HasIndex(e => e.EventId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Code = "admin",
                CreatedAt = now,
                LastLoginAt = now
            },
            new User
            {
                Id = 2,
                Code = "user1",
                CreatedAt = now.AddDays(-30),
                LastLoginAt = now.AddDays(-1)
            },
            new User
            {
                Id = 3,
                Code = "user2",
                CreatedAt = now.AddDays(-60),
                LastLoginAt = now.AddDays(-2)
            },
            new User
            {
                Id = 4,
                Code = "testuser",
                CreatedAt = now.AddDays(-15),
                LastLoginAt = now.AddDays(-5)
            }
        );

        // Seed TreeNodes - Create a hierarchical tree structure
        // Tree 1: Company Organization
        modelBuilder.Entity<TreeNode>().HasData(
            // Root node
            new TreeNode
            {
                Id = 1,
                TreeName = "Company",
                Name = "Acme Corporation",
                ParentId = null,
                CreatedAt = now.AddDays(-90)
            },
            // Level 1 - Departments
            new TreeNode
            {
                Id = 2,
                TreeName = "Company",
                Name = "Engineering",
                ParentId = 1,
                CreatedAt = now.AddDays(-90)
            },
            new TreeNode
            {
                Id = 3,
                TreeName = "Company",
                Name = "Sales",
                ParentId = 1,
                CreatedAt = now.AddDays(-90)
            },
            new TreeNode
            {
                Id = 4,
                TreeName = "Company",
                Name = "HR",
                ParentId = 1,
                CreatedAt = now.AddDays(-90)
            },
            // Level 2 - Engineering sub-teams
            new TreeNode
            {
                Id = 5,
                TreeName = "Company",
                Name = "Backend",
                ParentId = 2,
                CreatedAt = now.AddDays(-80)
            },
            new TreeNode
            {
                Id = 6,
                TreeName = "Company",
                Name = "Frontend",
                ParentId = 2,
                CreatedAt = now.AddDays(-80)
            },
            new TreeNode
            {
                Id = 7,
                TreeName = "Company",
                Name = "DevOps",
                ParentId = 2,
                CreatedAt = now.AddDays(-80)
            },
            // Level 2 - Sales regions
            new TreeNode
            {
                Id = 8,
                TreeName = "Company",
                Name = "North America",
                ParentId = 3,
                CreatedAt = now.AddDays(-75)
            },
            new TreeNode
            {
                Id = 9,
                TreeName = "Company",
                Name = "Europe",
                ParentId = 3,
                CreatedAt = now.AddDays(-75)
            },
            // Tree 2: Product Categories
            new TreeNode
            {
                Id = 10,
                TreeName = "Products",
                Name = "Electronics",
                ParentId = null,
                CreatedAt = now.AddDays(-60)
            },
            new TreeNode
            {
                Id = 11,
                TreeName = "Products",
                Name = "Computers",
                ParentId = 10,
                CreatedAt = now.AddDays(-60)
            },
            new TreeNode
            {
                Id = 12,
                TreeName = "Products",
                Name = "Mobile Devices",
                ParentId = 10,
                CreatedAt = now.AddDays(-60)
            },
            new TreeNode
            {
                Id = 13,
                TreeName = "Products",
                Name = "Laptops",
                ParentId = 11,
                CreatedAt = now.AddDays(-55)
            },
            new TreeNode
            {
                Id = 14,
                TreeName = "Products",
                Name = "Desktops",
                ParentId = 11,
                CreatedAt = now.AddDays(-55)
            },
            new TreeNode
            {
                Id = 15,
                TreeName = "Products",
                Name = "Smartphones",
                ParentId = 12,
                CreatedAt = now.AddDays(-50)
            }
        );

        // Seed ExceptionJournals - Sample exception entries
        modelBuilder.Entity<ExceptionJournal>().HasData(
            new ExceptionJournal
            {
                Id = 1,
                EventId = 1001,
                CreatedAt = now.AddDays(-10),
                ExceptionType = "System.ArgumentNullException",
                ExceptionMessage = "Value cannot be null. (Parameter 'treeName')",
                StackTrace = "   at TestAssignment.Infrastructure.Services.TreeService.GetTree(String treeName) in TreeService.cs:line 45",
                QueryString = "?name=",
                RequestBody = null,
                RequestPath = "/api/tree",
                RequestMethod = "GET"
            },
            new ExceptionJournal
            {
                Id = 2,
                EventId = 1002,
                CreatedAt = now.AddDays(-8),
                ExceptionType = "TestAssignment.Domain.Exceptions.SecureException",
                ExceptionMessage = "Node with the same name already exists in this tree",
                StackTrace = "   at TestAssignment.Infrastructure.Services.TreeService.CreateNode(String treeName, String nodeName, Int64 parentId) in TreeService.cs:line 78",
                QueryString = null,
                RequestBody = "{\"treeName\":\"Company\",\"name\":\"Engineering\",\"parentId\":1}",
                RequestPath = "/api/tree/node",
                RequestMethod = "POST"
            },
            new ExceptionJournal
            {
                Id = 3,
                EventId = 1003,
                CreatedAt = now.AddDays(-5),
                ExceptionType = "Microsoft.EntityFrameworkCore.DbUpdateException",
                ExceptionMessage = "An error occurred while saving the entity changes.",
                StackTrace = "   at Microsoft.EntityFrameworkCore.DbContext.SaveChanges()\n   at TestAssignment.Infrastructure.Services.JournalService.LogException(Exception ex) in JournalService.cs:line 23",
                QueryString = null,
                RequestBody = null,
                RequestPath = "/api/journal",
                RequestMethod = "POST"
            },
            new ExceptionJournal
            {
                Id = 4,
                EventId = 1004,
                CreatedAt = now.AddDays(-3),
                ExceptionType = "System.InvalidOperationException",
                ExceptionMessage = "Cannot delete a node that has children",
                StackTrace = "   at TestAssignment.Infrastructure.Services.TreeService.DeleteNode(Int64 nodeId) in TreeService.cs:line 125",
                QueryString = null,
                RequestBody = null,
                RequestPath = "/api/tree/node/1",
                RequestMethod = "DELETE"
            },
            new ExceptionJournal
            {
                Id = 5,
                EventId = 1005,
                CreatedAt = now.AddDays(-1),
                ExceptionType = "System.UnauthorizedAccessException",
                ExceptionMessage = "Invalid authentication token",
                StackTrace = "   at TestAssignment.Infrastructure.Services.AuthService.ValidateToken(String token) in AuthService.cs:line 67",
                QueryString = null,
                RequestBody = null,
                RequestPath = "/api/partner/login",
                RequestMethod = "POST"
            }
        );
    }
}

