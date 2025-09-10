using System;
using System.Windows.Forms;
using System.Drawing;

class Ventana_Basica : Form
{
    public Ventana_Basica()
    {
        // Configurar tamaño y título
        this.Text = "Mi Ventana 400x400";
        this.Width = 400;
        this.Height = 400;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.LightBlue;
    }
}

class Boton_Basico
{
    public Button boton;

    public Boton_Basico(Form ventana)
    {
        // Crear el botón
        boton = new Button();

        // Configurar tamaño y posición
        boton.Text = "Tome Agua";
        boton.Width = 100;
        boton.Height = 50;
        boton.Top = boton.Height;
        boton.Left = boton.Width;

        // Configurar color de fondo
        boton.BackColor = Color.LightCoral;

        // Agregar evento click
        boton.Click += (sender, e) =>
        {
            ventana.BackColor = Color.LightGreen;
        };

        // Agregar botón a la ventana
        ventana.Controls.Add(boton);
    }
}

class Program
{
    static void Main()
    {
        Ventana_Basica ventana = new Ventana_Basica();
        Boton_Basico boton = new Boton_Basico(ventana);

        // Mostrar ventana
        Application.Run(ventana);
    }
}
