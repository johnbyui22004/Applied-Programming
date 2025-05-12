using System;
using System.Collections.Generic;
using System.IO;

public enum RecurrenceType { None, Monthly, Yearly }

public abstract class BudgetItem
{
    private string _name;
    private decimal _amount;
    private bool _isRecurring;
    private DateTime? _firstOccurrence;
    private RecurrenceType _recurrenceType;

    public string Name { get => _name; protected set => _name = value; }
    public decimal Amount { get => _amount; protected set => _amount = value; }
    public bool IsRecurring { get => _isRecurring; protected set => _isRecurring = value; }
    public DateTime? FirstOccurrence { get => _firstOccurrence; protected set => _firstOccurrence = value; }
    public RecurrenceType RecurrenceType { get => _recurrenceType; protected set => _recurrenceType = value; }

    public BudgetItem(string name, decimal amount, bool isRecurring = false, DateTime? firstOccurrence = null, RecurrenceType recurrenceType = RecurrenceType.None)
    {
        Name = name;
        Amount = amount;
        IsRecurring = isRecurring;
        FirstOccurrence = firstOccurrence;
        RecurrenceType = recurrenceType;
    }

    public abstract string GetDescription();
    public abstract string ToFileString();

    public int GetOccurrencesUntil(DateTime untilDate)
    {
        if (!IsRecurring || FirstOccurrence == null || untilDate < FirstOccurrence) return 1;

        TimeSpan span = untilDate - FirstOccurrence.Value;
        int occurrences = 1; // Count the first occurrence

        if (RecurrenceType == RecurrenceType.Monthly)
        {
            occurrences += (int)(span.TotalDays / 30); // Approximate months
        }
        else if (RecurrenceType == RecurrenceType.Yearly)
        {
            occurrences += (int)(span.TotalDays / 365); // Approximate years
        }

        return occurrences;
    }
}