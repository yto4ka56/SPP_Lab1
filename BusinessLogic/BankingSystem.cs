namespace BusinessLogic;

public class Transaction {
    public int Amount { get; set; }
    public DateTime Timestamp { get; set; }
}

public class BankAccount {
    public string Owner { get; set; }
    public int Balance { get; private set; }
    public List<Transaction> History { get; } = new();
    public bool IsBlocked { get; set; }

    public void Deposit(int amount) {
        if (amount <= 0) throw new ArgumentException("Сумма должна быть положительным числом");
        Balance += amount;
        History.Add(new Transaction { Amount = amount, Timestamp = DateTime.Now });
    }

    public async Task<bool> WithdrawAsync(int amount) {
        await Task.Delay(50);
        if (IsBlocked || Balance < amount) return false;
        Balance -= amount;
        History.Add(new Transaction { Amount = -amount, Timestamp = DateTime.Now });
        return true;
    }
}