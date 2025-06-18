using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Core.ScenarioSystem
{
    public class Scenario
    {
        private List<Command> commands = new List<Command>();
        private int _index = 0;

        public Command GetCommand()
        {
            if (_index < commands.Count)
            {
                Command command = commands[_index];
                _index++;
                return command;
            }
            return null;
        }

        public void Reset()
        {
            _index = 0;
        }

        public void JumpTo(int idx)
        {
            _index += idx;
        }
        
        public Scenario(TextAsset asset)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(asset.text);
            //루트 설정
            XmlElement nodes = xmlDocument["Scenario"];
       
            //루트에서 요소 꺼내기
            foreach (XmlElement node in nodes.ChildNodes)
            {
                //빈 커맨드 생성
                Command command = new Command();
                //아래 foreach 문에서 생성된 모든 커맨드는 한꺼번에 실행됨.
                foreach (XmlElement element in node.ChildNodes)
                {
                    switch (element.Name)
                    {
                        case "Dialog":
                            // command += new Dialog(element.GetAttribute("speaker"),element.GetAttribute("content"));
                            break;
                        case "Test":
                            // command += new TestCommand();
                            break;
                        //TODO *커맨드 추가될 때 여기에 추가
                    }
                }
                commands.Add(command);
            }
        }
    }
}
