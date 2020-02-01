namespace EntitySystem
{
    public interface IEdible
    {
        float GetFilling();
        void OnEaten(Entity predator);
    }
}

