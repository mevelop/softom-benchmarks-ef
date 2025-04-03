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
    private static readonly CancellationToken CancellationToken = CancellationToken.None;
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
    public async Task<IReadOnlyList<Blog>> Ef_Include_NoTracking_ToArray()
    {
        await using var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        return await context.Blogs
            .Take(N)
            .Include(x => x.Posts)
            .AsNoTracking()
            .ToArrayAsync(CancellationToken)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_Include_Tracking_ToList()
    {
        await using var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        return await context.Blogs
            .Take(N)
            .Include(x => x.Posts)
            .ToListAsync(CancellationToken)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_Load_Tracking_ToList()
    {
        await using var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        var blogs = await context.Blogs
            .Take(N)
            .ToListAsync(CancellationToken)
            .ConfigureAwait(false);

        await context.Posts
            .Where(x => blogs.Select(y => y.Id).Contains(x.BlogId))
            .LoadAsync(CancellationToken)
            .ConfigureAwait(false);

        return blogs;
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_LoadAsList_Tracking_ToList()
    {
        await using var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        var blogs = await context.Blogs
            .Take(N)
            .ToListAsync(CancellationToken)
            .ConfigureAwait(false);

        var blogIds = blogs.Select(x => x.Id).ToArray();

        await context.Posts
            .Where(x => blogIds.Contains(x.BlogId))
            .ToListAsync(CancellationToken)
            .ConfigureAwait(false);

        return blogs;
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_Load_NoTracking_ToDict()
    {
        await using var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        var blogs = await context.Blogs
            .Take(N)
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x, CancellationToken)
            .ConfigureAwait(false);

        await foreach (var post in context.Posts
                           .Where(x => blogs.Keys.Contains(x.BlogId))
                           .AsNoTracking()
                           .AsAsyncEnumerable()
                           .WithCancellation(CancellationToken)
                           .ConfigureAwait(false))
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_FromSql_Tracking_ToList()
    {
        var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        var blogs = await context.Blogs
            .FromSql($"""
                      SELECT
                          *
                      FROM "Blogs" AS b 
                      ORDER BY b."Id"
                      LIMIT {N}
                      """)
            .ToListAsync(CancellationToken)
            .ConfigureAwait(false);

        await context.Posts
            .FromSql($"""
                      SELECT
                          * 
                      FROM "Posts" AS p
                      WHERE p."BlogId" = ANY ({blogs.Select(x => x.Id)})
                      ORDER BY p."Id"
                      """)
            .LoadAsync(CancellationToken)
            .ConfigureAwait(false);

        return blogs;
    }

    [Benchmark]
    public async Task<IReadOnlyList<Blog>> Ef_FromSql_NoTracking_ToList()
    {
        var context = await GetDatabaseContextAsync(CancellationToken).ConfigureAwait(false);

        var blogs = await context.Blogs
            .FromSql($"""
                      SELECT
                          *
                      FROM "Blogs" AS b 
                      ORDER BY b."Id"
                      LIMIT {N}
                      """)
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x, CancellationToken)
            .ConfigureAwait(false);

        await foreach (var post in context.Posts
                           .FromSql($"""
                                     SELECT
                                         * 
                                     FROM "Posts" AS p
                                     WHERE p."BlogId" = ANY ({blogs.Keys})
                                     ORDER BY p."Id"
                                     """)
                           .AsNoTracking()
                           .AsAsyncEnumerable()
                           .WithCancellation(CancellationToken)
                           .ConfigureAwait(false))
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    [Benchmark(Baseline = true)]
    public async Task<Blog[]> Dapper_TwoQuery()
    {
        await using var connection = await _dapper
            .OpenConnectionAsync(CancellationToken)
            .ConfigureAwait(false);

        var blogs = (await connection
                .QueryAsync<Blog>("""
                                  SELECT
                                      *
                                  FROM "Blogs" AS b 
                                  ORDER BY b."Id"
                                  LIMIT @n
                                  """, new { n = N })
                .ConfigureAwait(false))
            .ToDictionary(x => x.Id, x => x);

        foreach (var post in await connection
                     .QueryAsync<Post>("""
                                       SELECT
                                           * 
                                       FROM "Posts" AS p
                                       WHERE p."BlogId" = ANY (@ids)
                                       ORDER BY p."Id"
                                       """, new { ids = blogs.Keys.ToArray() })
                     .ConfigureAwait(false))
        {
            blogs[post.BlogId].Posts.Add(post);
        }

        return blogs.Values.ToArray();
    }

    private static ValueTask<DatabaseContext> GetDatabaseContextAsync(CancellationToken cancellationToken)
        => ValueTask.FromResult(new DatabaseContext());
}