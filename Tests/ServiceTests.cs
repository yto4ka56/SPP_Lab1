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
        _tempLog.Clear();
        _tempLog = null;
    }
    
    [MyTest] // 1. AreEqual
    [MyTestCase(3000, 5, 0)] // Бесплатная доставка
    public void Test_AreEqual(int price, double dist, int exp) {
        MyAssert.AreEqual(exp, _service.CalculateDelivery(price, dist));
    }

    [MyTest] // 2. AreNotEqual
    public void Test_AreNotEqual() {
        MyAssert.AreNotEqual(500m, _service.CalculateDelivery(100, 1));
    }

    [MyTest] // 3. IsTrue
    public async Task Test_IsTrue() {
        bool result = await _service.ProcessPaymentAsync(100);
        MyAssert.IsTrue(result);
    }

    [MyTest] // 4. IsFalse
    public async Task Test_IsFalse() {
        bool result = await _service.ProcessPaymentAsync(-50);
        MyAssert.IsFalse(result);
    }

    [MyTest] // 5. IsNull
    public void Test_IsNull() {
        MyAssert.IsNull(_service.GetCourierStatus(-1));
    }

    [MyTest] // 6. IsNotNull
    public void Test_IsNotNull() {
        MyAssert.IsNotNull(_service.GetCourierStatus(77));
    }

    [MyTest] // 7. IsNotEmpty
    public void Test_IsNotEmpty() {
        var order = new Order { Items = { "Бургер" } };
        MyAssert.IsNotEmpty(order.Items);
    }

    [MyTest] // 8. Contains
    public void Test_Contains() {
        var items = new List<string> { "Пицца", "Суши" };
        MyAssert.Contains("Суши", items);
    }

    [MyTest] // 9. IsInstanceOf
    public void Test_IsInstanceOf() {
        var order = new Order();
        MyAssert.IsInstanceOf<Order>(order);
    }

    [MyTest] // 10. Throws
    public void Test_Throws() {
        MyAssert.Throws<ArgumentException>(() => _service.CalculateDelivery(100, -10));
    }

    [MyTest(Skip = "Тест на проверку пропуска")]
    public void Test_Skipped() {
        MyAssert.IsTrue(false);
    }
}