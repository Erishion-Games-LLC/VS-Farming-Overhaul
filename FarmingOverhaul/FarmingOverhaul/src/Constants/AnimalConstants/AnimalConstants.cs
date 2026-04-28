using System.Collections.Generic;

namespace FarmingOverhaul.src.Constants.AnimalConstants
{
    public class AnimalConstants
    {
        public EstrusConstants Estrus;
        public PregnancyConstants Pregnancy;
        public BreedingConstants Breeding;
        public EnergyConstants Energy;


        public List<string> Validate()
        {
            List<string> errors = [];

            ValidateConstantsClass(Estrus, nameof(Estrus), errors);
            ValidateConstantsClass(Pregnancy, nameof(Pregnancy), errors);
            ValidateConstantsClass(Breeding, nameof(Breeding), errors);
            ValidateConstantsClass(Energy, nameof(Energy), errors);

            return errors;
        }

        private static void ValidateConstantsClass<T>(T classInstance, string name, List<string> errors) where T : class, IConstants
        {
            if (classInstance == null)
            {
                errors.Add($"{name} constants cannot be null");
                return;
            }
            else
            {
                errors.AddRange(classInstance.ValidateVariableRange());
            }
        }
    }
}