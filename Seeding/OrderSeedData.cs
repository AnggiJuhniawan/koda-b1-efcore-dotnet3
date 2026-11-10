using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public static class OrderSeedData
{
    private const string DemoOrderNumber = "ORD-001";

    public static void Seed(DbContext context)
    {
        if (context.Set<Order>().Any(o => o.OrderNumber == DemoOrderNumber)) return;

        var keyboard = context.Set<Product>().Single(p => p.ProductCode == "P001");
        var mouse = context.Set<Product>().Single(p => p.ProductCode == "P002");

        var order = new Order(DemoOrderNumber);
        order.AddItem(keyboard, 2);
        order.AddItem(mouse, 1);

        context.Set<Order>().Add(order);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct)
    {
        if (await context.Set<Order>().AnyAsync(o => o.OrderNumber == DemoOrderNumber, ct)) return;

        var keyboard = await context.Set<Product>().SingleAsync(p => p.ProductCode == "P002", ct);
        var mouse = await context.Set<Product>().SingleAsync(p => p.ProductCode == "P003", ct);

        var order = new Order(DemoOrderNumber);
        order.AddItem(keyboard, 2);
        order.AddItem(mouse, 1);

        context.Set<Order>().Add(order);
        await context.SaveChangesAsync(ct);
    }
}