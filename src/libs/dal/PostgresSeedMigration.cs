namespace HSB.DAL;

/// <summary>
/// PostgresSeedMigration abstract class, provides seed migration for PostgreSQL.
/// </summary>
public abstract class PostgresSeedMigration : SeedMigration
{
    /// <summary>
    /// Print a message to the SQL output.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected override string PrintMessage(string message)
    {
        return base.PrintMessage($"do $$ begin raise notice '{message}'; end; $$;");
    }
}
