using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Work.KJY.Code.Core;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Manager
{
    public class PlazaManager : MonoSingleton<PlazaManager>
    {
        [SerializedDictionary("level", "Need Money")]
        public SerializedDictionary<int, int> levelDict = new();

        [SerializeField] private List<GameObject> level1Objs = new(); 
        [SerializeField] private List<GameObject> level2Objs = new(); 
        [SerializeField] private List<GameObject> level3Objs = new(); 
        
        private int _curLevel = 1;

        private void Start()
        {
            foreach (GameObject obj in level1Objs)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in level2Objs)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in level3Objs)
            {
                obj.SetActive(false);
            }
        }

        public int GetCurLevel() => _curLevel;
        public int GetNeedMoney() => levelDict.ContainsKey(_curLevel) ? levelDict[_curLevel] : -1;
        public bool IsMaxLevel => _curLevel >= levelDict.Count;

        public void LevelUp()
        {
            if (IsMaxLevel)
            {
                return;
            }
            
            _curLevel++;

            if (_curLevel == 2)
            {
                foreach (GameObject obj in level1Objs)
                {
                    obj.SetActive(true);
                }
            }

            if (_curLevel == 3)
            {
                foreach (GameObject obj in level2Objs)
                {
                    obj.SetActive(true);
                }
            }

            if (_curLevel == 4)
            {
                foreach (GameObject obj in level3Objs)
                {
                    obj.SetActive(true);
                }
            }
            
            Bus<PlazaLevelUpgradedEvent>.Raise(new PlazaLevelUpgradedEvent(_curLevel));
        }
    }
}