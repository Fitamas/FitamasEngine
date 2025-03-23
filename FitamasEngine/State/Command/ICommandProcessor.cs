using System;

namespace Fitamas.State.Command
{
    public interface ICommandProcessor
    {
        void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
        bool Process<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
