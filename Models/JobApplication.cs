using System;

namespace JobTrack.Models
{
    public class JobApplication
    {
        public string? Company { get; set; }
        public string? Role { get; set; }
        public DateTime DateApplied { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}