namespace NHibernateCourse.Demo7.Infrastructure
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TResult> : ICommandHandler
    {
        TResult Handle(TCommand command);
    }
}