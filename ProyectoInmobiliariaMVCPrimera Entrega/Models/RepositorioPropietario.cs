using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//es 
namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
	public class RepositorioPropietario
	{
		//2 campos de solo lectura
		private readonly string connectionString;
		private readonly IConfiguration configuration;

		public RepositorioPropietario(IConfiguration configuration)
		{
			//agregmos 2 campor se solo lectura configuracion y la conexion
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}

		public int Alta(Propietario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Propietarios (Dni, Nombre, Apellido, Email, Telefono) " +
					$"VALUES (@dni, @nombre, @apellido, @email, @telefono);" +
					$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@email", p.Email);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.PropietarioId = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Propietarios WHERE PropietarioId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Propietario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Propietarios SET Dni=@dni, Nombre=@nombre, Apellido=@apellido, Email=@email, Telefono=@telefono " +
					$"WHERE PropietarioId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@email", p.Email);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@id", p.PropietarioId);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Propietario> ObtenerTodos()
		{
			IList<Propietario> res = new List<Propietario>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT PropietarioId, Dni, Nombre, Apellido,  Email, Telefono" +
					$" FROM Propietarios";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							PropietarioId = reader.GetInt32(0),
							Dni = reader.GetString(1),
							Nombre = reader.GetString(2),
							Apellido = reader.GetString(3),
							Email = reader.GetString(4),
							Telefono = reader.GetString(5),

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Propietario ObtenerPorId(int id)
		{
			Propietario p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT PropietarioId, Dni, Nombre, Apellido, Email, Telefono FROM Propietarios" +
					$" WHERE PropietarioId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							PropietarioId = reader.GetInt32(0),
							Dni = reader.GetString(1),
							Nombre = reader.GetString(2),
							Apellido = reader.GetString(3),
							Email = reader.GetString(4),
							Telefono = reader.GetString(5),


						};
					}
					connection.Close();
				}
			}
			return p;
		}

		

	}
}
