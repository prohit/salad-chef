
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] PickableType type;
    public PickableType Type { get { return type; } }
    public Carrier Carrier { get; set; }
}
