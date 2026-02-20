using Library;
using BusinessLogic;

namespace Tests;

[MyTestClass]
public class DeliveryTests
{
    private DeliveryService _service;
    private List<string> _tempLog;

    [MyBeforeTest]
    public void Setup() 
    {
        _service = new DeliveryService();
        _tempLog = new List<string>();
    }

    [MyAfterTest]
    public void Teardown() 
    {
        _service = null;
        _tempLog = null;
    }
    
    [MyTest]
    [MyTestCase(3000, 5, 0)]
    [MyTestCase(10000, 2, 300)]
    public void CalculateDelivery_VaryingOrderAmounts(int price, double dist, int exp) {
        MyAssert.AreEqual(exp, _service.CalculateDelivery(price, dist));
    }

    [MyTest]
    public void CalculateDelivery_SmallOrder() {
        MyAssert.AreNotEqual(0m, _service.CalculateDelivery(100, 1));
    }

    [MyTest]
    public async Task ProcessPaymentAsync_PositiveAmount() {
        bool result = await _service.ProcessPaymentAsync(100);
        MyAssert.IsTrue(result);
    }

    [MyTest]
    public async Task ProcessPaymentAsync_NegativeAmount() {
        bool result = await _service.ProcessPaymentAsync(-50);
        MyAssert.IsFalse(result);
    }

    [MyTest]
    public void GetCourierStatus_InvalidId() {
        MyAssert.IsNull(_service.GetCourierStatus(-1));
    }

    [MyTest]
    public void GetCourierStatus_ValidId() {
        MyAssert.IsNotNull(_service.GetCourierStatus(77));
    }

    [MyTest]
    public void OrderItems_WhenAdded() {
        var order = new Order { Items = { "Бургер" } };
        MyAssert.IsNotEmpty(order.Items);
    }

    [MyTest]
    public void OrderItems_SpecificItemAdded() {
        var items = new List<string> { "Пицца", "Суши" };
        MyAssert.Contains("Суши", items);
    }

    [MyTest]
    public void OrderObject_UponCreation() {
        var order = new Order();
        MyAssert.IsInstanceOf<Order>(order);
    }

    [MyTest]
    public void CalculateDelivery_NegativeDistance() {
        MyAssert.Throws<ArgumentException>(() => _service.CalculateDelivery(100, -10));
    }

    [MyTest(Skip = "Фича отмены заказа в разработке")]
    public void CancelOrder() {
        MyAssert.IsTrue(false);
    }
}