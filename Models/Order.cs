namespace KodaStore.Api.Models;

public sealed class Order
{
    private Order() { }

    public long OrderId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTimeOffset OrderDate { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    public Order(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber) || orderNumber.Trim().Length > 35)
        {
            throw new ArgumentException("OrderNumber wajib diisi (Max 35 Karakter).", nameof(orderNumber));
        }
        OrderNumber = orderNumber.Trim().ToUpperInvariant();
        OrderDate = DateTimeOffset.UtcNow.AddHours(7);
        Status = "Pending";
    }

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (Status != "Pending") throw new InvalidOperationException("Order tidak bisa diubah setelah dikonfirmasi");
        if (Items.Any(i => i.ProductCode == product.ProductCode)) throw new ArgumentException("Product Code tidak boleh berulang dalam satu order");

        Items.Add(new OrderItem(product, quantity));
    }
}