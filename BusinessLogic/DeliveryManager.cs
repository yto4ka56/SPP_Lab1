namespace BusinessLogic;

public class Order {
    public List<string> Items { get; set; } = new();
    public int Price { get; set; }
    public bool IsExpress { get; set; }
}

public class DeliveryService
{
    public int CalculateDelivery(int amount, double distanceKm) {
        if (distanceKm < 0) throw new ArgumentException("Negative distance");
        if (amount >= 2000) return 0; 
        return 200 + (int)(distanceKm * 50);
    }

    public async Task<bool> ProcessPaymentAsync(int amount) {
        await Task.Delay(50);
        return amount > 0;
    }

    public string GetCourierStatus(int id) => id > 0 ? "Active" : null;
}