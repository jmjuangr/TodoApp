using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;

namespace AporopoApi.Data
{
    public static class TxtProcesador
    {
        private static readonly string tareasRutaArchivo = "Data/tareas.txt";
        private static readonly string usuariosRutaArchivo = "Data/usuarios.txt";

        // Leer Tareas
        public static List<TareaItem> LeerTareas()

        {
                var lineas = File.ReadAllLines(tareasRutaArchivo)
                     .Where(linea => !string.IsNullOrWhiteSpace(linea)) // Ignorar líneas vacías
                     .ToList();
            if (!File.Exists(tareasRutaArchivo)) return new List<TareaItem>();
            
            List<TareaItem> taskList = new List<TareaItem>();
            foreach (string linea in lineas)
            {
                taskList.Add(LineaTarea(linea));
            }
            return taskList;
        }

        //Crear Tareas
        public static void CrearTareas(List<TareaItem> tareas)
        {
            List<string> lineas = new List<string>();
            foreach (TareaItem tarea in tareas)
            {
                lineas.Add(TareaLinea(tarea));
            }
            File.WriteAllLines(tareasRutaArchivo, lineas);
        }

        // Convertir la línea de texto en un objeto TareaItem
        private static TareaItem LineaTarea(string linea)
        {
            var parts = linea.Split('|');
            return new TareaItem
            {
                TareaId = int.Parse(parts[0]),
                Titulo = parts[1],
                Descripcion = parts[2],
                Fecha = DateTime.Parse(parts[3]),
                Estado = bool.Parse(parts[4]),
                Prioridad = parts[5],
            };
        }

        // Convierte un objeto TareaItem en una línea de texto
        private static string TareaLinea(TareaItem tarea)
        {
            return $"{tarea.TareaId}|{tarea.Titulo}|{tarea.Descripcion}|{tarea.Fecha:yyyy-MM-ddTHH:mm:ss}|{tarea.Estado}|{tarea.Prioridad}";
        }

        // Leer todos los usuarios
        public static List<Usuario> LeerUsuarios()
        {
            if (!File.Exists(usuariosRutaArchivo)) return new List<Usuario>();
            
            var lineas = File.ReadAllLines(usuariosRutaArchivo);
            List<Usuario> userList = new List<Usuario>();
            foreach (string linea in lineas)
            {
                userList.Add(LineaUsuario(linea));
            }
            return userList;
        }

        // Crear usuarios
        public static void CrearUsuarios(List<Usuario> usuarios)
        {
            List<string> lineas = new List<string>();
            foreach (Usuario usuario in usuarios)
            {
                lineas.Add(UsuarioLinea(usuario));
            }
            File.WriteAllLines(usuariosRutaArchivo, lineas);
        }

        // Convertir una línea de texto en un objeto USuaro
        private static Usuario LineaUsuario(string linea)
        {
            var parts = linea.Split('|');
            return new Usuario
            {
                Id = int.Parse(parts[0]),
                NombreUsuario = parts[1],
                Password = parts[2]
            };
        }

        // Convertir un objeto Usuario en una línea de texto
        private static string UsuarioLinea(Usuario usuario)
        {
            return $"{usuario.Id}|{usuario.NombreUsuario}|{usuario.Password}";
        }
    }
}

