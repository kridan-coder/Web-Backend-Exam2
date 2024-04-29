using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackendTest2.Models;

namespace BackendTest2.Models.ViewModels
{
    public class TasksFilterViewModel
    {

        public string Theme { get; set; }

        public string Name { get; set; }

    }
}
