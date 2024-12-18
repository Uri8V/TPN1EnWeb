using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient; // Asegúrate de instalar este paquete
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Instalar paquete Newtonsoft.Json para deserialización de JSON


namespace TPN1EnWeb.API.CountryStateCity
{
    //Clases Country, State, y City:
    //Modelan las entidades que recibimos de la API con sus propiedades alineadas a los nombres de la API.
    // Representa un país
    public class Country
    {
        public string id { get; set; } // ID alfanumérico del país
        public string name { get; set; } // Nombre del país
        public string iso2 { get; set; } // Código ISO del país
    }

    // Representa un estado
    public class State
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country_id")]
        public int CountryId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("iso2")]
        public string Iso2 { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }

    // Representa una ciudad
    public class City
    {
        public int id { get; set; } // ID alfanumérico de la ciudad
        public string name { get; set; } // Nombre de la ciudad
        public int state_id { get; set; } // Relación con el ID del estado
        public string state_code { get; set; }
        public int country_id { get; set; } // Relación con el ID del país
        public string country_code { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                //const string API_KEY = "SzNqR2NJZUtUcXBmTmlZeWNnOUF1OVJuQ1h3aWNXa28yZjFSZ3U2cA==";

                //Console.WriteLine("Obteniendo países desde la API...");
                //var countries = await GetCountriesFromAPI(API_KEY);

                //Console.WriteLine("Guardando países en la base de datos...");
                //SaveCountriesToDatabase(countries);

                //Console.WriteLine("Obteniendo estados desde la API...");
                //var states = await GetStatesWithDetails(countries, API_KEY);

                //Console.WriteLine("Guardando estados en la base de datos...");
                //SaveStatesToDatabase(states);

                Console.WriteLine("Obteniendo ciudades desde la API...");
                string filePathCities = @"C:/Users/uri8m/OneDrive/Escritorio/3er Año ASI/Maquina para hacer Cafe/countries-states-cities-database-master/json/cities.json";

                var conn = "Data Source=.;Initial Catalog=ShoesEFCore;Trusted_Connection=true;TrustServerCertificate=true;";
                var e=SaveCitiesAsync(filePathCities, conn);
                
                Console.ReadLine();

                if (e.IsCompleted)
                {
                    Console.WriteLine("¡Proceso completado exitosamente!");
                }
                else
                {
                    Console.WriteLine("No se pudo completar");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }

        //Métodos GetCountriesFromAPI, GetStatesFromAPI:
        //Se encargan de hacer las solicitudes HTTP y deserializar la respuesta JSON.

        // Obtiene los países desde la API
        static async Task<List<Country>> GetCountriesFromAPI(string apiKey)
        {
            var apiUrl = "https://api.countrystatecity.in/v1/countries";

            using var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(apiUrl),
                Headers =
                {
                    { "X-CSCAPI-KEY", apiKey }
                }
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Country>>(responseBody)!;
        }

        // Obtiene los estados para cada país desde la API
        static async Task<List<State>> GetStatesWithDetails(List<Country> countries, string apiKey)
        {
            var states = new List<State>();
            using var client = new HttpClient();

            foreach (var country in countries)
            {
                // 1. Obtener la lista inicial de estados para este país
                var initialUrl = $"https://api.countrystatecity.in/v1/countries/{country.iso2}/states";
                var initialRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(initialUrl),
                    Headers = { { "X-CSCAPI-KEY", apiKey } }
                };

                var initialResponse = await client.SendAsync(initialRequest);
                if (!initialResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener los estados para el país {country.name}");
                    continue; // Pasamos al siguiente país si hay un error
                }

                var initialResponseBody = await initialResponse.Content.ReadAsStringAsync();
                var basicStates = JsonConvert.DeserializeObject<List<State>>(initialResponseBody)!;

                // 2. Iterar sobre cada estado para obtener los detalles
                foreach (var state in basicStates)
                {
                    var detailUrl = $"https://api.countrystatecity.in/v1/countries/{country.iso2}/states/{state.Iso2}";
                    var detailRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(detailUrl),
                        Headers = { { "X-CSCAPI-KEY", apiKey } }
                    };

                    var detailResponse = await client.SendAsync(detailRequest);
                    if (!detailResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error al obtener detalles para el estado {state.Name}");
                        continue;
                    }

                    var detailResponseBody = await detailResponse.Content.ReadAsStringAsync();
                    var detailedState = JsonConvert.DeserializeObject<State>(detailResponseBody)!;

                    // Agregar país y detalles al estado
                    detailedState.CountryId = int.Parse(country.id); // Asociamos el estado al país por su ID
                    states.Add(detailedState);
                }
            }

            return states;
        }

        //Métodos SaveCountriesToDatabase, SaveStatesToDatabase:
        //Guardan los datos en la base de datos usando SqlCommand.

        // Guarda los países en la base de datos
        static void SaveCountriesToDatabase(List<Country> countries)
        {
            string connectionString = "Data Source=.;Initial Catalog=ShoesEFCore;Trusted_Connection=True;TrustServerCertificate=True;";

            using var connection = new SqlConnection(connectionString);
            connection.Open();
           
            foreach (var country in countries)
            {
                var query = "INSERT INTO Countries (CountryId, CountryName, CountryCode) VALUES (@CountryId, @CountryName, @CountryCode)";
                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CountryId", country.id);
                command.Parameters.AddWithValue("@CountryName", country.name);
                command.Parameters.AddWithValue("@CountryCode", country.iso2);

                command.ExecuteNonQuery();
            }
        }

        // Guarda los estados en la base de datos
        static void SaveStatesToDatabase(List<State> states)
        {
            string connectionString = "Data Source=.;Initial Catalog=ShoesEFCore;Trusted_Connection=True;TrustServerCertificate=True;";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            foreach (var state in states)
            {
                var query = "INSERT INTO States (StateId, StateName,CountryId) VALUES (@StateId, @StateName,@CountryId)";
                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StateId", state.Id);
                command.Parameters.AddWithValue("@StateName", state.Name);
                command.Parameters.AddWithValue("@CountryId", state.CountryId);

                command.ExecuteNonQuery();
            }
        }

        public static async Task SaveCitiesAsync(string citiesJsonPath, string connectionString)
        {
            List<City> cities= new List<City>();
            
            try
            {
                if (!File.Exists(citiesJsonPath))
                {
                    Console.WriteLine($"Archivo no encontrado: {citiesJsonPath}");
                    return;
                }
                // Leer archivo JSON
               var citiesJson = await File.ReadAllTextAsync(citiesJsonPath);
               cities = JsonConvert.DeserializeObject<List<City>>(citiesJson)!;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar el JSON: {ex.Message}");
            }

            if (cities == null)
            {
                Console.WriteLine("Error: No se pudieron deserializar los archivos JSON.");
                return;
            }

            // Conectar a la base de datos
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            foreach (var city in cities)
            {
                // Crear comando SQL para insertar ciudad
                var command = new SqlCommand(@"
            INSERT INTO Cities (CityId, CityName, StateId, CountryId)
            VALUES (@Id, @Name, @StateId, @CountryId)", connection);

                // Agregar parámetros al comando
                command.Parameters.AddWithValue("@Id", city.id);
                command.Parameters.AddWithValue("@Name", city.name);
                command.Parameters.AddWithValue("@StateId", city.state_id);
                command.Parameters.AddWithValue("@CountryId",city.country_id);

                try
                {
                    // Ejecutar comando
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al insertar la ciudad {city.name}: {ex.Message}");
                }
            }
                Console.WriteLine("Ciudades guardadas correctamente.");
        }











































































        //// Obtiene las ciudades para cada estado desde la API
        //static async Task<List<City>> GetCitiesFromAPI(List<State> states, string apiKey)
        //{
        //    var cities = new List<City>();

        //    using var client = new HttpClient();

        //    foreach (var state in states)
        //    {
        //        var apiUrl = $"https://api.countrystatecity.in/v1/countries/{state.CountryCode}/states/{state.Id}/cities";

        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri(apiUrl),
        //            Headers =
        //            {
        //                { "X-CSCAPI-KEY", apiKey }
        //            }
        //        };

        //        var response = await client.SendAsync(request);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine($"Error al obtener ciudades para {state.Name}. Saltando...");
        //            continue;
        //        }

        //        var responseBody = await response.Content.ReadAsStringAsync();
        //        cities.AddRange(JsonConvert.DeserializeObject<List<City>>(responseBody)!);
        //    }

        //    return cities;
        //}

        //// Guarda las ciudades en la base de datos
        //static void SaveCitiesToDatabase(List<City> cities)
        //{
        //    string connectionString = "Data Source=.;Initial Catalog=ShoesEFCore;Trusted_Connection=True;TrustServerCertificate=True;";

        //    using var connection = new SqlConnection(connectionString);
        //    connection.Open();

        //    foreach (var city in cities)
        //    {
        //        var query = "INSERT INTO Cities (CityId, CityName, StateId, CountryId) VALUES (@CityId, @CityName, @StateId, @CountryId)";
        //        using var command = new SqlCommand(query, connection);

        //        command.Parameters.AddWithValue("@CityId", city.id);
        //        command.Parameters.AddWithValue("@CityName", city.name ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@StateId", city.state_id);
        //        command.Parameters.AddWithValue("@CountryId", city.country_id);

        //        command.ExecuteNonQuery();
        //    }
        //}
    }
}