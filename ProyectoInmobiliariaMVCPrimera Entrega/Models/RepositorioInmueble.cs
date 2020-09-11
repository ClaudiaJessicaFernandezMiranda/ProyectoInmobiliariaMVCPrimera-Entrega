using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
	public class RepositorioInmueble
	{
		private readonly string connectionString;
		private readonly IConfiguration configuration;
		public RepositorioInmueble(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}
		public int Alta(Inmueble i)
		{
			int res = -1;
			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Tipo, Costo, Superficie, Latitud, Longitud, PropietarioId,EstaPublicado,EstaHabilitado) " +
						$"VALUES (@direccion, @ambientes, @tipo, @costo, @superficie, @latitud, @longitud, @propietarioId,1,1);" +
						$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
					using (var command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@direccion", i.Direccion);
						command.Parameters.AddWithValue("@ambientes", i.Ambientes);
						command.Parameters.AddWithValue("@tipo", i.Tipo);
						command.Parameters.AddWithValue("@costo", i.Costo);
						command.Parameters.AddWithValue("@superficie", i.Superficie);
						command.Parameters.AddWithValue("@latitud", i.Latitud);
						command.Parameters.AddWithValue("@longitud", i.Longitud);
						command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);

						connection.Open();
						res = Convert.ToInt32(command.ExecuteScalar());
						i.InmuebleId = res;
						connection.Close();
					}
				}

			}
			catch (Exception ex)
			{

			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmuebles WHERE InmuebleId = @id";
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

		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmuebles SET " +
					"Direccion=@direccion, Ambientes=@ambientes, Tipo=@tipo, Costo=@costo, Superficie=@superficie, Latitud=@latitud, Longitud=@longitud, PropietarioId=@propietarioId " +
					"WHERE InmuebleId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@costo", i.Costo);
					command.Parameters.AddWithValue("@superficie", i.Superficie);
					command.Parameters.AddWithValue("@latitud", i.Latitud);
					command.Parameters.AddWithValue("@longitud", i.Longitud);
					command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
					command.Parameters.AddWithValue("@id", i.InmuebleId);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.InmuebleId, Direccion, Ambientes, Tipo, Costo, Superficie, Latitud, Longitud, i.PropietarioId, " +
					"p.Nombre,p.Apellido " +
					" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.propietarioId " +
					"ORDER BY Direccion ";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							InmuebleId = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Tipo = reader.GetString(3),
							Costo = reader.GetDecimal(4),
							Superficie = reader.GetDecimal(5),
							Latitud = reader.GetDecimal(6),
							Longitud = reader.GetDecimal(7),
							PropietarioId = reader.GetInt32(8),
							Propietario = new Propietario
							{
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							},
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT InmuebleId, Direccion, Ambientes, Tipo, Costo, Superficie, Latitud, Longitud, i.PropietarioId, " +
					"p.Nombre,p.Apellido " +
					"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.propietarioId " +
					" WHERE i.InmuebleId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							InmuebleId = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Tipo = reader.GetString(3),
							Costo = reader.GetDecimal(4),
							Superficie = reader.GetDecimal(5),
							Latitud = reader.GetDecimal(6),
							Longitud = reader.GetDecimal(7),
							PropietarioId = reader.GetInt32(8),
							Propietario = new Propietario
							{
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
					}
					connection.Close();
				}
			}
			return i;
		}

		//public IList<Inmueble> ObtenerPorPropietario(string propietarioId)
		//{
		//	IList<Inmueble> res = new List<Inmueble>();
		//	using (SqlConnection connection = new SqlConnection(connectionString))
		//	{
		//		string sql = $"SELECT InmuebleId, Direccion, Ambientes, Tipo, Costo, Superficie, Latitud, Longitud, PropietarioId, " +
		//			"p.Nombre,p.Apellido" +
		//			"FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.id" +
		//			" WHERE PropietarioId=@propietarioId";
		//		using (SqlCommand command = new SqlCommand(sql, connection))
		//		{
		//			command.CommandType = CommandType.Text;
		//			command.Parameters.Add("@propietarioId", SqlDbType.VarChar).Value = propietarioId;
		//			connection.Open();
		//			var reader = command.ExecuteReader();
		//			if (reader.Read())
		//			{
		//				Inmueble i = new Inmueble
		//				{
		//					InmuebleId = reader.GetInt32(0),
		//					Direccion = reader.GetString(1),
		//					Ambientes = reader.GetInt32(2),
		//					Tipo = reader.GetString(3),
		//					Costo = reader.GetDecimal(4),
		//					Superficie = reader.GetDecimal(5),
		//					Latitud = reader.GetDecimal(6),
		//					Longitud = reader.GetDecimal(7),
		//					PropietarioId = reader.GetInt32(8),
		//				};
		//				res.Add(i);
		//			}
		//			connection.Close();
		//		}
		//	}
		//	return res;
		//}
		

		

	}
}
