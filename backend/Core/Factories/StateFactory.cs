using Core.Entities;

namespace Core.Factories;

public static class StateFactory
{
    // The FACTORY PATTERN is a creative design pattern used to create objects
    // without specifying the exact class of the object to be created.
    public static State CreateState(string name)
    {
        return new State
        {
            Name = name
        };
    }
}
