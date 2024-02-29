using Cysharp.Threading.Tasks;

public interface IReadingWaiter
{
    public UniTask WaitReading();
}
