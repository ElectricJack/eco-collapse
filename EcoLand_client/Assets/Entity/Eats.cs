namespace EntitySystem
{
    enum VoreType
    {
        herbivore,
        carnivore,
        omnivore
    }
    class Eats : BaseComponent, ISteppable
    {
        public    VoreType edibleType;
        public    int         howFilling;
        
        public void Step()
        {
            // Is there something close enough to eat?
        }
        public void OnEats(Entity prey)
        {
            var edibles = prey.instance.GetComponents<IEdible>();
            if (edibles == null)
                return;
            
            
            foreach(var edible in edibles)
            {
                switch(edibleType)
                {
                    case VoreType.carnivore:
                        if(edible.GetType() == typeof(Fauna))
                            edible.OnEaten(entity);
                        break;
                    case VoreType.herbivore:
                        if(edible.GetType() == typeof(Flora))
                            edible.OnEaten(entity);
                        break;
                    default:
                        edible.OnEaten(entity);
                        break;
                }
                
            }
                

            entity.stomachFullness += howFilling;
        }
    }

    
    
}

