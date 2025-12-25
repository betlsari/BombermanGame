
namespace BombermanGame.src.Patterns.Behavioral.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(GameEvent gameEvent);
    }
}