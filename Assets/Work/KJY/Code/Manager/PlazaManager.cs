using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Work.GMS.Code.Data;
using Work.KJY.Code.Core;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Manager
{
    public class PlazaManager : MonoSingleton<PlazaManager>
    {
        [SerializedDictionary("level", "Need Money")]
        public SerializedDictionary<int, int> levelDict = new();
        
        private int _curLevel = 1;

        private void Start()
        {
            // DataContainer에서 저장된 레벨을 불러옵니다.
            _curLevel = DataContainer.Instance.PlazaLevel;

            // 모든 오브젝트를 비활성화하여 초기 상태를 깨끗하게 설정합니다.
            foreach (GameObject obj in LevelObjectDataManager.Instance.level1Objs) { obj.SetActive(false); }
            foreach (GameObject obj in LevelObjectDataManager.Instance.level2Objs) { obj.SetActive(false); }
            foreach (GameObject obj in LevelObjectDataManager.Instance.level3Objs) { obj.SetActive(false); }

            // 불러온 레벨에 맞춰 오브젝트들을 누적하여 활성화합니다.
            if (_curLevel >= 2)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level1Objs) { obj.SetActive(true); }
            }
            if (_curLevel >= 3)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level2Objs) { obj.SetActive(true); }
            }
            if (_curLevel >= 4)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level3Objs) { obj.SetActive(true); }
            }
        }

        public int GetCurLevel() => _curLevel;
        public int GetNeedMoney() => levelDict.ContainsKey(_curLevel) ? levelDict[_curLevel] : -1;
        public bool IsMaxLevel => _curLevel >= levelDict.Count;

        public int LevelUp()
        {
            if (IsMaxLevel)
            {
                return -1;
            }
            
            _curLevel++;
            // 레벨업 시 새로운 레벨을 DataContainer에 저장합니다.
            DataContainer.Instance.SetPlazaLevel(_curLevel);

            if (_curLevel == 2)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level1Objs)
                {
                    obj.SetActive(true);
                }
            }

            if (_curLevel == 3)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level2Objs)
                {
                    obj.SetActive(true);
                }
            }

            if (_curLevel == 4)
            {
                foreach (GameObject obj in LevelObjectDataManager.Instance.level3Objs)
                {
                    obj.SetActive(true);
                }
            }
            
            Bus<PlazaLevelUpgradedEvent>.Raise(new PlazaLevelUpgradedEvent(_curLevel));
            return _curLevel;
        }
    }
}