// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

class Program
{
    static List<string> nombres = new List<string>();
    static List<decimal> precios = new List<decimal>();
    static List<int> stocks = new List<int>();
    static List<int> unidadesVendidas = new List<int>();

    static int totalVentas = 0;
    static decimal totalCaja = 0;

    static void Main()
    {
        int opcion;
        do
        {
            ImprimirEncabezado("SISTEMA GESTOR DE VENTAS E INVENTARIO ");
            Console.WriteLine("| 1. Registrar nuevo producto en inventario         |");
            Console.WriteLine("| 2. Consultar inventario completo                  |");
            Console.WriteLine("| 3. Registrar una venta                            |");
            Console.WriteLine("| 4. Ver reporte de caja y estadisticas diarias     |");
            Console.WriteLine("| 5. Salir                                          |");
            Console.WriteLine("=====================================================");
            Console.WriteLine();
            opcion = LeerEntero("Seleccione una opcion (1-5): ", 1, 5);

            switch (opcion)
            {
                case 1:
                    RegistrarProducto();
                    break;

                case 2:
                    ConsultarInventario();
                    break;

                case 3:
                    RegistrarVenta();
                    break;

                case 4:
                    MostrarReporte();
                    break;

                case 5:
                    ImprimirEncabezado("SALIENDO...");
                    Console.WriteLine("Gracias por utilizar el sistema!");
                    break;
            }

            if (opcion != 5)
            {
                Console.WriteLine("presione ENTER para continuar");
                Console.ReadLine();
            }

        } while (opcion != 5);
    }


    //Lectura enteros de manera segura
    static int LeerEntero(string mensaje, int min, int max)
    {
        int valor;
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine()!;

            if (int.TryParse(entrada, out valor))
            {
                if (valor >= min && valor <= max)
                {
                    return valor;
                }
                else
                {
                    Console.WriteLine($"[ERROR] Opcion fuera de rango. Ingrese un valor entre {min} y {max}.");
                }
            }
            else
            {
                Console.WriteLine("[ERROR] Entrada no valida. Debe ingresar un numero entero.");
            }
        }
    }


    // Lectura decimales demanera segura
    static decimal LeerDecimal(string mensaje, decimal min)
    {
        decimal valor;

        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine()!;

            if (decimal.TryParse(entrada, out valor))
            {
                if (valor >= min)
                {
                    return valor;
                }
                else
                {
                    Console.WriteLine($"[ERROR] El valor debe ser mayor o igual a {min}.");
                }
            }
            else
            {
                Console.WriteLine("[ERROR] Entrada no valida. Debe ingresar un numero decimal.");
            }
        }
    }


    //Calculo de factura
    static decimal CalcularFactura(decimal precio, int cantidad, bool tieneDescuento, out decimal montoIva, out decimal montoDescuento)
    {
        decimal subtotal = precio * cantidad;

        if (tieneDescuento)
        {
            montoDescuento = subtotal * 0.10m;
        }
        else
        {
            montoDescuento = 0;
        }
        montoIva = (subtotal - montoDescuento) * 0.19m;
        decimal total = subtotal - montoDescuento + montoIva;
        return total;
    }

    // Encabezados (metodos estaticos)
    static void ImprimirEncabezado(string titulo)
    {
        Console.WriteLine("========================================================");
        Console.WriteLine($"|              {titulo}              |");
        Console.WriteLine("========================================================");
    }

    // Registro de productos
    static void RegistrarProducto()
    {
        ImprimirEncabezado("REGISTRAR NUEVO PRODUCTO");
        string nombre;
        while (true)
        {
            Console.Write("Ingrese el nombre del producto: ");
            nombre = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("[ERROR] El nombre del producto no puede estar vacio.");
                continue;
            }
            bool existe = false;

            for (int i = 0; i < nombres.Count; i++)
            {
                if (nombres[i].Equals(nombre, StringComparison.OrdinalIgnoreCase)) //no importa mayusculas o minusculas
                {
                    existe = true;
                    break;
                }
            }
            if (existe)
            {
                Console.WriteLine("[ERROR] Ya existe un producto con ese nombre.");
            }
            else
            {
                break;
            }
        }

        decimal precio = LeerDecimal("Ingrese el precio unitario ($): ", 0.01m);
        int stock = LeerEntero("Ingrese el stock inicial: ", 0, 1000000);

        nombres.Add(nombre);
        precios.Add(precio);
        stocks.Add(stock);
        unidadesVendidas.Add(0);
        Console.WriteLine();
        Console.WriteLine("[OK] Producto registrado correctamente.");
    }
    // Consultar inventario
    static void ConsultarInventario()
    {
        ImprimirEncabezado("INVENTARIO COMPLETO");
        if (nombres.Count == 0)
        {
            Console.WriteLine("No hay productos registrados en el inventario.");
            return;
        }
        Console.WriteLine(
            $"{"|ID",-5} {"|PRODUCTO",-25} {"|PRECIO",-15} {"|STOCK",-10}");

        Console.WriteLine(
            "--------------------------------------------------------------------------");

        for (int i = 0; i < nombres.Count; i++)
        {
            string alerta = "";
            if (stocks[i] < 5)
            {
                alerta = " [ALERTA: BAJO STOCK]";
            }
            Console.WriteLine( $"|{i + 1,-5} |{nombres[i],-25} |" + $"{precios[i],-15:C2} |{stocks[i],-10}{alerta}");
        }
    }

    // Registrar venta
    static void RegistrarVenta()
    {
        ImprimirEncabezado("REGISTRAR VENTA");
        if (nombres.Count == 0)
        {
            Console.WriteLine("[No hay productos registrados]");
            return;
        }
        Console.WriteLine("Productos disponibles:");
        Console.WriteLine();

        for (int i = 0; i < nombres.Count; i++)
        {
            string alerta = "";
            if (stocks[i] < 5)
            {
                alerta = " [ALERTA: BAJO STOCK]";
            }
            Console.WriteLine($"{i + 1}. {nombres[i]} | " +  $"Precio: {precios[i]:C2} | " + $"Stock: {stocks[i]}{alerta}");
        }
        Console.WriteLine();

        int producto = LeerEntero($"Seleccione el numero del producto a vender (1-{nombres.Count}): ",1,nombres.Count);
        int indice = producto - 1;
        int cantidad;

        while (true)
        {
            cantidad = LeerEntero("Ingrese la cantidad a comprar: ",1, 1000000);

            if (cantidad > stocks[indice])
            {
                Console.WriteLine($"[ERROR] Stock insuficiente. " + $"Solo quedan {stocks[indice]} unidades en inventario.");
            }
            else
            {
                break;
            }
        }
        bool descuento = false;
        while (true)
        {
            Console.Write("¿Aplica descuento de cliente frecuente (10%)? (S/N): ");
            string respuesta = Console.ReadLine()!;
            if (respuesta.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                descuento = true;
                break;
            }
            else if (respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                descuento = false;
                break;
            }
            else
            {
                Console.WriteLine(
                    "[ERROR] Responda solamente S o N.");
            }
        }

        decimal subtotal = precios[indice] * cantidad;
        decimal montoIva;
        decimal montoDescuento;

        decimal total = CalcularFactura(precios[indice], cantidad, descuento, out montoIva, out montoDescuento);

        // Actualizar inventario
        stocks[indice] -= cantidad;

        // Actualizar ventas
        unidadesVendidas[indice] += cantidad;

        totalVentas++;
        totalCaja += total;

        // Ticket
        Console.WriteLine();
        ImprimirEncabezado("TICKET DE VENTA");

        Console.WriteLine(
            $"Producto:             {nombres[indice]} (x{cantidad})");

        Console.WriteLine(
            $"Subtotal:             {subtotal:C2}");

        Console.WriteLine(
            $"Descuento (10%):     -{montoDescuento:C2}");

        Console.WriteLine(
            $"IVA (19%):            +{montoIva:C2}");

        Console.WriteLine(
            "----------------------------------------------------");

        Console.WriteLine(
            $"TOTAL A PAGAR:        {total:C2}");

        Console.WriteLine(
            "====================================================");

        Console.WriteLine(
            $"[OK] Venta efectuada con exito. " +
            $"Stock actualizado: {stocks[indice]} unidades.");
    }
    // Reporte de caja
    static void MostrarReporte()
    {
        ImprimirEncabezado("REPORTE DE CAJA Y ESTADISTICAS DIARIAS");
        Console.WriteLine($"Total de ventas realizadas: {totalVentas}");
        Console.WriteLine($"Total acumulado en caja:{totalCaja:C2}");
        decimal promedio = 0;
        if (totalVentas > 0)
        {
            promedio = totalCaja / totalVentas;
        }
        Console.WriteLine( $"Promedio por venta: {promedio:C2}");
        Console.WriteLine();
        if (nombres.Count == 0)
        {
            Console.WriteLine("Producto con mayor cantidad de unidades vendidas: Ninguno");
            return;
        }
        int mayor = 0;
        int indiceMayor = 0;
        for (int i = 0; i < unidadesVendidas.Count; i++)
        {
            if (unidadesVendidas[i] > mayor)
            {
                mayor = unidadesVendidas[i];
                indiceMayor = i;
            }
        }
        if (mayor == 0)
        {
            Console.WriteLine("Producto con mayor cantidad de unidades vendidas: Ninguno");
        }
        else
        {
            Console.WriteLine($"Producto con mayor cantidad de unidades vendidas: " + $"{nombres[indiceMayor]} ({mayor} unidades)");
        }
    }
}