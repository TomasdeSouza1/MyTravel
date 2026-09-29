using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTravel.Application.DTOs.Auth
{
    public record RegisterRequestDto
    {

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [MaxLength(50, ErrorMessage ="El Nombre no puede superar los 50 caracteres")]
        public string FullName { get; init; } = string.Empty;
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; init; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(10, ErrorMessage = "La contraseña debe tener al menos 10 caracteres.")]
        public string Password { get; init; } = string.Empty;
    }
}
