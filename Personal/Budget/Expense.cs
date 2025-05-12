public class Expense : BudgetItem
{
    private string _category;

    public Expense(string name, decimal amount, string category, bool isRecurring = false, DateTime? firstOccurrence = null, RecurrenceType recurrenceType = RecurrenceType.None) 
        : base(name, amount, isRecurring, firstOccurrence, recurrenceType)
    {
        _category = category;
    }

    public override string GetDescription()
    {
        string recurrence = IsRecurring ? $" (Recurring {RecurrenceType}, First: {FirstOccurrence?.ToShortDateString()})" : "";
        return $"Expense - {Name}: ${Amount} in category {_category}{recurrence}";
    }

    public override string ToFileString()
    {
        return $"Expense|{Name}|{Amount}|{_category}|{IsRecurring}|{FirstOccurrence?.ToString("yyyy-MM-dd") ?? "null"}|{(int)RecurrenceType}";
    }
}