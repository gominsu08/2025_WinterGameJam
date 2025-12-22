using Unity.Cinemachine;
using UnityEngine;

namespace Work.Characters.Code.Test
{
    public class CharacterDataInjecter : MonoBehaviour
    {
        [SerializeField] private CharacterDataSO characterDataSO;
        [SerializeField] private CharacterDataContainer characterDataContainer;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private CinemachineCamera playerFollowCam;

    }
}