using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SoftOm.Benchmarks.Ef;

public class DatabaseContext(DbContextOptions<DatabaseContext> options)
    : DbContext(options)
{
    public static readonly string ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                                     "Host=localhost;" +
                                                     "Port=5101;" +
                                                     "Database=softom-ef;" +
                                                     "Username=pg-user;" +
                                                     "Password=pg-password;" +
                                                     "Pooling=true;";

    private const int BlogCount = 300;
    private const int PostCount = 20;

    public const int ContentLength = 1024;
    public const int UrlLength = 256;
    public const int TitleLength = 128;

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DatabaseContext()
        : this(new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(ConnectionString)
            .Options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // var random = new Random(123);
        //
        // var blogs = GetBlogs(random).ToArray();
        //
        // modelBuilder
        //     .Entity<Blog>()
        //     .HasData(blogs);
        //
        // var posts = blogs
        //     .SelectMany(x => GetPosts(random, x))
        //     .ToArray();
        //
        // modelBuilder
        //     .Entity<Post>()
        //     .HasData(posts);
    }

    public static async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = new DatabaseContext();
        await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        if (await dbContext.Blogs
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false))
        {
            return;
        }

        var random = new Random(123);

        var blogs = GetBlogs(random).ToArray();

        await dbContext.Blogs
            .AddRangeAsync(blogs, cancellationToken)
            .ConfigureAwait(false);

        var posts = blogs
            .SelectMany(x => GetPosts(random, x))
            .ToArray();

        await dbContext.Posts
            .AddRangeAsync(posts, cancellationToken)
            .ConfigureAwait(false);

        await dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IEnumerable<Blog> GetBlogs(Random random)
    {
        for (var i = 0; i < BlogCount; i++)
        {
            var blog = new Blog
            {
                Id = Guid.CreateVersion7(),
                Rating = random.Next(),
                Url = CreateRandomString(random, UrlLength),
                IsDeleted = false
            };

            yield return blog;
        }
    }

    private static IEnumerable<Post> GetPosts(Random random, Blog blog)
    {
        for (var i = 0; i < PostCount; i++)
        {
            yield return new Post
            {
                Id = Guid.CreateVersion7(),
                Content = CreateRandomString(random, ContentLength),
                Title = CreateRandomString(random, TitleLength),
                IsDeleted = false,
                BlogId = blog.Id
            };
        }
    }

    private static string CreateRandomString(Random random, int length)
    {
        ReadOnlySpan<char> chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Span<char> buffer = stackalloc char[length];

        random.GetItems(chars, buffer);
        return buffer[..length].ToString();
    }
}

// [LinqToDB.Mapping.Table("Blogs")]
public sealed class Blog
{
    // [LinqToDB.Mapping.PrimaryKey, LinqToDB.Mapping.Identity, LinqToDB.Mapping.Column]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    // [LinqToDB.Mapping.Column]
    public required int Rating { get; set; }

    // [LinqToDB.Mapping.Column, MaxLength(256)]
    [MaxLength(DatabaseContext.UrlLength)]
    public required string Url { get; set; }

    // [LinqToDB.Mapping.Column]
    public required bool IsDeleted { get; set; }

    // [LinqToDB.Mapping.Association(ThisKey = nameof(Id), OtherKey = nameof(Post.BlogId))]
    public List<Post> Posts { get; set; } = [];
}

// [LinqToDB.Mapping.Table("Posts")]
public sealed class Post
{
    // [LinqToDB.Mapping.PrimaryKey, LinqToDB.Mapping.Identity, LinqToDB.Mapping.Column]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    // [LinqToDB.Mapping.Column, MaxLength(2048)]
    [MaxLength(DatabaseContext.ContentLength)]
    public required string Content { get; set; }

    // [LinqToDB.Mapping.Column, MaxLength(256)]
    [MaxLength(DatabaseContext.TitleLength)]
    public required string Title { get; set; }

    // [LinqToDB.Mapping.Column]
    public required bool IsDeleted { get; set; }

    // [LinqToDB.Mapping.Column]
    public required Guid BlogId { get; set; }

    // [LinqToDB.Mapping.Association(ThisKey = nameof(BlogId), OtherKey=nameof(Blog.Id))]
    public Blog Blog { get; set; }
}