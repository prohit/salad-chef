public enum KitchenStandState
{
    EMPTY,
    CONATIN_VEGETABLE,
    CONTAIN_PLATE,
    READY_TO_CHOP,
    CHOPPING,
    CHOPPING_DONE,
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
