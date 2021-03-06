//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DroneWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DroneLogEntry
    {
        public int DroneLogEntryId { get; set; }
        public Nullable<long> Tick_no { get; set; }
        public Nullable<double> OffsetTime { get; set; }
        public Nullable<int> FlightTime { get; set; }
        public Nullable<int> NavHealth { get; set; }
        public Nullable<double> GeneralRelHeight { get; set; }
        public string FlyCState { get; set; }
        public string ControllerCTRLMode { get; set; }
        public string BatteryStatus { get; set; }
        public Nullable<double> BatteryPercentage { get; set; }
        public Nullable<int> SmartBattGoHome { get; set; }
        public Nullable<int> SmartBattLand { get; set; }
        public string NonGPSCause { get; set; }
        public string CompassError { get; set; }
        public string ConnectedToRC { get; set; }
        public string BatteryLowVoltage { get; set; }
        public string GPSUsed { get; set; }
        public int FlightId { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
        public virtual DroneGP DroneGP { get; set; }
        public virtual DroneIMU_ATTI DroneIMU_ATTI { get; set; }
        public virtual DroneMotor DroneMotor { get; set; }
        public virtual DroneOA DroneOA { get; set; }
        public virtual DroneRC DroneRC { get; set; }
        public virtual DroneRTKData DroneRTKData { get; set; }
    }
}
