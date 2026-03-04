using System;

public enum SpeciesList
{
    Hare, Chicken, Pig, Sheep, Goat
}

public static class HelperFunctions
{
    public static int GenerateRandomInt(int min, int max)
    {
        return Random.Shared.Next(min, max + 1);
    }
}
public record AnimalConstants
{
    public SpeciesList Species {get; init; }

    public double MaintenanceEnergyReqConst {get; init; }
    public double BodyWeightExponent {get; init; }
    public double GrowthCoefficient {get; init; }
    public double LactationEnergyReqConst {get; init; }
    public int KCalPerKgWeightChange {get; init; }

    public int MinDaysPregnant {get; init; }
    public int MaxDaysPregnant {get; init; }
    public double LateGestationPercent {get; init; }

    public double BaseFetusEnergyModifier {get; init; }
    public double AdditionalFetusEnergyModifier {get; init; }
    public double EarlyGestationFetusEnergyModifier {get; init; }

    public double KCalPerKgMilkProduced {get; init; }
    public double LowerTempRange {get; init; }
    public double UpperTempRange {get; init; }
}

public static class AnimalConstantsProvider
{
    static readonly AnimalConstants Hare = new();
    static readonly AnimalConstants Chicken = new();
    static readonly AnimalConstants Pig = new();
    static readonly AnimalConstants Sheep = new()
    {
        Species = SpeciesList.Sheep,
        MaintenanceEnergyReqConst = 0.056,
        BodyWeightExponent = 0.75,
        GrowthCoefficient = 0.276,
        LactationEnergyReqConst = 5,
        KCalPerKgWeightChange = 7000,

        MinDaysPregnant = 142,
        MaxDaysPregnant = 152,
        LateGestationPercent = 0.70,

        BaseFetusEnergyModifier = 0.1,
        AdditionalFetusEnergyModifier = 0.05,
        EarlyGestationFetusEnergyModifier = 0.025,

        KCalPerKgMilkProduced = 0,
        LowerTempRange = 0,
        UpperTempRange = 0,
    };
    static readonly AnimalConstants Goat = new();

    public static AnimalConstants GetConstantsFromSpecies(SpeciesList species)
    {
        return species switch
        {
            SpeciesList.Hare => Hare,
            SpeciesList.Chicken => Chicken,
            SpeciesList.Pig => Pig,
            SpeciesList.Sheep => Sheep,
            SpeciesList.Goat => Goat,
            _ => throw new NotImplementedException()
        };
    }
}

public class Animal
{
    public SpeciesList Species {get;}
    public AnimalConstants Constants {get;}

    //scaled based on body weight, implement
    double kgMilkProducedDaily = 1;
    public int DaysPregnant {get; set;} = 0;
    int pregnancyLength = 0;
    bool lateGestation = false;
    public int FetusAmount {get; set;} = 0;

    bool belowTempThresh = false;

    double bodyWeight = 0;
    double caloriesConsumed = 0;

    public Animal(SpeciesList species)
    {
        Species = species;
        Constants = AnimalConstantsProvider.GetConstantsFromSpecies(species);
        pregnancyLength = HelperFunctions.GenerateRandomInt(Constants.MinDaysPregnant, Constants.MaxDaysPregnant);
    }

    private bool InLateGestation()
    {
        if (DaysPregnant / pregnancyLength < Constants.LateGestationPercent)
            {
                return false;
            }
        else return true;
    }
    public double CalculateTotalMaintenanceEnergyReq()
    {
        double temperatureModifer = 1;
        double pregnancyEnergyReq;
        double metabolicBodyWeight = Math.Pow(bodyWeight, Constants.BodyWeightExponent);
        double baseMaintenanceEnergyReq = Constants.MaintenanceEnergyReqConst * metabolicBodyWeight;
        double lactationEnergyReq = Constants.LactationEnergyReqConst * kgMilkProducedDaily;
        
        if (InLateGestation())
            pregnancyEnergyReq = (Constants.BaseFetusEnergyModifier + (Constants.AdditionalFetusEnergyModifier * (FetusAmount - 1))) * bodyWeight;
        else if (FetusAmount > 0)
            pregnancyEnergyReq = Constants.EarlyGestationFetusEnergyModifier * FetusAmount * bodyWeight;
        else
            pregnancyEnergyReq = 0;

        if (localTemperature > Constants.UpperTempRange)
        {
            //have it scale based on high much higher it is
            temperatureModifer = 1.25;
        }
        if (localTemperature < Constants.LowerTempRange)
        {
            //have it scale based on high much lower it is
            temperatureModifer = 1.25;
        }
        return (baseMaintenanceEnergyReq + lactationEnergyReq + pregnancyEnergyReq) * temperatureModifer;

    }

    public void AdjustAnimalWeight(double totalMaintenanceEnergyReq)
    {
        double excessCalories = caloriesConsumed - totalMaintenanceEnergyReq;

        if (excessCalories != 0)
        {
            this.bodyWeight += excessCalories / Constants.KCalPerKgWeightChange;
        }
    }
}