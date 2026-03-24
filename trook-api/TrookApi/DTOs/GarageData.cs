using TrookApi.Database.Entities;

namespace TrookApi.DTOs;

public record GarageData(
    List<Garage> Entities,
    Dictionary<string, Garage> DriverIdMap,
    Dictionary<string, Garage> TruckIdMap,
    Dictionary<string, Garage> TrailerIdMap,
    Dictionary<string, Garage> ProfitLogIdMap);