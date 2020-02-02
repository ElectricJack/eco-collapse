namespace EntitySystem
{
    class Poisonable : BaseComponent, IStatusStep
    {
        private int poisonVal = 0;

        public void OnPoison(int potency)
        {
            poisonVal += potency;
        }

        public void StatusStep()
        {
            if (poisonVal > 0)
            {
                entity.deathAge -= poisonVal;
                poisonVal--;
            }
        }
    }
}

