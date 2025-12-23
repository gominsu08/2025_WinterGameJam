using UnityEngine;

namespace Work.Entities.Code
{
    public abstract class AbstractEntityDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public float InteractionRange { get; private set; }
    }
}
