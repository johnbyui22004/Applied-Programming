public class Income : BudgetItem
{
    private string _source;

    public Income(string name, decimal amount, string source, bool isRecurring = false, DateTime? firstOccurrence = null, RecurrenceType recurrenceType = RecurrenceType.None) 
        : base(name, amount, isRecurring, firstOccurrence, recurrenceType)
    {
        _source = source;
    }

    public override string GetDescription()
    {
        string recurrence = IsRecurring ? $" (Recurring {RecurrenceType}, First: {FirstOccurrence?.ToShortDateString()})" : "";
        return $"Income - {Name}: ${Amount} from {_source}{recurrence}";
    }

    public override string ToFileString()
    {
        return $"Income|{Name}|{Amount}|{_source}|{IsRecurring}|{FirstOccurrence?.ToString("yyyy-MM-dd") ?? "null"}|{(int)RecurrenceType}";
    }
}