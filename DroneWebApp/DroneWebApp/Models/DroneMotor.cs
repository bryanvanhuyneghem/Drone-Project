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
    
    public partial class DroneMotor
    {
        public int MotorId { get; set; }
        public Nullable<double> CurrentRFront { get; set; }
        public Nullable<double> CurrentLFront { get; set; }
        public Nullable<double> CurrentLBack { get; set; }
        public Nullable<double> CurrentRBack { get; set; }
    
        public virtual DroneLogEntry DroneLogEntry { get; set; }
    }
}
