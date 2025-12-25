
namespace BombermanGame.src.Patterns.Behavioral.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        string GetDescription();
    }
}