using UnityEngine;

public class StationaryAutopilot : Autopilot
{
    public StationaryAutopilot(VehicleController vehicle) : base(vehicle) { }

    protected override void SeekUpdate()
    {
       
    }

    protected override void ArriveUpdate()
    {
        
    }

    protected override void HaltUpdate()
    {
        ((StationaryVehicleController)VehicleController).BreakInput = true;
    }
}