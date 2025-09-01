using System;
using System.Collections.Generic;
using System.Threading;
using Core.ScenarioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Manager
{
    public class StoryManager : BaseManager
    {
        private readonly Dictionary<string, Scenario> _scenarios = new();
        private Command _currentCommand;
        private Scenario _scenario;

        private CancellationTokenSource _currentCts;
        private UniTask _currentTask;
        private bool _isRunning;

        public Dictionary<string, UI_Story> UIStories { get; } = new();

        public override void Init()
        {
            _scenarios.Clear();
            // 로딩 생략
        }

        public override async UniTask InitAsync()
        {
            await UniTask.CompletedTask;
        }

        public void StartScenario(string sceanarioID)
        {
            if (_scenarios.TryGetValue(sceanarioID, out _scenario))
            {
                _scenario.Reset();
                InvokeNextCommand(); // fire-and-cancel semantics 유지
                return;
            }
            Debug.Log($"ScenarioManager : {sceanarioID} is not found");
        }

        public void StartScenario(Define.Story story) => StartScenario($"{story}");

        public void StartScenario(Scenario scenario)
        {
            _scenario = scenario;
            InvokeNextCommand();
        }

        public UI_Story GetOrCreateStoryUI(Type type) => GetOrCreateStoryUI(type.Name);

        public UI_Story GetOrCreateStoryUI(string uiName)
        {
            if (UIStories.TryGetValue(uiName, out var ui)) return ui;
            ui = Managers.UI.ShowStoryUI<UI_Story>(uiName);
            UIStories.Add(uiName, ui);
            return ui;
        }

        /// <summary>
        /// 현재 커맨드가 실행 중이면 취소하고 반환.
        /// 실행 중이 아니면 다음 커맨드를 가져와 비동기로 실행 시작.
        /// (기존 코루틴 기반의 토글 동작을 UniTask로 동일하게 재현)
        /// </summary>
        public bool InvokeNextCommand()
        {
            // 진행 중이면: 취소 + Revoke
            if (_isRunning)
            {
                TryCancelCurrent();
                _currentCommand?.Revoke();
                return true;
            }

            // 다음 커맨드 가져오기
            _currentCommand = _scenario?.GetCommand();
            if (_currentCommand == null) return false;

            // 실행 시작
            _currentCts = new CancellationTokenSource();
            _currentTask = RunCurrentCommandAsync(_currentCommand, _currentCts.Token);
            _currentTask.Forget(); // fire-and-forget
            return true;
        }

        private void TryCancelCurrent()
        {
            try
            {
                _currentCts?.Cancel();
            }
            catch { /* ignore */ }
            finally
            {
                _currentCts?.Dispose();
                _currentCts = null;
            }
        }

        /// <summary>
        /// 단일 커맨드 실행 루프
        /// </summary>
        private async UniTask RunCurrentCommandAsync(Command cmd, CancellationToken token)
        {
            _isRunning = true;
            try
            {
                // (권장) Command.Invoke(CancellationToken) 이 있다면 그걸 호출
                // await cmd.Invoke(token);

                // (대안) 기존 시그니처가 UniTask Invoke() 라면 외부 취소를 부착
                await cmd.Invoke().AttachExternalCancellation(token);
            }
            catch (OperationCanceledException)
            {
                // 취소 시 정상 흐름
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                _isRunning = false;
                _currentCommand = null;

                _currentCts?.Dispose();
                _currentCts = null;
            }
        }
    }
}