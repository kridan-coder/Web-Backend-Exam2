using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTest2.Models
{
    public class HogTaskUser
    {
        public String UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid HogTaskId { get; set; }
        public HogTask HogTask { get; set; }

        [Required]
        public String FileSolutionName { get; set; }

        [Required]
        public String FileSolutionPath { get; set; }

        public String SolutionLanguage { get; set; }
        public String Verdict { get; set; } = "None";

    }
}
