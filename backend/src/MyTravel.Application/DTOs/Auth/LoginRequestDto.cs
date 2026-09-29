using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTravel.Application.DTOs.Auth
{
    public record LoginRequestDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; init; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; init; } = string.Empty;
    }
}
