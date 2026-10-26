namespace Core.Dto;

public sealed record ImportResult<T>(
    IReadOnlyList<T> Items,
    IReadOnlyList<string> Errors)
{
    public int Total => Items.Count + Errors.Count;
    public int Accepted => Items.Count;
    public int Rejected => Errors.Count;
    public double ErrorRate => Total == 0 ? 0 : (double)Rejected / Total * 100;
}