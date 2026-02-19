using Library;
using BusinessLogic;

namespace Tests;

[MyTestClass] 
public class ServiceTests
{
    private BankAccount _account;

    [MyBeforeTest] 
    public void Setup()
    {
        _account = new BankAccount { Owner = "Алексей" };
        _account.Deposit(1000); 
    }

    [MyAfterTest] 
    public void Teardown()
    {
        _account = null;
    }

    [MyTest] // Тест 1: AreEqual и IsNotNull
    public void TestInitialDeposit()
    {
        MyAssert.IsNotNull(_account);
        MyAssert.AreEqual(1000, _account.Balance);
    }

    [MyTest] // Тест 2: AreNotEqual и IsNotEmpty
    public void TestTransactionHistory()
    {
        _account.Deposit(500);
        MyAssert.AreNotEqual(1000, _account.Balance);
        MyAssert.IsNotEmpty(_account.History); // Коллекция не пуста
    }

    [MyTest] // Тест 3: IsTrue и IsFalse
    public void TestAccountBlocking()
    {
        MyAssert.IsFalse(_account.IsBlocked);
        _account.IsBlocked = true;
        MyAssert.IsTrue(_account.IsBlocked);
    }

    [MyTest] // Тест 4: Contains и IsInstanceOf
    public void TestTransactionDetails()
    {
        _account.Deposit(100);
        var lastTransaction = _account.History[0];
        
        MyAssert.Contains(lastTransaction, _account.History);
        MyAssert.IsInstanceOf<Transaction>(lastTransaction);
    }

    [MyTest] // Тест 5: IsNull
    public void TestOwnerName()
    {
        BankAccount emptyAccount = new BankAccount();
        MyAssert.IsNull(emptyAccount.Owner); 
    }

    [MyTest] // Тест 6: Throws 
    public void TestInvalidDeposit_ShouldThrow()
    {
        MyAssert.Throws<ArgumentException>(() => _account.Deposit(-100));
    }

    [MyTest] // Тест 7: StringContains
    public void TestOwnerNaming()
    {
        MyAssert.StringContains("Алекс", _account.Owner);
    }

    [MyTest] // Тест 8: Асинхронный метод
    public async Task TestWithdrawal_Async()
    {
        bool result = await _account.WithdrawAsync(500);
        MyAssert.IsTrue(result);
        MyAssert.AreEqual(500, _account.Balance);
    }

    [MyTest] // Тест 9: Атрибут с параметрами
    [MyTestCase(100, 900)]
    [MyTestCase(500, 500)]
    [MyTestCase(1000, 0)]
    public async Task TestMultipleWithdrawals(int amount, int expectedBalance)
    {
        await _account.WithdrawAsync(amount);
        MyAssert.AreEqual(expectedBalance, _account.Balance);
    }

    [MyTest(Skip = "Этот тест временно отключен, так как логика кредитования еще не написана")] 
    // Тест 10: Использование свойства Skip
    public void TestCreditLimit()
    {
        MyAssert.IsTrue(false); 
    }
}