using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SoftOm.Benchmarks.Ef;

[SimpleJob(RuntimeMoniker.Net90)]
[Orderer(SummaryOrderPolicy.SlowestToFastest)]
[MeanColumn, MemoryDiagnoser]
public class EfBenchmark
{
    private NpgsqlDataSource _dapper;

    [Params(5, 50, 100, 300)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _dapper = NpgsqlDataSource.Create(DatabaseContext.ConnectionString);
    }

    [GlobalCleanup]
    public void Dispose()
    {
        _dapper.Dispose();
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_Include_NoTracking_ToArray()
    {
        using var context = GetDatabaseContext();

        return context.Blogs
            .Take(N)
            .Include(x => x.Posts)
            .AsNoTracking()
            .ToArray();
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_Include_Tracking_ToList()
    {
        using var context = GetDatabaseContext();

        return context.Blogs
            .Take(N)
            .Include(x => x.Posts)
            .ToList();
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_Load_Tracking_ToList()
    {
        using var context = GetDatabaseContext();

        var blogs = context.Blogs
            .Take(N)
            .ToList();

        context.Posts
            .Where(x => blogs.Select(y => y.Id).Contains(x.BlogId))
            .Load();

        return blogs;
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_LoadAsList_Tracking_ToList()
    {
        using var context = GetDatabaseContext();

        var blogs = context.Blogs
            .Take(N)
            .ToList();

        var blogIds = blogs.Select(x => x.Id).ToArray();

        _ = context.Posts
            .Where(x => blogIds.Contains(x.BlogId))
            .ToList();

        return blogs;
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_Load_NoTracking_ToDict()
    {
        using var context = GetDatabaseContext();

        var blogs = context.Blogs
            .Take(N)
            .AsNoTracking()
            .ToDictionary(x => x.Id, x => x);

        foreach (var post in context.Posts
                     .Where(x => blogs.Keys.Contains(x.BlogId))
                     .AsNoTracking()
                     .AsEnumerable())
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_FromSql_Tracking_ToList()
    {
        using var context = GetDatabaseContext();

        var blogs = context.Blogs
            .FromSql($"""
                      SELECT
                          *
                      FROM "Blogs" AS b 
                      ORDER BY b."Id"
                      LIMIT {N}
                      """)
            .ToList();

        context.Posts
            .FromSql($"""
                      SELECT
                          * 
                      FROM "Posts" AS p
                      WHERE p."BlogId" = ANY ({blogs.Select(x => x.Id)})
                      ORDER BY p."Id"
                      """)
            .Load();

        return blogs;
    }

    [Benchmark]
    public IReadOnlyList<Blog> Ef_FromSql_NoTracking_ToList()
    {
        using var context = GetDatabaseContext();

        var blogs = context.Blogs
            .FromSql($"""
                      SELECT
                          *
                      FROM "Blogs" AS b 
                      ORDER BY b."Id"
                      LIMIT {N}
                      """)
            .AsNoTracking()
            .ToDictionary(x => x.Id, x => x);

        foreach (var post in context.Posts
                     .FromSql($"""
                               SELECT
                                   * 
                               FROM "Posts" AS p
                               WHERE p."BlogId" = ANY ({blogs.Keys})
                               ORDER BY p."Id"
                               """)
                     .AsNoTracking()
                     .AsEnumerable())
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    [Benchmark(Baseline = true)]
    public Blog[] Dapper_TwoQuery()
    {
        using var connection = _dapper.OpenConnection();

        var blogs = connection
            .Query<Blog>("""
                         SELECT
                             *
                         FROM "Blogs" AS b 
                         ORDER BY b."Id"
                         LIMIT @n
                         """, new { n = N })
            .ToDictionary(x => x.Id, x => x);

        foreach (var post in connection
                     .Query<Post>("""
                                  SELECT
                                      * 
                                  FROM "Posts" AS p
                                  WHERE p."BlogId" = ANY (@ids)
                                  ORDER BY p."Id"
                                  """, new { ids = blogs.Keys.ToArray() }))
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    private static DatabaseContext GetDatabaseContext() => new DatabaseContext();
}