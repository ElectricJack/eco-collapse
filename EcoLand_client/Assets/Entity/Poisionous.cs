namespace EntitySystem
{
    class Poisionous : BaseComponent, IEdible
    {
        public int potency = 0;
        public void OnEaten(Entity predator)
        {
            Poisonable poison = predator.GetComponent<Poisonable>();
            if(poison != null)
            {
                poison.OnPoison(potency);
            }
        }
    }

    
    
}

