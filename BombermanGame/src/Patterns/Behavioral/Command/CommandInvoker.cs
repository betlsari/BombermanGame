
using System.Collections.Generic;

namespace BombermanGame.src.Patterns.Behavioral.Command
{
    public class CommandInvoker
    {
        private Stack<ICommand> _commandHistory = new Stack<ICommand>();
        private int _maxHistorySize = 10;

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();

            _commandHistory.Push(command);

            // Tarihçe boyutunu sınırla
            if (_commandHistory.Count > _maxHistorySize)
            {
                var tempStack = new Stack<ICommand>();
                for (int i = 0; i < _maxHistorySize; i++)
                {
                    tempStack.Push(_commandHistory.Pop());
                }
                _commandHistory = tempStack;
            }
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                var command = _commandHistory.Pop();
                command.Undo();
            }
        }

        public void ClearHistory()
        {
            _commandHistory.Clear();
        }

        public int GetHistorySize()
        {
            return _commandHistory.Count;
        }
    }
}