namespace Aplicacion.CadenaConexion
{
    public static class CadenaConexion
    {
        // Atributos
        private const string Servidor = @"FACUNDO-PC"; // Cambia
        private const string BaseDatos = @"CommerceDbC4";
        private const string Usuario = @"sa";
        private const string Password = @"Gamuza Bazan"; // Cambia

        // Propiedad
        public static string ObtenerCadenaSql => $"Data Source={Servidor}; " +
                                                 $"Initial Catalog={BaseDatos}; " +
                                                 $"User Id={Usuario}; " +
                                                 $"Password={Password};";

        public static string ObtenerCadenaWin => $"Data Source={Servidor}; " +
                                                 $"Initial Catalog={BaseDatos}; " +
                                                 $"Integrated Security=true;";
    }
}
