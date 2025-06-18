using System.Collections;
using System.Collections.Generic;

namespace Core.ScenarioSystem
{
    public class Command
    {
        public virtual IEnumerator Invoke()
        {
            yield return null;
        }

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
        public static LinkedCommand operator +(LinkedCommand a, Command b)
        {
            a.Commands.Add(b);
            return new LinkedCommand(a.Commands);
        }
        public static LinkedCommand operator +(Command a, LinkedCommand b)
        {
            List<Command> commands = new List<Command>();
            commands.Add(a);
            commands.AddRange(b.Commands);
            return new LinkedCommand(commands);
        }
    }

    public class LinkedCommand :Command
    {
        public List<Command> Commands;
        public LinkedCommand(List<Command> commands)
        {
            this.Commands = commands;
        }
        public override IEnumerator Invoke()
        {
            foreach (var command in Commands)
            {
                yield return command.Invoke();
            }
        }
        public override void Revoke()
        {
            foreach (var command in Commands)
            {
                command.Revoke();
            }
        }
    }
}