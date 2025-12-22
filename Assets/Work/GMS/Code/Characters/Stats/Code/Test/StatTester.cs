using UnityEngine;
using Work.Characters.Code;

namespace Work.Characters.Stats.Code.Test
{
    public class StatTester : MonoBehaviour, IStatUpgrader
    {
        //[SerializeField] private CharacterDataSO characterData;
        //[SerializeField] private string statName = "HP";
        //[SerializeField] private float addValue = 10;
        //[SerializeField] private CharacterDataContainer chareacterDataContainer;

        //private void Start()
        //{
        //    chareacterDataContainer.SetCharacterData(characterData);
        //    chareacterDataContainer.CurrentCharacter.StatContainer.AddListenerValueChangedEvent(Handler, statName);
        //    Debug.Log($"스탯 : {statName}");
        //}

        //[ContextMenu("AddTest")]
        //public void AddTest()
        //{
        //    chareacterDataContainer.CurrentCharacter.StatContainer.AddModifier(statName, this, addValue);
        //}

        //[ContextMenu("RemoveTest")]
        //public void RemoveTest()
        //{
        //    chareacterDataContainer.CurrentCharacter.StatContainer.RemoveModifier(statName, this);
        //}

        //[ContextMenu("GetTest")]
        //public void GetTest()
        //{
        //    Debug.Log(chareacterDataContainer.CurrentCharacter.StatContainer.GetStatValue(statName));
        //}

        //private void Handler(float prev, float change)
        //{
        //    Debug.Log($"이전값 : {prev}");
        //    Debug.Log($"바뀐값 : {change}");
        //}
    }
}