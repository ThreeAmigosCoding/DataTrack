using DataTrack.Config;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class SimulationService : IHostedService
{
    //private readonly IDeviceService _deviceService;

    // public SimulationService(IDeviceService deviceService)
    // {
    //     _deviceService = deviceService;
    // }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // ScadaConfig.LoadScadaConfig();
        // RunSimulation();
        Console.WriteLine("Servis pokrenut");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    // private void RunSimulation()
    // {
    //     Task.Run(async () =>
    //     {
    //         
    //         var rand = new Random();
    //
    //         while (true)
    //         {
    //             Console.WriteLine("Reading all devices...");
    //             var devices = await _deviceService.ReadAll();
    //             Console.WriteLine($"Found {devices.Count} devices.");
    //
    //             foreach (var device in devices)
    //             {
    //                 // TODO: videti da li da driver ostane u tagu ili da se prebaci u device (sto ima vise smisla)
    //                 var driver = "SIMULATION";
    //                 if (driver == "SIMULATION")
    //                 {
    //                     if (device.IsDigital)
    //                     {
    //                         if (ScadaConfig.simulationType == "SINE") device.Value = Sine() > 0 ? 1 : 0;
    //                         else if (ScadaConfig.simulationType == "COSINE") device.Value = Cosine() > 0 ? 1 : 0;
    //                         else if (ScadaConfig.simulationType == "RAMP") device.Value = Ramp() > 0 ? 1 : 0;
    //                     }
    //                     else
    //                     {
    //                         if (ScadaConfig.simulationType == "SINE") device.Value = Sine();
    //                         else if (ScadaConfig.simulationType == "COSINE") device.Value = Cosine();
    //                         else if (ScadaConfig.simulationType == "RAMP") device.Value = Ramp(); 
    //                     }
    //                 }
    //                 else
    //                 {
    //                     var randValue = rand.Next(device.LowerBound, device.UpperBound);
    //                     var avg = (device.LowerBound + device.UpperBound) / 2;
    //
    //                     if (device.IsDigital) device.Value = randValue > avg ? 1 : 0;
    //                     else
    //                     {
    //                         double newVal;
    //                         if (randValue > avg)
    //                         {
    //                             newVal = device.Value + device.Value * 0.2;
    //                             if (newVal > device.UpperBound) newVal = device.UpperBound;
    //                         }
    //                         else
    //                         {
    //                             newVal = device.Value - device.Value * 0.2;
    //                             if (newVal < device.LowerBound) newVal = device.LowerBound;
    //                         }
    //
    //                         device.Value = newVal;
    //                     }
    //                 }
    //
    //                 await _deviceService.Update(device);
    //                 Console.WriteLine(device.Name + ": " + device.Value);
    //             }
    //
    //             await Task.Delay(ScadaConfig.updateFrequency);
    //         }
    //         
    //     });
    // }

    
    private static double Sine()
    {
        return 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
    }

    private static double Cosine()
    {
        return 100 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
    }

    private static double Ramp()
    {
        return 100 * DateTime.Now.Second / 60;
    }
}