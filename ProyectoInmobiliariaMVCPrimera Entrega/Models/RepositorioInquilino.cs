using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
	public class RepositorioInquilino
	{
		private readonly string connectionString;
		private readonly IConfiguration configuration;
		public RepositorioInquilino(IConfiguration configuration)
		{
			//agregmos 2 campor se solo lectura configuracion y la conexion
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}

		public int Alta(Inquilino i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilinos (Dni, Nombre, Apellido, Email, Telefono) " +
					$"VALUES (@dni, @nombre, @apellido, @email, @telefono);" +
					$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@dni", i.Dni);
					command.Parameters.AddWithValue("@nombre", i.Nombre);
					command.Parameters.AddWithValue("@apellido", i.Apellido);
					command.Parameters.AddWithValue("@email", i.Email);
					command.Parameters.AddWithValue("@telefono", i.Telefono);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.InquilinoId = res;
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
				string sql = $"DELETE FROM Inquilinos WHERE InquilinoId = @id";
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
		public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilinos SET Dni=@dni, Nombre=@nombre, Apellido=@apellido, Email=@email, Telefono=@telefono " +
					$"WHERE InquilinoId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@dni", i.Dni);
					command.Parameters.AddWithValue("@nombre", i.Nombre);
					command.Parameters.AddWithValue("@apellido", i.Apellido);
					command.Parameters.AddWithValue("@email", i.Email);
					command.Parameters.AddWithValue("@telefono", i.Telefono);
					command.Parameters.AddWithValue("@id", i.InquilinoId);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT InquilinoId, Dni, Nombre, Apellido, Email, Telefono" +
					$" FROM Inquilinos";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							InquilinoId = reader.GetInt32(0),
							Dni = reader.GetString(1),
							Nombre = reader.GetString(2),
							Apellido = reader.GetString(3),
							Email = reader.GetString(4),
							Telefono = reader.GetString(5),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT InquilinoId, Dni, Nombre, Apellido, Email, Telefono FROM Inquilinos" +
					$" WHERE InquilinoId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							InquilinoId = reader.GetInt32(0),
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
			return i;
		}

		

		
	}
}
