using Unity.Jobs;

namespace HaMa.Scripts._Core.Utils
{
    public interface ICustomJob
    {
        public void Execute();
    }

    public class JobToken
    {
        private readonly int _id;
        public int Id => _id;
        private readonly float _cooldown;
        public float Cooldown => _cooldown;
        public float Time = 0;
        public JobToken(int id, float cooldown)
        {
            _id = id;
            _cooldown = cooldown;
            Time = 0;
        }
    }


    public class asdf : ICustomJob
    {
        public void Execute()
        {

        }
    }
}