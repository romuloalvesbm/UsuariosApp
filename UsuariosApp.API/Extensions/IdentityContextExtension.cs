﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UsuariosApp.API.Identity.Contexts;
using UsuariosApp.API.Identity.Entities;
using UsuariosApp.API.Settings;


namespace UsuariosApp.API.Extensions
{
    /// <summary>
    /// Classe de extensão para o AspNetCore Identity / EntityFramework
    /// </summary>
    public static class IdentityContextExtension
    {
        public static IServiceCollection AddIdentityContext
            (this IServiceCollection services, IConfiguration configuration)
        {
            //configurando os parametros do /appsettings
            var identitySettings = new IdentitySettings();
            new ConfigureFromConfigurationOptions<IdentitySettings>
                (configuration.GetSection("IdentitySettings"))
                .Configure(identitySettings);

            services.AddSingleton(identitySettings);

            //injeção de dependência para o contexto
            services.AddDbContext<IdentityContext>
                (options => options.UseNpgsql(configuration.GetConnectionString("BDUsuariosApp")));

            //configurar as preferências do Identity
            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; //não necessita de confirmação de cadastro
                options.User.RequireUniqueEmail = true; //email deve ser unico para cada usuário
                options.Password.RequireDigit = false; //senha deve ter dígitos numéricos
                options.Password.RequiredLength = 8; //senha deve ter no mínimo 8 caracteres
                options.Password.RequireNonAlphanumeric = true; //senha deve ter caracteres especiais
                options.Password.RequireUppercase = false; //senha deve ter letras maiúsculas
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
    }
}

