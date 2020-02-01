namespace EntitySystem
{
    class Fauna : BaseComponent, IEdible
    {
        public void OnEaten(Entity predator)
        {
            entity.Die(predator);
        }
    }
}

