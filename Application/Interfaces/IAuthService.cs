﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> AutenticarOuRegistrarUsuarioGoogleAsync(string email, string nome, string fotoPerfil);
    }

}
