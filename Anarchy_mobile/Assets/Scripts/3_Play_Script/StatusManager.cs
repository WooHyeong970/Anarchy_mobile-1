using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Status
{
    public class StatusManager
    {
        // 유닛 생성 당시 능력치 부여
        // 버프 추가시 해당 유닛 전부 능력치 개선
        // 공격력 방어력 행동력 코스
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setCreatedUnitStatus(GameObject unit)
        {
            unit.gameObject.layer = CentralProcessor.Instance.player.GetLayer();

        }
    }
}