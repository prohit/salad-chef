public enum KitchenStandState
{
    EMPTY,
    CONATIN_VEGETABLE,
    READY_TO_CHOP,
    CHOPPING,
    CHOPPING_DONE,
    CONTAIN_SALAD,
    CUSTOMER_WAITING,
    CUSTOMER_EATING,
}

public enum PlayerState
{
    EMPTY_HAND,
    CHOPPING,
    CARRYING_VEGETABLE,
    CARRYING_CHOPPED_VEGETABLE,
    CARRYING_PLATE,
}

public enum Command
{
    PICKUP,
    DROP,
    CHOPPING
}

public enum VegType
{
    CARROT,
    LETTUCE,
    TOMATO,
    ONION,
    CUCUMBER,
    CAPSICUM
}

public enum OrderStatus
{
    NOT_SERVED,
    WRONG_SALAD_SERVED,
    CORRECT_SALAD_SERVERD,
    FAST_SERVED
}

public enum RewardType
{
    SPEED,
    TIME,
    SCORE
}
