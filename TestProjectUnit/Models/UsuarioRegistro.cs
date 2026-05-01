using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectUnit.Models
{
    public class UsuarioRegistro
    {
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
