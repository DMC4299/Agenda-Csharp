using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Contacto
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }

    // Constructor para inicializar propiedades
    public Contacto(string nombre, string telefono, string correo)
    {
        Nombre = nombre;
        Telefono = telefono;
        Correo = correo;
    }
}

class Agenda
{
    private List<Contacto> contactos = new List<Contacto>();
    private const string archivo = "agenda.json";

    public Agenda()
    {
        CargarContactos();
    }

    public void MostrarMenu()
    {
        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== Agenda de Contactos ===");
            Console.WriteLine("1. Agregar Contacto");
            Console.WriteLine("2. Mostrar Contactos");
            Console.WriteLine("3. Actualizar Contacto");
            Console.WriteLine("4. Eliminar Contacto");
            Console.WriteLine("5. Salir");
            Console.Write("Elige una opción: ");
            string opcion = Console.ReadLine() ?? string.Empty;

            switch (opcion)
            {
                case "1":
                    AgregarContacto();
                    break;
                case "2":
                    MostrarContactos();
                    break;
                case "3":
                    ActualizarContacto();
                    break;
                case "4":
                    EliminarContacto();
                    break;
                case "5":
                    GuardarContactos();
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
            if (continuar)
            {
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private string LeerCadenaNoVacia(string mensaje)
    {
        string valor;
        do
        {
            Console.Write(mensaje);
            valor = Console.ReadLine() ?? string.Empty;
        } while (string.IsNullOrWhiteSpace(valor));
        return valor;
    }

    private void AgregarContacto()
    {
        string nombre = LeerCadenaNoVacia("Nombre: ");
        string telefono = LeerCadenaNoVacia("Teléfono: ");
        string correo = LeerCadenaNoVacia("Correo: ");
        contactos.Add(new Contacto(nombre, telefono, correo));
        Console.WriteLine("Contacto agregado exitosamente.");
    }

    private void MostrarContactos()
    {
        Console.WriteLine("\nLista de Contactos:");
        if (contactos.Count == 0)
        {
            Console.WriteLine("No hay contactos registrados.");
        }
        else
        {
            var listaContactos = contactos.Select((contacto, index) => $"{index + 1}. {contacto.Nombre} - {contacto.Telefono} - {contacto.Correo}");
            Console.WriteLine(string.Join("\n", listaContactos));
        }
    }

    private void ActualizarContacto()
    {
        MostrarContactos();
        Console.Write("\nIngresa el número del contacto a actualizar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= contactos.Count)
        {
            Console.Write("Nuevo nombre: ");
            contactos[indice - 1].Nombre = Console.ReadLine() ?? string.Empty;
            Console.Write("Nuevo teléfono: ");
            contactos[indice - 1].Telefono = Console.ReadLine() ?? string.Empty;
            Console.Write("Nuevo correo: ");
            contactos[indice - 1].Correo = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Contacto actualizado.");
        }
        else
        {
            Console.WriteLine("Índice no válido.");
        }
    }

    private void EliminarContacto()
    {
        MostrarContactos();
        Console.Write("\nIngresa el número del contacto a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= contactos.Count)
        {
            contactos.RemoveAt(indice - 1);
            Console.WriteLine("Contacto eliminado.");
        }
        else
        {
            Console.WriteLine("Índice no válido.");
        }
    }

    private void CargarContactos()
    {
        if (File.Exists(archivo))
        {
            try
            {
                string json = File.ReadAllText(archivo);
                contactos = JsonSerializer.Deserialize<List<Contacto>>(json) ?? new List<Contacto>();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error al procesar el archivo JSON: " + ex.Message);
            }
        }
    }

    private void GuardarContactos()
    {
        try
        {
            string json = JsonSerializer.Serialize(contactos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivo, json);
            Console.WriteLine("Contactos guardados correctamente.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al guardar el archivo: " + ex.Message);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Agenda agenda = new Agenda();
        agenda.MostrarMenu();
    }
}
