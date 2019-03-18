using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] RewardType type;
    [SerializeField] float rewardAmount; //score, time, speed

    public RewardType Type { get { return type; } }
    public float RewardAmount { get { return rewardAmount; } }
    public Player Owner { get; set; }
}
