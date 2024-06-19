
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Text;

namespace Artemis.Dal.Library
{
    public static class Common
    {
        public const int LOGO_IMAGE_MAXSIZE = 65_536;       // 64KB
        public const int IMAGE_MAXSIZE = 819_200;           // 800KB

        private static IConfiguration? config;

        public static IConfiguration Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                config = builder.Build();
                return config;
            }
        }


        public static ArtemisDbContext CreateDbContext()
        {
            string? connectionString = Configuration.GetConnectionString("ArtemisConnection")
                ?? throw new InvalidOperationException("Connection string 'ArtemisConnection' not found!");

            var optionsBuilder = new DbContextOptionsBuilder<ArtemisDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            ArtemisDbContext dbContext = new(optionsBuilder.Options);
            return dbContext;
        }

        public static long GetChecksum(string text)
        {
            long sum = 0;
            byte overflow;
            for (int i = 0; i < text.Length; i++)
            {
                sum = (16 * sum) ^ Convert.ToUInt32(text[i]);
                overflow = (byte)(sum / 4294967296);
                sum -= overflow * 4294967296;
                sum ^= overflow;
            }

            if (sum > 2147483647)
                sum -= 4294967296;
            else if (sum >= 32768 && sum <= 65535)
                sum -= 65536;
            else if (sum >= 128 && sum <= 255)
                sum -= 256;

            sum = Math.Abs(sum);

            return sum;
        }

        public static bool ValidImageExtension(string extension)
        {
            string[] extensions = { ".JPEG", ".JPG", ".GIF", ".PNG", ".TIF", ".TIFF", ".BMP" };

            List<string> allowed_extensions = new(extensions);

            if (allowed_extensions.Contains(extension.ToUpper()))
                return true;
            return false;
        }

        public static byte[] FileToByteArray(IFormFile file)
        {
            byte[] result = Array.Empty<byte>();
            if (file != null)
            {
                using var dataStream = new MemoryStream();
                file.CopyTo(dataStream);
                result = dataStream.ToArray();
            }
            return result;
        }

        #region Handle genres storage

        public static string GenreValuesToString(int[] genreValues)
        {
            using var db = CreateDbContext();

            StringBuilder sb = new();
            List<MovieGenre> data = db.MovieGenre.ToList();
            string[] names = new string[genreValues.Length];

            int idx = 1;
            foreach (int value in genreValues)
            {
                names[idx - 1] = GetGenreName(value, data);
                idx++;
            }
            sb.AppendJoin(',', names);
            string result = sb.ToString();

            return result;
        }

        public static int[] GenreNamesToValues(string genreNames)
        {
            using var db = CreateDbContext();

            if (genreNames == null) return Array.Empty<int>();

            string[] names = genreNames.Split(',');
            int[] values = new int[names.Length];

            List<MovieGenre> data = db.MovieGenre.ToList();

            int idx = 0;
            foreach (string name in names)
            {
                values[idx] = GetGenreId(name, data);
                idx++;
            }
            return values;
        }

        public static string GetGenreName(int genreId, List<MovieGenre> data)
        {
            string result = data.Find(x => x.GenreId == genreId)?.GenreName ?? string.Empty;
            return result;
        }

        public static int GetGenreId(string genreName, List<MovieGenre> data)
        {
            if (genreName == null) return 0;

            int? result = data.Find(x => x.GenreName == genreName)?.GenreId;
            return result ?? default;
        }

        #endregion

        #region Validations


        #endregion
    }
}
