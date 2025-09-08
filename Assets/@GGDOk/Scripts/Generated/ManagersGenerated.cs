// 자동 생성된 코드 - 수동으로 수정하지 마세요
using System;
using Cysharp.Threading.Tasks;
namespace Core.Manager
{

    public partial class Managers
    {
        #region Content
        private readonly GGDOk.Scripts.Content.Manager.GameManager _game = new();
        public static GGDOk.Scripts.Content.Manager.GameManager Game => Instance._game;

        #endregion

        private static async UniTask InitAsyncContents()
        {
            await Game.InitAsync();
        }
        private static void InitContents()
        {
            Game.Init();
        }
    }
}
