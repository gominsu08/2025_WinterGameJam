using System.Collections.Generic;
using UnityEngine;
using Work.Characters;
using Work.GMS.Code.Characters.Code.Test;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.GMS.Code.UIs.BoostUIs
{
    public class BoostUIContainer : MonoBehaviour
    {
        [SerializeField] private GameObject boostIconPrefab;
        [SerializeField] private CharacterDataContainer characterDataContainer;

        private Dictionary<BoostType, BoostIcon> _boostIconIstance = new Dictionary<BoostType, BoostIcon>();

        [SerializeField] private Sprite speed, glow, sield;

        private CharacterBoostCompo _boostCompo;

        public void Awake()
        {
            Bus<GetBoostItemEvent>.Events += HandleBoostItemEevent;
        }

        private void OnDestroy()
        {
            Bus<GetBoostItemEvent>.Events -= HandleBoostItemEevent;
        }

        private void Start()
        {
            _boostCompo = characterDataContainer.CurrentCharacter.GetCompo<CharacterBoostCompo>();

            _boostCompo.OnChangeValueEvent += () =>
            {
                if(!_boostCompo.IsSprintBoost)
                {
                    if(_boostIconIstance.ContainsKey(BoostType.SpeedBoost))
                    {
                        Destroy(_boostIconIstance[BoostType.SpeedBoost].gameObject);
                        _boostIconIstance.Remove(BoostType.SpeedBoost);
                    }
                }
                if(!_boostCompo.IsSnowBoost)
                {
                    if(_boostIconIstance.ContainsKey(BoostType.SnowBoost))
                    {
                        Destroy(_boostIconIstance[BoostType.SnowBoost].gameObject);
                        _boostIconIstance.Remove(BoostType.SnowBoost);
                    }
                }
                if(!_boostCompo.IsShield)
                {
                    if(_boostIconIstance.ContainsKey(BoostType.SnowShield))
                    {
                        Destroy(_boostIconIstance[BoostType.SnowShield].gameObject);
                        _boostIconIstance.Remove(BoostType.SnowShield);
                    }
                }
            };
        }

        private void HandleBoostItemEevent(GetBoostItemEvent evt)
        {
            switch (evt.Type)
            {
                case BoostType.SpeedBoost:
                    {
                        if(!_boostIconIstance.ContainsKey(BoostType.SpeedBoost))
                        {
                            BoostIcon icon = IconCreate(speed);
                            icon.SetColor(Color.cyan);
                            _boostIconIstance.Add(BoostType.SpeedBoost, icon);
                        }
                        
                    }
                    break;
                case BoostType.SnowBoost:
                    {
                        if (!_boostIconIstance.ContainsKey(BoostType.SnowBoost))
                        {
                            BoostIcon icon = IconCreate(glow);
                            icon.SetColor(Color.white);
                            _boostIconIstance.Add(BoostType.SnowBoost, icon);
                        }
                    }
                    break;
                case BoostType.SnowShield:
                    {
                        if (!_boostIconIstance.ContainsKey(BoostType.SnowShield))
                        {
                            BoostIcon icon = IconCreate(sield);
                            icon.SetColor(Color.yellow);
                            _boostIconIstance.Add(BoostType.SnowShield, icon);
                        }
                    }
                    break;
                case BoostType.Other:
                    {

                    }
                    break;
            }
        }
        
        public BoostIcon IconCreate(Sprite str)
        {
            BoostIcon icon = Instantiate(boostIconPrefab, transform).GetComponent<BoostIcon>();
            icon.SetIcon(str);
            return icon;
        }

        public void Update()
        {

        }
    }
}