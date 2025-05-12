class Program
{
    static void Main(string[] args)
    {
        BudgetManager budget = new BudgetManager();
        budget.DisplayBudgetSummary();

        while (true)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1 = Add One-time Income");
            Console.WriteLine("2 = Add One-time Expense");
            Console.WriteLine("3 = Add Recurring Income");
            Console.WriteLine("4 = Add Recurring Expense");
            Console.WriteLine("5 = Set Savings Goal");
            Console.WriteLine("6 = Get Budget Advice");
            Console.WriteLine("7 = Reset Budget");
            Console.WriteLine("0 = Exit");
            string choice = Console.ReadLine();

            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    AddIncome(budget, false);
                    budget.DisplayBudgetSummary();
                    break;

                case "2":
                    AddExpense(budget, false);
                    budget.DisplayBudgetSummary();
                    break;

                case "3":
                    AddIncome(budget, true);
                    budget.DisplayBudgetSummary();
                    break;

                case "4":
                    AddExpense(budget, true);
                    budget.DisplayBudgetSummary();
                    break;

                case "5":
                    Console.Write("Enter new savings goal: $");
                    decimal newGoal = decimal.Parse(Console.ReadLine());
                    budget.SetSavingsGoal(newGoal);
                    budget.DisplayBudgetSummary();
                    break;

                case "6":
                    budget.ProvideBudgetAdvice();
                    break;

                case "7":
                    Console.WriteLine("Are you sure you want to reset the budget? (y/n)");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        budget.ResetBudget();
                        budget.DisplayBudgetSummary();
                    }
                    else
                    {
                        Console.WriteLine("Budget reset cancelled.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddIncome(BudgetManager budget, bool isRecurring)
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Amount: $");
        decimal amount = decimal.Parse(Console.ReadLine());
        Console.Write("Source: ");
        string source = Console.ReadLine();

        DateTime? firstOccurrence = null;
        RecurrenceType recurrenceType = RecurrenceType.None;

        if (isRecurring)
        {
            Console.Write("First occurrence date (yyyy-MM-dd): ");
            firstOccurrence = DateTime.Parse(Console.ReadLine());
            Console.Write("Recurrence type (1 = Monthly, 2 = Yearly): ");
            recurrenceType = Console.ReadLine() == "1" ? RecurrenceType.Monthly : RecurrenceType.Yearly;
        }

        budget.AddItem(new Income(name, amount, source, isRecurring, firstOccurrence, recurrenceType));
    }

    static void AddExpense(BudgetManager budget, bool isRecurring)
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Amount: $");
        decimal amount = decimal.Parse(Console.ReadLine());
        Console.Write("Category: ");
        string category = Console.ReadLine();

        DateTime? firstOccurrence = null;
        RecurrenceType recurrenceType = RecurrenceType.None;

        if (isRecurring)
        {
            Console.Write("First occurrence date (yyyy-MM-dd): ");
            firstOccurrence = DateTime.Parse(Console.ReadLine());
            Console.Write("Recurrence type (1 = Monthly, 2 = Yearly): ");
            recurrenceType = Console.ReadLine() == "1" ? RecurrenceType.Monthly : RecurrenceType.Yearly;
        }

        budget.AddItem(new Expense(name, amount, category, isRecurring, firstOccurrence, recurrenceType));
    }
}