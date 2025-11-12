//В.1 Приклад програмного коду до рефакторинґу
public class Employee
{
    // Код До: 40 і 1.5 - це "магічні числа"
    public double CalculateOvertime(int hoursWorked)
    {
        if (hoursWorked > 40)
        {
            return (hoursWorked - 40) * 1.5; 
        }
        return 0;
    }
}

// В.2 Приклад програмного коду після рефакторинґу
// --- Після Рефакторингу ---
public class Employee
{
    // Виносимо значення в константи для C#
    private const int STANDARD_WORK_WEEK = 40;
    private const double OVERTIME_RATE = 1.5;

    public double CalculateOvertime(int hoursWorked)
    {
        if (hoursWorked > STANDARD_WORK_WEEK)
        {
           return (hoursWorked - STANDARD_WORK_WEEK) * OVERTIME_RATE; 
        }
        return 0;
    }
 }

//В.3 Приклад програмного коду до рефакторинґу
GitHub репозиторій: 
public bool IsSpecialClearance(User user)
{
    // Код До: Дублювання return true;
    if (user.IsAdmin)
    {
        return true; 
    }
    if (user.Department == "Security")
    {
        return true; 
    }
    if (user.YearsOfService > 20)
    {
        return true; 
    }
    return false;
}

// В.4 Приклад програмного коду після рефакторинґу
// --- Після Рефакторингу ---
public bool IsSpecialClearance(User user)
{
    // Об'єднання умов оператором || (АБО)
    if (user.IsAdmin || 
        user.Department == "Security" || 
        user.YearsOfService > 20)
    {
        return true; 
    }
    return false;
}

//В.5 Приклад програмного коду до рефакторинґу
public class InventoryManager
{
    private int stock = 10;

    // Метод одночасно повертає значення (чи можна відвантажити) ТА зменшує запас (модифікатор)
    public bool TryShipProduct(int quantity)
    {
        if (stock >= quantity)
        {
            stock -= quantity; // Зміна стану
            Console.WriteLine($"Shipped {quantity} items.");
            return true; // Повернення результату
        }
        return false;
    }
}

//В.6 Приклад програмного коду після рефакторинґу
// --- Після Рефакторингу ---
public class InventoryManager
{
    private int stock = 10;

    // 1. Метод-Запит (Query): Тільки повертає значення, НЕ змінює стан
    public bool CanShip(int quantity)
    {
        return stock >= quantity;
    }

    // 2. Метод-Модифікатор (Modifier): Тільки змінює стан (void), викликається після перевірки
    public void ShipProduct(int quantity)
    {
        if (CanShip(quantity))
        {
            stock -= quantity; // Зміна стану
            Console.WriteLine($"Shipped {quantity} items.");
        }
        else
        {
            throw new InvalidOperationException("Not enough stock.");
        }
    }
}
