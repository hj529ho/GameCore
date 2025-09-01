using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Core.ScenarioSystem
{
    public abstract class Command
    {
        public abstract UniTask Invoke();

        public virtual void Revoke()
        {
            return;
        }

        public static LinkedCommand operator +(Command a, Command b)
        {
            List<Command> commands = new List<Command>();
            commands.Add(a);
            commands.Add(b);
            return new LinkedCommand(commands);
        }
    }

    public class LayeredCommand : Command
    {
        public readonly List<Command> commands;
        
        public LayeredCommand(List<Command> commands)
        {
            this.commands = commands;
        }
        public override async UniTask Invoke()
        {
            await UniTask.WhenAll(commands.Select(action => action.Invoke()));
        }
    }

    public class LinkedCommand :Command
    {
        public readonly List<Command> commands;
        public LinkedCommand(List<Command> commands)
        {
            this.commands = commands;
        }
        public override async UniTask Invoke()
        {
            foreach (var command in commands)
            {
                await command.Invoke();
            }
        }
        public override void Revoke()
        {
            foreach (var command in commands)
            {
                command.Revoke();
            }
        }
    }
}