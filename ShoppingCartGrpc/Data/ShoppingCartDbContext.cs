namespace ShoppingCartGrpc.Data;

public class ShoppingCartDbContext : DbContext
{
    public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
    {
        
    }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;

    public override int SaveChanges()
    {
        OnBeforeSaveChanges();

        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaveChanges();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        OnBeforeSaveChanges();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
    {
        OnBeforeSaveChanges();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();

        var records = ChangeTracker.Entries();
        var entityEntries = records.ToList();

        var addedEntries = entityEntries.Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .OfType<Entity>()
            .ToList();

        var updatedEntries = entityEntries.Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity)
            .OfType<Entity>()
            .ToList();

        var now = DateTime.UtcNow;

        addedEntries.ForEach(x =>
        {
            x.CreatedAt = now;
            x.ModifiedAt = now;
        });

        updatedEntries.ForEach(x =>
        {
            x.ModifiedAt = now;
        });
    }
}