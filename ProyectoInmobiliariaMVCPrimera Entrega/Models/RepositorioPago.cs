using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models

{
	public class RepositorioPago
	{
		private readonly string connectionString;
		private readonly IConfiguration configuration;
		public RepositorioPago(IConfiguration configuration)
		{
			this.configuration = configuration;
			connectionString = configuration["ConnectionStrings:DefaultConnection"];
		}
		public int Alta(Pago p)
		{
			int res = -1;
			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					string sql = $"INSERT INTO Pagos (Numero, Fecha, Importe, AlquilerId) " +
						$"VALUES (@numero, @fecha, @importe, @alquilerId);" +
						$"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
					using (var command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@numero", p.Numero);
						command.Parameters.AddWithValue("@fecha", p.Fecha);
						command.Parameters.AddWithValue("@importe", p.Importe);
						command.Parameters.AddWithValue("@AlquilerId", p.AlquilerId);
						connection.Open();
						res = Convert.ToInt32(command.ExecuteScalar());
						p.PagoId = res;
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
				string sql = $"DELETE FROM Pagos WHERE PagoId = @id";
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

		public int Modificacion(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Pagos SET " +
					"Numero=@numero, Fecha=@fecha, Importe=@importe, AlquilerId=@alquilerId " +
					"WHERE PagoId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@numero", p.Numero);
					command.Parameters.AddWithValue("@fecha", p.Fecha);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@alquilerId", p.AlquilerId);
					command.Parameters.AddWithValue("@id", p.PagoId);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				//string sql = $" SELECT p.PagoId, Numero, Fecha, Importe, p.AlquilerId, " +
				//	"$ a.Monto, a.Descripcion, " +
				//	"$ iq.InquilinoId, iq.Nombre, iq.Apellido " +
				//	"$ FROM Alquileres a INNER JOIN Pagos p ON a.AlquilerId = p.AlquilerId INNER JOIN Inquilinos iq ON a.InquilinoId = iq.inquilinoId" +
				//	"$ ORDER BY Numero ";
				string sql = $" SELECT p.PagoId, Numero, Fecha, Importe, p.AlquilerId " +
					$" FROM Pagos p, Alquileres a, Inquilinos i " +
					$" WHERE p.AlquilerId = a.AlquilerId and " +
					$"       a.InquilinoId = i.InquilinoId ";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							PagoId = reader.GetInt32(0),
							Numero = reader.GetString(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetString(3),
							AlquilerId = reader.GetInt32(4),
							alquiler = new Alquiler
							{
								Monto = reader.GetString(5),
								Descripcion = reader.GetString(6),
							},
							inquilino = new Inquilino
							{
								InquilinoId = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							},

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT PagoId, Numero, Fecha, Importe, p.AlquilerId, " +
					"a.Descripcion,a.Monto " +
					"FROM Pagos p INNER JOIN Alquiler a ON a.AlquilerId = p.AlquilerId " +
					" WHERE p.PagoId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							PagoId = reader.GetInt32(0),
							Numero = reader.GetString(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetString(3),
							AlquilerId = reader.GetInt32(4),
							alquiler = new Alquiler
							{
								Descripcion = reader.GetString(5),
								Monto = reader.GetString(6),
							}
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public IList<Pago> ObtenerPorAlquiler(string alquilerId)
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT PagoId, Numero, Fecha, Importe, p.AlquilerId, a.Descripcion,a.Monto FROM Pagos p, Alquileres a WHERE p.AlquilerId = a.AlquilerId and a.AlquilerId = @alquilerId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@alquilerId", SqlDbType.VarChar).Value = alquilerId;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{

						Pago p = new Pago
						{
							PagoId = reader.GetInt32(0),
							Numero = reader.GetString(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetString(3),
							AlquilerId = reader.GetInt32(4),
						};
						res.Add(p);

					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> BuscarPorNombre(string nombre)
		{
			List<Pago> res = new List<Pago>();
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $" SELECT p.PagoId, Numero, Fecha, Importe, p.AlquilerId " +
					$" FROM Pagos p, Alquileres a, Inquilinos i " +
					$" WHERE p.AlquilerId = a.AlquilerId and " +
					$"       a.InquilinoId = i.InquilinoId and " +
					$"       i.Nombre like @nombre";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Pago
						{
							PagoId = reader.GetInt32(0),
							Numero = reader.GetString(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetString(3),
							AlquilerId = reader.GetInt32(4),
							alquiler = new Alquiler
							{
								Monto = reader.GetString(5),
								Descripcion = reader.GetString(6),
							},
							inquilino = new Inquilino
							{
								InquilinoId = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							},

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}




		public IList<Pago> ObtenerInmueblePorDni(string dni)
		{
			IList<Pago> res = new List<Pago>();
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $" SELECT al.Monto, al.Descripcion, al.FechaAlta, al.InmuebleId, " +
					$" iq.InquilinoId, iq.Nombre, iq.Apellido, iq.Dni, iq.Telefono " +
					$" FROM Alquileres al  INNER JOIN Inquilinos iq ON al.InquilinoId = iq.inquilinoId" +
					$" WHERE iq.Dni = @dni";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@dni", SqlDbType.VarChar).Value = dni;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Pago
						{
							PagoId = reader.GetInt32(0),
							Numero = reader.GetString(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetString(3),
							AlquilerId = reader.GetInt32(4),
							alquiler = new Alquiler
							{
								Monto = reader.GetString(5),
								Descripcion = reader.GetString(6),
								FechaAlta = reader.GetDateTime(7),
								InmuebleId = reader.GetInt32(8),
							},
							inquilino = new Inquilino
							{
								InquilinoId = reader.GetInt32(9),
								Nombre = reader.GetString(10),
								Apellido = reader.GetString(11),
                                Dni = reader.GetString(12),
                                Telefono = reader.GetString(13),
                            },
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Pago ObtenerNumeroDePagoPorIdAlquiler(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Numero FROM Pagos where AlquilerId = @id order by Numero desc";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							Numero = reader.GetString(0),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

	}
}
