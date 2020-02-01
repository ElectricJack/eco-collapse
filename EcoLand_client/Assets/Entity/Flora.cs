namespace EntitySystem
{
    class Flora : BaseComponent, IEdible
    {
        public void OnEaten(Entity predator)
        {
            entity.Die(predator);
        }
    }
}

