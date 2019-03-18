using UnityEngine;

public class VegetableItem : PickableItem
{
    [SerializeField] float choppingTime = 3f;
    public float ChoppingTime { get { return choppingTime; } }

    [SerializeField] Sprite icon;
    public Sprite Icon { get { return icon; } }

    public bool IsChopped { get; private set; }
    public VegType VegType;

    Animator animator;
    int choppingHash = Animator.StringToHash("chopping");

    private void Start()
    {
        animator = GetComponent<Animator>();
        IsChopped = false;
    }

    public void SetChopping(bool enable)
    {
        animator.SetBool(choppingHash, enable);
        if(!enable)
        {
            IsChopped = true;
            transform.Find("Veg").gameObject.SetActive(false);
            transform.Find("Chopped").gameObject.SetActive(true);
        }
    }
}
