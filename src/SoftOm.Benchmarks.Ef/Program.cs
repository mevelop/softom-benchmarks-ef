using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using SoftOm.Benchmarks.Ef;

await DatabaseContext
    .MigrateAsync(CancellationToken.None)
    .ConfigureAwait(false);

// var N = 10;
//
// await using var context = new DatabaseContext();

// var blogs = context.Blogs
//     .Include(x => x.Posts)
//     .ToQueryString();

// var blogs = await context.Blogs
//     .FromSql($"""
//               SELECT
//                   *
//               FROM "Blogs" AS b 
//               ORDER BY b."Id"
//               LIMIT {N}
//               """)
//     .ToListAsync()
//     .ConfigureAwait(false);

//
// var blogIds = blogs.Select(x => x.Id).ToArray();
//
// var str = context.Posts
//     .Where(x => blogIds.Contains(x.BlogId))
//     .ToQueryString();
//
// await context.Posts
//     .FromSql($"""
//               SELECT
//                   * 
//               FROM "Posts" AS p
//               WHERE p."BlogId" = ANY ({blogIds})
//               ORDER BY p."Id"
//               """)
//     .LoadAsync()
//     .ConfigureAwait(false);
//
// var a = 1;

// BenchmarkRunner.Run<EfBenchmarkAsync>();

BenchmarkRunner.Run<EfBenchmark>();