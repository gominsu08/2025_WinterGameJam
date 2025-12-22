using UnityEngine;

namespace Work.Characters.Code
{
    public enum CharacterEnum
    {
        LandWizard,
        Soldier,
        BattleMaid,
        Novelist,
        None = -1
    }

    public enum CharacterStateEnum
    {
        Idle,
        Walking,
        Running,
        Attacking,
        Dead,
        None = -1
    }

    public enum AttackTypeEnum
    {
        Melee,
        Magic,
        None = -1
    }

    public enum ChracterAttackRangeTypeEnum
    {
        /// <summary>
        /// 근거리 공격
        /// </summary>
        CloseRange,
        /// <summary>
        /// 중거리 공격
        /// </summary>
        MiddleDistance,
        /// <summary>
        /// 장거리 공격
        /// </summary>
        LongDistance,
        /// <summary>
        /// 중장거리 공격
        /// </summary>
        MidToLongDistance,
        None = -1
    }
}