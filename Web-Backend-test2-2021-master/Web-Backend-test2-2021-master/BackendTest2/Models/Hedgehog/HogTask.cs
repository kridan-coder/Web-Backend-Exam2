using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTest2.Models
{
    public class HogTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ThemeId { get; set; }

        public Theme Theme { get; set; }


        [Required]
        public String FileNameIn { get; set; }

        [Required]
        public String FilePathIn { get; set; }

        [Required]
        public String FileNameOut { get; set; }

        [Required]
        public String FilePathOut { get; set; }


        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public Int64 Cost { get; set; }

        [Required]
        public Boolean Postmoderation { get; set; }

    }
}
