using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackendTest2.Models;

namespace BackendTest2.Models.ViewModels
{
    public class UsersFilterViewModel
    {

        public string Email { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

    }
}
