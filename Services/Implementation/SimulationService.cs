using DataTrack.Config;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class SimulationService : IHostedService
{
    private readonly IDeviceService _deviceService;
    private readonly IAnalogInputService _analogInputService;
    private readonly IDigitalInputService _digitalInputService;

     public SimulationService(IDeviceService deviceService, IAnalogInputService analogInputService, 
         IDigitalInputService digitalInputService)
     {
         _deviceService = deviceService;
         _analogInputService = analogInputService;
         _digitalInputService = digitalInputService;
     }
     
    public Task StartAsync(CancellationToken cancellationToken)
    {
        ScadaConfig.LoadScadaConfig();
        _analogInputService.StartReadingAll();
        _digitalInputService.StartReadingAll();
        RunSimulation(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async void RunSimulation(CancellationToken cancellationToken)
    {
         var rand = new Random();
            while (!cancellationToken.IsCancellationRequested)
            {
                var devices = (await _deviceService.ReadAll()).Where(d => !d.Deleted);
    
                foreach (var device in devices)
                {
                    var driver = device.Driver;
                    if (driver == "SIMULATION")
                    {
                        if (device.IsDigital)
                        {
                            if (ScadaConfig.simulationType == "SINE") device.Value = Sine() > 0 ? 1 : 0;
                            else if (ScadaConfig.simulationType == "COSINE") device.Value = Cosine() > 0 ? 1 : 0;
                            else if (ScadaConfig.simulationType == "RAMP") device.Value = Ramp() > 0 ? 1 : 0;
                        }
                        else
                        {
                            if (ScadaConfig.simulationType == "SINE") device.Value = Sine();
                            else if (ScadaConfig.simulationType == "COSINE") device.Value = Cosine();
                            else if (ScadaConfig.simulationType == "RAMP") device.Value = Ramp(); 
                        }

                        device.Value = Math.Round(device.Value, 3);
                    }
                    else
                    {
                        var randValue = rand.Next(device.LowerBound, device.UpperBound);
                        var avg = (device.LowerBound + device.UpperBound) / 2;

                        if (device.IsDigital)
                            device.Value = rand.Next(device.LowerBound, device.UpperBound + 1);
                        else
                        {
                            double newVal;
                            if (randValue > avg)
                            {
                                newVal = device.Value + device.Value * 0.05;
                                if (newVal > device.UpperBound) newVal = device.UpperBound;
                            }
                            else
                            {
                                newVal = device.Value - device.Value * 0.05;
                                if (newVal < device.LowerBound) newVal = device.LowerBound;
                            }

                            if (newVal < 0.001) newVal = 0.001;
                            
                            device.Value = Math.Round(newVal, 3);
                        }
                    }
    
                    await _deviceService.Update(device);
                    Console.WriteLine(device.Name + ": " + device.Value);
                }
    
                await Task.Delay(ScadaConfig.updateFrequency);
            }
    }


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