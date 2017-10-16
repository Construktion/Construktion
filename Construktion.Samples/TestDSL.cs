using System;
using Construktion.Samples.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Construktion.Samples
{
    public static class TestDSL
    {
        //In an actual app I would use a real database and wipe it clean before each test case.
        //It's not meant to mimic a Relational Db and behaves differently.
        private static readonly DbContextOptions<LeagueContext> options =
            new DbContextOptionsBuilder<LeagueContext>().UseInMemoryDatabase("InMemoryDatabase")
                .Options;

        public static void Insert(params Entity[] entities)
        {
            using (var context = new LeagueContext(options))
            {
                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                        context.Add(entity);
                }

                context.SaveChanges();
            }
        }

        public static void Update(params Entity[] entities)
        {
            using (var context = new LeagueContext(options))
            {
                foreach (var entity in entities)
                {
                    context.Update(entity);
                }

                context.SaveChanges();
            }
        }

        public static TEntity Query<TEntity>(Func<LeagueContext, TEntity> query)
        {
            using (var context = new LeagueContext(options))
            {
                return query(context);
            }
        }

        public static T Construct<T>()
        {
            return ConstruktionData.Construktion.Construct<T>();
        }
    }
}