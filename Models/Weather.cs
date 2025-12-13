
public class Weather
{
    public float latitude { get; set; }
    public float longitude { get; set; }
    public float generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public float elevation { get; set; }
    public Current_Units current_units { get; set; }
    public Current current { get; set; }
}

public class Current_Units
{
    public string time { get; set; }
    public string interval { get; set; }
    public string wind_speed_10m { get; set; }
    public string temperature_2m { get; set; }
    public string rain { get; set; }
    public string cloud_cover { get; set; }
}

public class Current
{
    public string time { get; set; }
    public int interval { get; set; }
    public float wind_speed_10m { get; set; }
    public float temperature_2m { get; set; }
    public float rain { get; set; }
    public int cloud_cover { get; set; }
}


