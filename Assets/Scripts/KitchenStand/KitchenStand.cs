using UnityEngine;

public class KitchenStand : MonoBehaviour
{
    [SerializeField] Color[] contactColors;
    MeshRenderer meshRenderer;
    protected Carrier carrier;
    [SerializeField] protected KitchenStandState currentState; //TODO remove serializefield

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = transform.Find("Tile").GetComponent<MeshRenderer>();
        meshRenderer.material.color = contactColors[0];
        carrier = GetComponent<Carrier>();
        currentState = KitchenStandState.EMPTY;
        Init();
    }

    protected virtual void Init() { }

    public virtual bool Execute(Command command, Carrier playerCarrier = null) { return false; }

    //called when player comes in contact with this stand
    public void SetPlayerContactEnable(bool enable)
    {
        meshRenderer.material.color = (enable) ? contactColors[1] : contactColors[0];
    }
}
