using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
	public class RepositorioAlquiler
	{
		private readonly string connectionString;
		private readonly IConfiguration configuration;
		public RepositorioAlquiler(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}
		public int Alta(Alquiler a)
		{
			int res = -1;
			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					string sql = $"INSERT INTO Alquileres (Descripcion, FechaAlta, FechaBaja, Monto, InmuebleId, InquilinoId) " +
						$"VALUES (@descripcion, @fechaAlta, @fechaBaja, @monto, @inmuebleId, @inquilinoId);" +
						$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
					using (var command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@descripcion", a.Descripcion);
						command.Parameters.AddWithValue("@fechaAlta", a.FechaAlta);
						command.Parameters.AddWithValue("@fechaBaja", a.FechaBaja);
						command.Parameters.AddWithValue("@monto", a.Monto);
						command.Parameters.AddWithValue("@inmuebleId", a.InmuebleId);
						command.Parameters.AddWithValue("@inquilinoId", a.InquilinoId);
						connection.Open();
						res = Convert.ToInt32(command.ExecuteScalar());
						a.AlquilerId = res;
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
				string sql = $"DELETE FROM Alquileres WHERE AlquilerId = @id";
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
		public int Modificacion(Alquiler a)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Alquileres SET " +
					"Descripcion=@descripcion, FechaAlta=@fechaAlta, FechaBaja=@fechaBaja, Monto=@monto, InmuebleId=@inmuebleId, InquilinoId=@inquilinoId " +
					"WHERE AlquilerId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@descripcion", a.Descripcion);
					command.Parameters.AddWithValue("@fechaAlta", a.FechaAlta);
					command.Parameters.AddWithValue("@fechaBaja", a.FechaBaja);
					command.Parameters.AddWithValue("@monto", a.Monto);
					command.Parameters.AddWithValue("@inmuebleId", a.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoId", a.InquilinoId);
					command.Parameters.AddWithValue("@id", a.AlquilerId);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		//arreglar INNER JOIN
		public IList<Alquiler> ObtenerTodos()
		{
			IList<Alquiler> res = new List<Alquiler>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $" SELECT AlquilerId, Descripcion, FechaAlta, FechaBaja, Monto, a.InmuebleId, a.InquilinoId," +
					$" iq.Nombre, iq.Apellido," +
					$" i.Direccion, i.Costo" +
					$" FROM Alquileres a join Inmuebles i ON a.InmuebleId = i.InmuebleId join Inquilinos iq ON a.InquilinoId = iq.inquilinoId";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Alquiler a = new Alquiler
						{
							AlquilerId = reader.GetInt32(0),
							Descripcion = reader.GetString(1),
							FechaAlta = reader.GetDateTime(2),
							FechaBaja = reader.GetDateTime(3),
							Monto = reader.GetString(4),
							InmuebleId = reader.GetInt32(5),
							InquilinoId = reader.GetInt32(6),
							inquilino = new Inquilino
							{
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(8),
							},
							inmueble = new Inmueble
							{
								Direccion = reader.GetString(9),
								Costo = reader.GetDecimal(10),
							},
						};
						res.Add(a);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Alquiler ObtenerPorId(int id)
		{
			Alquiler entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT AlquilerId, Descripcion, FechaAlta, FechaBaja, Monto, al.InmuebleId, al.InquilinoId, " +
					$" inm.Direccion" +
					$" FROM Alquileres al, Inmuebles inm" +
					$" WHERE al.InmuebleId = inm.InmuebleId and " +
					$"       al.AlquilerId=@id";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Alquiler
						{
							AlquilerId = reader.GetInt32(0),
							Descripcion = reader.GetString(1),
							FechaAlta = reader.GetDateTime(2),
							FechaBaja = reader.GetDateTime(3),
							Monto = reader.GetString(4),
							InmuebleId = reader.GetInt32(5),
							InquilinoId = reader.GetInt32(6),
							inmueble = new Inmueble
							{
								Direccion = reader.GetString(7),
							},
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}


		public IList<Alquiler> ObtenerInmueblePorDni(string dni)
		{
			IList<Alquiler> res = new List<Alquiler>();
			Alquiler a = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $" SELECT AlquilerId, Descripcion, FechaAlta, FechaBaja, Monto, a.InmuebleId, a.InquilinoId," +
					$" iq.Nombre, iq.Apellido," +
					$" i.Direccion, i.Costo" +
					$" FROM Alquileres a join Inmuebles i ON a.InmuebleId = i.InmuebleId " +
					$"                   join Inquilinos iq ON a.InquilinoId = iq.inquilinoId" +
					$" WHERE iq.Dni = @dni";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						a = new Alquiler
						{
							AlquilerId = reader.GetInt32(0),
							Descripcion = reader.GetString(1),
							FechaAlta = reader.GetDateTime(2),
							FechaBaja = reader.GetDateTime(3),
							Monto = reader.GetString(4),
							InmuebleId = reader.GetInt32(5),
							InquilinoId = reader.GetInt32(6),
							inquilino = new Inquilino
							{
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(8),
							},
							inmueble = new Inmueble
							{
								Direccion = reader.GetString(9),
								Costo = reader.GetDecimal(10),
							},
						};
						res.Add(a);
					}
					connection.Close();
				}
			}
			return res;
		}

		
	}
}