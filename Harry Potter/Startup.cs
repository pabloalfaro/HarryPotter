using System.IO;
using System.Reflection;
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Harry_Potter.Modelos;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;

namespace Harry_Potter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContextoBD>(opt =>
               opt.UseInMemoryDatabase("ListaPel�culas"));
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            var scope = app.ApplicationServices.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<ContextoBD>();
            LeerDatos(contexto);

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void LeerDatos(ContextoBD contexto)
        {
            string url = "https://www.omdbapi.com/?apikey=731e41f&s=harry+potter";

            WebRequest petici�n = WebRequest.Create(url);
            WebResponse respuesta = petici�n.GetResponse();
            StreamReader sr = new StreamReader(respuesta.GetResponseStream());

            string pel�culasJS = sr.ReadToEnd().Trim();
            //Limpio el inicio
            int inicio = pel�culasJS.IndexOf("[");
            pel�culasJS = pel�culasJS.Remove(0, inicio);
            //Limpio el final
            int final = pel�culasJS.IndexOf("]");
            pel�culasJS = pel�culasJS.Remove(final+1);

            List<Pel�cula> pel�culas = JsonSerializer.Deserialize<List<Pel�cula>>(pel�culasJS);

            foreach (Pel�cula peli in pel�culas)
            {
                if (peli.Type == "movie") 
                { 
                    peli.Id = peli.imdbID;
                    contexto.Pel�culas.Add(peli);
                }
            }

            contexto.SaveChanges();
        }
    }
}
