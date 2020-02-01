namespace EntitySystem
{
    class Fauna : BaseComponent, IEdible
    {
        public float HowFilling = 0f;
        
        public float GetFilling()
        {
            return HowFilling;
        }

        public void OnEaten(Entity predator)
        {
            entity.Die(predator);
        }
    }
}

