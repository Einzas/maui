using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speed.Models
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public SpeedData Data { get; set; }
    }
}
