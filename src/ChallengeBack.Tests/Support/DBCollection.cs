namespace ChallengeBack.Tests.Support;

[CollectionDefinition("db", DisableParallelization = true)]
public sealed class DbCollection : ICollectionFixture<PostgresFixture> { }