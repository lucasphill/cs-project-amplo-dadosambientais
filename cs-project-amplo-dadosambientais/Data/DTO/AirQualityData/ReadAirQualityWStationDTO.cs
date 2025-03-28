﻿using cs_project_amplo_dadosambientais.Models;

namespace cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;

public class ReadAirQualityWStationDTO
{
    public Guid Id { get; set; }

    public Dictionary<string, object>? Data { get; set; }

    public string? Obs { get; set; }

    public DateTime InsertTimeStamp { get; set; }

    public StationModel Station { get; set; }
}
