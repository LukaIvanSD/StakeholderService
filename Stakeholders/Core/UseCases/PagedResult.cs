namespace Stakeholders.Core.UseCases;

public class PagedResult<T>
{
    public List<T> Results { get; }
    public int TotalCount { get; }
    public int RemainingCount { get; }

    public PagedResult(List<T> items, int totalCount, int remainingCount)
    {
        Results = items;
        TotalCount = totalCount;
        RemainingCount = remainingCount;
    }
}