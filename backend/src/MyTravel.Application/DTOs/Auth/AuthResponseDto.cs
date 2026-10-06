using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTravel.Application.DTOs.Auth
{
    public sealed record AuthResponseDto
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }=string.Empty;
        public string FullName { get; init; }=string.Empty;
        public string Token { get; init; }=string.Empty;
    }
}
