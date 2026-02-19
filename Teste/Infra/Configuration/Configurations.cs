namespace Teste.Infra.Configuration
{
    public class Configurations
    {
        public static IConfiguration config { get; set; }

        public static string ConnectionString => config.GetConnectionString("DefaultConnection");
    }
}
