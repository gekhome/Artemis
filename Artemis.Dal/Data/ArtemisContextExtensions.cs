using Artemis.Dal.Models;
using Artemis.Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Dal.Data
{
    public static class ArtemisContextExtensions
    {
        private const string ArtemisConnection =
            "Server=.\\SQLEXPRESS;Initial Catalog=Artemis;Trusted_Connection=True;" +
            "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

        public static IServiceCollection AddArtemisDbContext(this IServiceCollection services, string connectionString = ArtemisConnection)
        {
            services.AddDbContext<ArtemisDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuting });
            });
            return services;
        }

        public static IServiceCollection AddArtemisRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IMovieGenreService, MovieGenreService>();
            services.AddScoped<IMusicGenreService, MusicGenreService>();
            services.AddScoped<IMovieStudioService, MovieStudioService>();
            services.AddScoped<IMusicStudioService, MusicStudioService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IYearService, YearService>();

            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IFilmCountryService, FilmCountryService>();
            services.AddScoped<IFilmCompanyService, FilmCompanyService>();
            services.AddScoped<IFilmActorService, FilmActorService>();

            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IActorFilmService, ActorFilmService>();
            services.AddScoped<IActorGalleryService, ActorGalleryService>();

            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IChartRepository, ChartRepository>();

            return services;
        }
    }
}
