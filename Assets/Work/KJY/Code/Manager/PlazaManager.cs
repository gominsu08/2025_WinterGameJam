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
        
        private int _curLevel = 1;

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
            Bus<PlazaLevelUpgradedEvent>.Raise(new PlazaLevelUpgradedEvent(_curLevel));
        }
    }
}