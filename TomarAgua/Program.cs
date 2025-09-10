using System;
using System.Windows.Forms;
using System.Drawing;

class Ventana_Basica : Form
{
    public Ventana_Basica()
    {
        // Configurar ventana
        this.Text = "Ventana con Botón Inteligente";
        this.Width = 400;
        this.Height = 400;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.LightBlue;
    }
}

class Boton_Basico : Button
{
    private Form ventana;

    public Boton_Basico(Form ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Text = "Tome Agua";
        this.Width = 100;
        this.Height = 50;
        this.BackColor = Color.LightCoral;

        // Evento click → cambia el color de la ventana
        this.Click += (sender, e) =>
        {
            ventana.BackColor = Color.LightGreen;
        };

        // Suscribirse al evento Resize de la ventana
        ventana.Resize += (sender, e) => Reposicionar();

        // Posicionarse inicialmente
        Reposicionar();

        // Agregar el botón a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

    private void Reposicionar()
    {
        //int margenInferior = (int)(ventana.ClientSize.Height * 0.10);
        this.Left = 10; // pegado al borde izquierdo
        this.Top = ventana.ClientSize.Height  - this.Height - 10;
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
