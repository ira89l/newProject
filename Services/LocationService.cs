using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;

namespace CrossHealthX.Services;

public class LocationService
{
    private readonly List<Location> _locationHistory = new();
    private double _totalDistance = 0;

    public double TotalDistance => _totalDistance; // у метрах
    public int TotalSteps { get; private set; } = 0;

    public LocationService()
    {
        try
        {
            StepCounter.Default.StepChanged += StepCounter_StepChanged;
            StepCounter.Default.Start(StepCounterAccuracy.High);
        }
        catch
        {
            // StepCounter недоступний
        }
    }

    private void StepCounter_StepChanged(object sender, StepChangedEventArgs e)
    {
        TotalSteps = e.Steps;
    }

    // Отримати поточне місцезнаходження
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);
            if (location != null)
                AddLocation(location);
            return location;
        }
        catch
        {
            return null;
        }
    }

    // Додати точку та оновити дистанцію
    private void AddLocation(Location location)
    {
        if (_locationHistory.Count > 0)
        {
            var lastLocation = _locationHistory[^1];
            _totalDistance += CalculateDistance(lastLocation, location);
        }
        _locationHistory.Add(location);
    }

    // Розрахунок дистанції між двома точками у метрах
    private double CalculateDistance(Location start, Location end)
    {
        double R = 6371000; // радіус Землі у метрах
        double lat1 = start.Latitude * Math.PI / 180;
        double lat2 = end.Latitude * Math.PI / 180;
        double dLat = (end.Latitude - start.Latitude) * Math.PI / 180;
        double dLon = (end.Longitude - start.Longitude) * Math.PI / 180;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    // Скидання даних
    public void Reset()
    {
        _locationHistory.Clear();
        _totalDistance = 0;
        TotalSteps = 0;
    }

    // Асинхронний метод для отримання кількості кроків (приклад)
    public async Task<int> GetStepCountAsync()
    {
        await Task.Delay(50); // симуляція
        return TotalSteps;
    }
}
