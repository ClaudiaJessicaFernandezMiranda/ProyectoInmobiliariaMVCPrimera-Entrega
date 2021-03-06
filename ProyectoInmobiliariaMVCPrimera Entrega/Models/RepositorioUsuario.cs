﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class RepositorioUsuario
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioUsuario(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public int Alta(Usuario u)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Usuarios (Nombre, Apellido, Email, Clave, Rol) " +
                    $"VALUES (@nombre, @apellido, @email, @clave, @rol);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", u.Nombre);
                    command.Parameters.AddWithValue("@apellido", u.Apellido);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@clave", u.Clave);
                    command.Parameters.AddWithValue("@rol", u.Rol);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    u.UsuarioId = res;
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
                string sql = $"DELETE FROM Usuarios WHERE UsuarioId = @id";
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
        public int Modificacion(Usuario u)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Usuarios SET Nombre=@nombre, Apellido=@apellido, Email=@email, Clave=@clave, Rol=@rol " +
                    $"WHERE UsuarioId = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", u.Nombre);
                    command.Parameters.AddWithValue("@apellido", u.Apellido);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@clave", u.Clave);
                    command.Parameters.AddWithValue("@rol", u.Rol);
                    command.Parameters.AddWithValue("@id", u.UsuarioId);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario u = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT UsuarioId, Nombre, Apellido, Email, Clave, Rol FROM Usuarios" +
                    $" WHERE Email=@email";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            UsuarioId = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetInt32(5),
                        };
                    }
                    connection.Close();
                }
            }
            return u;
        }
        //Modificando
        public IList<Usuario> ObtenerTodos()
        {
            IList<Usuario> res = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT UsuarioId, Nombre, Apellido, Email, Clave, Rol" +
                    $" FROM Usuarios";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario u = new Usuario
                        {
                            UsuarioId = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetInt32(5),
                        };
                        res.Add(u);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public Usuario ObtenerPorId(int id)
        {
            Usuario u = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT UsuarioId, Nombre, Apellido, Email, Clave, Rol FROM Usuarios" +
                    $" WHERE UsuarioId=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        u = new Usuario
                        {
                            UsuarioId = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetInt32(5),
                        };
                    }
                    connection.Close();
                }
            }
            return u;
        }
        //public Usuario ObtenerPorRol(string rol)
        //{
        //    Usuario u = null;
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string sql = $"SELECT UsuarioId, Nombre, Apellido, Email, Clave, Rol FROM Usuarios" +
        //            $" WHERE Rol=@rol";
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            command.CommandType = CommandType.Text;
        //            command.Parameters.Add("@rol", SqlDbType.VarChar).Value = rol;
        //            connection.Open();
        //            var reader = command.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                u = new Usuario
        //                {
        //                    UsuarioId = reader.GetInt32(0),
        //                    Nombre = reader.GetString(1),
        //                    Apellido = reader.GetString(2),
        //                    Email = reader.GetString(3),
        //                    Clave = reader.GetString(4),
        //                    Rol = reader.GetString(5),
        //                };
        //            }
        //            connection.Close();
        //        }
        //}
        // return u;
        //}
    }
}
