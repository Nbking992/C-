using System;
using System.Windows.Forms;
using System.Drawing;
using Accessibility;
using System.Drawing.Text;
using System.CodeDom;

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
    private Ventana_Basica ventana;

    public Boton_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Text = "Tome Agua";
        this.Width = 100;
        this.Height = 50;  
        this.BackColor = Color.LightCoral;
        

        // Evento click → cambia el color de la ventana
        /*this.Click += (sender, e) =>
        {
            ventana.BackColor = Color.LightGreen;
        };*/

        // Suscribirse al evento Resize de la ventana
        //ventana.Resize += (sender, e) => Reposicionar();

        // Posicionarse inicialmente
        //Reposicionar();

        // Agregar el botón a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

    private void Reposicionar()
    {
        this.Left = 10;
        this.Top = ventana.ClientSize.Height  - this.Height - 10;
    }
}

class Timer_Basico : Timer
{
    private Ventana_Basica ventana;

    public Timer_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;
        this.Interval = 1000; // 1 segundos
        //this.Tick += (sender, e) => CambiarColor();
        this.Start();
    }

    private void CambiarColor()
    {
        Random rand = new Random();
        ventana.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
    }
}

class TextBox_Basico : TextBox
{
     private Ventana_Basica ventana;

    public TextBox_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Width = 200;
        this.Height = 50;  
        this.BackColor = Color.White;
        this.Text = "¡Recuerde tomar agua!";
        this.Font = new Font("Arial", 16);
        this.TextAlign = HorizontalAlignment.Center;
        this.ReadOnly = true;
        this.BorderStyle = BorderStyle.FixedSingle;

        // Posicionarse inicialmente
        Reposicionar();

        // Agregar el TextBox a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

    private void Reposicionar()
    {
        this.Left = (ventana.ClientSize.Width - this.Width) / 2;
        this.Top = (ventana.ClientSize.Height - this.Height) / 2;
    }
}

class Label_Basico : Label
{
    private Ventana_Basica ventana;

    public Label_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Width = 300;
        this.Height = 30;  
        this.Text = "Tiempo desde la última vez que tomó agua:";
        this.Font = new Font("Arial", 20);
        this.TextAlign = ContentAlignment.MiddleCenter;
        this.AutoSize = true;

        // Posicionarse inicialmente
        Reposicionar();

        // Agregar el Label a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

    private void Reposicionar()
    {
        this.Left = (ventana.ClientSize.Width - this.Width) / 2;
        this.Top = ventana.ClientSize.Height / 2; // Justo encima del TextBox
    }
}

class Program
{
    static int segundos = 0;

    static void Cronometro(Timer_Basico timer, Label_Basico label) 
    {
        int minutos = 0;
        int resto = 0;

        timer.Interval = 1000; // 1 segundo
        timer.Tick += (s, e) =>
        {
            minutos = segundos / 60;
            resto = segundos % 60;
            label.Text = $"{minutos:D2}:{resto:D2}";
            segundos++;
        };
        timer.Start();
    }

    static void reiniciar(Boton_Basico boton, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro)
    {
        boton.Click += (s, e) =>
         {
             segundos = 0; // Reiniciar contador al hacer clic
             textBox.Text = "¡Gracias por tomar agua!";
             ventana.BackColor = ventana.BackColor == Color.LightGreen ? Color.LightBlue : Color.LightGreen; // Restaurar color original
             label.Text = label.Text + $" {cronometro.Text}"; // Mantener el texto del cronómetro
             timer.Stop();
             timer.Start();
             ventana.ClientSize = new Size(ventana.Width + TextRenderer.MeasureText(label.Text, label.Font).Width, ventana.ClientSize.Height);
         };

        // Ajustar tamaño de la ventana según el texto
        ventana.ClientSize = new Size(TextRenderer.MeasureText(label.Text, label.Font).Width, ventana.ClientSize.Height);
        textBox.Width = textBox.Width + TextRenderer.MeasureText(cronometro.Text, cronometro.Font).Width / 2;
       
        // Reposicionar controles
        cronometro.Left = (ventana.ClientSize.Width - cronometro.Width) / 2;
        textBox.Left = (ventana.ClientSize.Width - textBox.Width) / 2;
        boton.Top = textBox.Top + textBox.Height + 20;
        boton.Left = (ventana.ClientSize.Width - boton.Width) / 2;
    }

    static void Main()
    {
        // Crear ventana y controles
        Ventana_Basica ventana = new();
        Boton_Basico boton = new(ventana);
        Timer_Basico timer = new(ventana);
        TextBox_Basico textBox = new(ventana);
        Label_Basico label = new(ventana);
        Label_Basico cronometro = new(ventana);

        // Configurar cronómetro y reinicio
        label.Top = label.Top - 100;
        cronometro.Top = label.Top + 40;
        cronometro.Text = "00:00";

        // Iniciar cronómetro y reinicio
        Cronometro(timer, cronometro);
        reiniciar(boton, ventana, textBox, timer, label,cronometro);

        // Mostrar ventana
        Application.Run(ventana);
    }
}

