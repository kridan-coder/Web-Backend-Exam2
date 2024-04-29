using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTest2.Models
{
    public class Theme
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? FatherThemeId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public Int32 MinimumPoints { get; set; }

        public ICollection<Theme> ChildThemes { get; set; }

        public ICollection<HogTask> HogTasks { get; set; }

    }
}
