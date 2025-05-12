public class BudgetManager
{
    private List<BudgetItem> _items;
    private decimal _savingsGoal;
    private const string FILE_PATH = "budget.txt";

    public BudgetManager()
    {
        _items = new List<BudgetItem>();
        _savingsGoal = 0m;
        LoadFromFile();
    }

    public void SetSavingsGoal(decimal goal)
    {
        _savingsGoal = goal;
        SaveToFile();
    }

    public void AddItem(BudgetItem item)
    {
        _items.Add(item);
        SaveToFile();
    }

    public decimal CalculateTotalIncome(DateTime untilDate)
    {
        decimal total = 0;
        foreach (var item in _items)
        {
            if (item is Income)
            {
                total += item.Amount * item.GetOccurrencesUntil(untilDate);
            }
        }
        return total;
    }

    public decimal CalculateTotalExpenses(DateTime untilDate)
    {
        decimal total = 0;
        foreach (var item in _items)
        {
            if (item is Expense)
            {
                total += item.Amount * item.GetOccurrencesUntil(untilDate);
            }
        }
        return total;
    }

    private void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(FILE_PATH))
        {
            writer.WriteLine(_savingsGoal);
            foreach (var item in _items)
            {
                writer.WriteLine(item.ToFileString());
            }
        }
    }

    private void LoadFromFile()
    {
        if (!File.Exists(FILE_PATH)) return;

        using (StreamReader reader = new StreamReader(FILE_PATH))
        {
            string line;
            bool firstLine = true;
            
            while ((line = reader.ReadLine()) != null)
            {
                if (firstLine)
                {
                    _savingsGoal = decimal.Parse(line);
                    firstLine = false;
                    continue;
                }

                string[] parts = line.Split('|');
                bool isRecurring = bool.Parse(parts[4]);
                DateTime? firstOccurrence = parts[5] == "null" ? null : DateTime.Parse(parts[5]);
                RecurrenceType recurrenceType = (RecurrenceType)int.Parse(parts[6]);

                if (parts[0] == "Income")
                {
                    _items.Add(new Income(parts[1], decimal.Parse(parts[2]), parts[3], isRecurring, firstOccurrence, recurrenceType));
                }
                else if (parts[0] == "Expense")
                {
                    _items.Add(new Expense(parts[1], decimal.Parse(parts[2]), parts[3], isRecurring, firstOccurrence, recurrenceType));
                }
            }
        }
    }

    public void ResetBudget()
    {
        _items.Clear();
        _savingsGoal = 0m;
        SaveToFile();
        Console.WriteLine("Budget has been reset. All data cleared and savings goal set to $0.");
    }

    public void DisplayBudgetSummary()
    {
        DateTime currentDate = DateTime.Now;
        Console.WriteLine($"\n=== Budget Summary as of {currentDate.ToShortDateString()} ===");
        foreach (var item in _items)
        {
            int occurrences = item.GetOccurrencesUntil(currentDate);
            Console.WriteLine($"{item.GetDescription()} - Total Occurrences: {occurrences}");
        }

        decimal income = CalculateTotalIncome(currentDate);
        decimal expenses = CalculateTotalExpenses(currentDate);
        decimal balance = income - expenses;

        Console.WriteLine($"\nTotal Income: ${income}");
        Console.WriteLine($"Total Expenses: ${expenses}");
        Console.WriteLine($"Current Balance: ${balance}");
        Console.WriteLine($"Savings Goal: ${_savingsGoal}");
        
        if (balance >= _savingsGoal)
        {
            Console.WriteLine("Congratulations! You've met your savings goal!");
        }
        else
        {
            Console.WriteLine($"You need ${(_savingsGoal - balance)} more to reach your savings goal.");
        }
    }

    public void ProvideBudgetAdvice()
    {
        DateTime currentDate = DateTime.Now;
        decimal income = CalculateTotalIncome(currentDate);
        decimal expenses = CalculateTotalExpenses(currentDate);
        decimal balance = income - expenses;

        Console.WriteLine("\n=== Budget Advice ===");
        
        if (income == 0)
        {
            Console.WriteLine("You haven't added any income yet. Consider finding a source of income to start building your budget.");
            return;
        }

        decimal expensePercentage = (expenses / income) * 100;
        
        if (expensePercentage > 80)
        {
            Console.WriteLine("Warning: You're spending more than 80% of your income!");
            Console.WriteLine("Try to reduce your expenses or increase your income to save more.");
        }
        else if (expensePercentage > 50)
        {
            Console.WriteLine("You're spending over half your income.");
            Console.WriteLine("Consider reviewing your expenses to see where you can cut back.");
        }
        else
        {
            Console.WriteLine("Good job! You're keeping your expenses well below your income.");
        }

        if (balance < 0)
        {
            Console.WriteLine("You're in the red! Your expenses exceed your income.");
            Console.WriteLine("Immediate action is recommended to avoid debt.");
        }
        else if (balance < _savingsGoal / 2)
        {
            Console.WriteLine("You're making progress, but you're less than halfway to your savings goal.");
            Console.WriteLine("Try to save more each month or adjust your goal if needed.");
        }
        else
        {
            Console.WriteLine("You're on track with your savings!");
            Console.WriteLine("Keep up the good work to reach or exceed your goal.");
        }
    }
}