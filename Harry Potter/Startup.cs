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
               opt.UseInMemoryDatabase("ListaPelículas"));
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

            WebRequest petición = WebRequest.Create(url);
            WebResponse respuesta = petición.GetResponse();
            StreamReader sr = new StreamReader(respuesta.GetResponseStream());

            string películasJS = sr.ReadToEnd().Trim();
            //Limpio el inicio
            int inicio = películasJS.IndexOf("[");
            películasJS = películasJS.Remove(0, inicio);
            //Limpio el final
            int final = películasJS.IndexOf("]");
            películasJS = películasJS.Remove(final+1);

            List<Película> películas = JsonSerializer.Deserialize<List<Película>>(películasJS);

            foreach (Película peli in películas)
            {
                if (peli.Type == "movie") 
                { 
                    peli.Id = peli.imdbID;
                    contexto.Películas.Add(peli);
                }
            }

            contexto.SaveChanges();
        }
    }
}
