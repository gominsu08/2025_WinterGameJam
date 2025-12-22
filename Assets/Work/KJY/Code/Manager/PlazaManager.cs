using AYellowpaper.SerializedCollections;
using UnityEngine;
using Work.KJY.Code.Core;

namespace Work.KJY.Code.Manager
{
    public class PlazaManager : MonoSingleton<PlazaManager>
    {
        [SerializedDictionary("level", "Need Money")]
        public SerializedDictionary<int, int> levelDict = new();
        
        private int _curLevel;

        public int GetCurLevel() => _curLevel;
        public int GetNeedMoney() => levelDict[_curLevel];

        public void LevelUp()
        {
            _curLevel++;
        }
    }
}