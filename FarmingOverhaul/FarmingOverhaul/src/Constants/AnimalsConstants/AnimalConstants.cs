using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;

namespace FarmingOverhaul.src.Constants.AnimalsConstants
{
    public class AnimalConstants
    {
        public EstrusConstants Estrus;
        public PregnancyConstants Pregnancy;
        public BreedingConstants Breeding;
        public EnergyConstants Energy;


        public List<ValidationResult> Validate()
        {
            List<ValidationResult> errors = [];

            ValidateConstantsClass(Estrus, nameof(Estrus), errors);
            ValidateConstantsClass(Pregnancy, nameof(Pregnancy), errors);
            ValidateConstantsClass(Breeding, nameof(Breeding), errors);
            ValidateConstantsClass(Energy, nameof(Energy), errors);

            return errors;
        }

        private static void ValidateConstantsClass<T>(T classInstance, string name, List<ValidationResult> errors) where T : class, IConstants
        {
            if (classInstance == null)
            {
                errors.Add(new ValidationResult(ValidationErrorType.NullClass, name));
                return;
            }
            else
            {
                errors.AddRange(classInstance.ValidateVariableRange());
            }
        }
    }
}