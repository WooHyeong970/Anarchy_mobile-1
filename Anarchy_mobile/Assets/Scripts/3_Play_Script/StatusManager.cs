using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Status
{
    public class StatusManager : MonoBehaviour
    {
        // 유닛 생성 당시 능력치 부s여
        // 버프 추가시 해당 유닛 전부 능력치 개선
        // 공격력 방어력 행동력 코스
        // Start is called before the first frame update

        //CentralProcessor CP = CentralProcessor.Instance;
        //Player player = CentralProcessor.Instance.GetPlayer();

        public enum UNIT
        {
            COST,
            HP,
            ACTIVE_COST,
            OFFENSIVE,
            DEFENSIVE,
        }

        public enum RANK
        {
            WAR,
            ARC,
            HERO,
        }

        public void SetCreatedUnitInfo(MyUnit unit, int num)
        {
            unit.gameObject.layer = CentralProcessor.Instance.GetPlayer().GetLayer();
            unit.GetComponent<MyUnit>().currentTile = CentralProcessor.Instance.GetPlayer().GetCoreTile();
            unit.GetComponent<MyUnit>().areaPosNumber = num;

            int type = unit.GetUnitType() - 1;
            unit.cost = GetStatus(type, (int)UNIT.COST);
            unit.hp = GetStatus(type, (int)UNIT.HP);
            unit.activeCost = GetStatus(type, (int)UNIT.ACTIVE_COST);
            unit.offensive = GetStatus(type, (int)UNIT.OFFENSIVE);
            unit.defensive = GetStatus(type, (int)UNIT.DEFENSIVE);
        }

        public void SetStatus(int row, int col, int num)
        {
            CentralProcessor.Instance.SetStatus(row, col, num);
        }

        public int GetStatus(int row, int col)
        {
            return CentralProcessor.Instance.GetUnitStatus(row, col);
        }
    }
}