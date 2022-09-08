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

        //CentralProcessor CP = CentralProcessor.Instance;
        //Player player = CentralProcessor.Instance.GetPlayer();

        public void SetCreatedUnitInfo(GameObject unit, int num)
        {
            unit.gameObject.layer = CentralProcessor.Instance.GetPlayer().GetLayer();
            unit.GetComponent<MyUnit>().currentTile = CentralProcessor.Instance.GetPlayer().GetCoreTile();
            unit.GetComponent<MyUnit>().areaPosNumber = num;
        }
    }
}