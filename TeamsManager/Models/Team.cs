using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeamsManager.Models
{
    public class Team
    {
        public string Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string DisplayName { get; set; }
        public string InternalId { get; set; }
        //public string Specialization { get; set; }
        public object WebUrl { get; set; }
        public string Description { get; set; }
    }
}
