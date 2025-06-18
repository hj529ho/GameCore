using System;
using System.Collections.Generic;
using Core.ScenarioSystem;
using UnityEngine;

namespace Core.Manager
{
    // [Manager("Story","Core")]
    public class StoryManager
    {
        private Dictionary<string, Scenario> _scenarios = new();
        private Coroutine _commandCoroutine = null;
        private Command _currentCommand = null;
        private Scenario _scenario = null;
        public Dictionary<string, UI_Story> UIStories = new Dictionary<string, UI_Story>();
        /// <summary>
        /// 이거 성능 구림.
        /// </summary>
        public void Init()
        {
            _scenarios.Clear();
            //  Managers.Localization.LoadAllLocalizedAsset<TextAsset>("StoryDataTable", (dict) =>
            // {
            //     foreach (var VARIABLE in dict)
            //     {
            //         _scenarios.Add(VARIABLE.Key,new Scenario(VARIABLE.Value));
            //     }
            // });
        }
        /// <summary>
        /// 시나리오 시작하는 메서드
        /// </summary>
        /// <param name="sceanarioID"> 시나리오 키값 </param>
        public void StartScenario(string sceanarioID)
        {
            //키 값으로 Dictionary에서 Scenario 인스턴스를 찾아  _scenario 변수에 할당.
            if (_scenarios.TryGetValue(sceanarioID, out _scenario))
            {
                //_scenario의 내부 인덱스 변수를 0으로 바꿔주는 메서드 Reset()을 실행.
                _scenario.Reset();
                //_scenario의 Command를 순서대로 실행시키는 메서드 InvokeNextCommad()실행
                InvokeNextCommand();
                return;
            }
            //딕셔너리에 값이 없으면 로그 출력
            Debug.Log($"ScenarioManager : {sceanarioID} is not found");
        }

        public UI_Story GetOrCreateStoryUI(Type type)
        {
            return GetOrCreateStoryUI(type.Name);
        }

        public UI_Story GetOrCreateStoryUI(string uiName)
        {
            UI_Story uiStory;
            if (UIStories.TryGetValue(uiName, out uiStory))
            {
                return uiStory;
            }
            uiStory = Managers.UI.ShowStoryUI<UI_Story>(uiName);
            UIStories.Add(uiName,uiStory);
            return uiStory;
        }

        /// <summary>
        /// 시나리오 시작하는 메서드
        /// </summary>
        /// <param name="story"> 시나리오 enum </param>
        public void StartScenario(Define.Story story)
        {
            StartScenario($"{story}");
        } 
        public void StartScenario(Scenario scenario)
        {
            _scenario = scenario;
            InvokeNextCommand();
        }
        public bool InvokeNextCommand()
        {
            //코루틴이 실행 중일때(스토리 연출이 실행중인 상태)
            if (_commandCoroutine != null)
            {
                //코루틴 정지
                CoroutineHelper.StopCoroutine(_commandCoroutine);
                _commandCoroutine = null;
                //실행 중이였던 커맨드 실행 취소
                _currentCommand.Revoke();
                //실행 성공했으니 true 반환
                return true;
            }
            //코루틴이 실행중이 아닐때(스토리 연출이 끝난 상태, 정지된 상태)
            //_currentCommand null로 초기화
            _currentCommand = null;
            //_scenario 인스턴스에서 다음 커맨드 가져오기
            _currentCommand = _scenario?.GetCommand();
            if (_currentCommand == null)
                //다음커맨드 가져오기 실패시 false 반환
                return false;
            if (_currentCommand != null)
                //가져오기 성공시 현재 커맨드의 Invoke() 코루틴 실행.
                _commandCoroutine = CoroutineHelper.StartCoroutine(_currentCommand?.Invoke());
            //성공했으니 true 반환
            return true;
        }

    }
}
