using UnityEngine;

public class KitchenStand : MonoBehaviour
{
    [SerializeField] Color[] contactColors;
    MeshRenderer meshRenderer;
    protected KitchenStandState currentState = KitchenStandState.EMPTY;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = contactColors[0];
        Init();
    }

    protected virtual void Init() { }

    public virtual void Execute(Command command, ICarrier carrier = null) { }

    //called when player comes in contact with this stand
    public void SetPlayerContactEnable(bool enable)
    {
        meshRenderer.material.color = (enable) ? contactColors[1] : contactColors[0];
    }
}
