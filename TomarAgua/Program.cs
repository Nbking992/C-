using System;
using System.Windows.Forms;
using System.Drawing;
using Accessibility;
using System.Drawing.Text;
using System.CodeDom;
using System.Collections.Generic;

class Ventana_Basica : Form
{
    public Ventana_Basica()
    {
        // Configurar ventana
        this.Text = "Recordatorio para tomar agua";
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
        this.Width = 400;
        this.Height = 50;  
        this.BackColor = Color.White;
        this.Text = "¡Recuerde tomar agua!";
        this.Font = new Font("Arial", 16);
        this.TextAlign = HorizontalAlignment.Center;
        //this.ReadOnly = true;
        this.BorderStyle = BorderStyle.FixedSingle;

        // Agregar el TextBox a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

}

class Label_Basico : Label
{
    private Ventana_Basica ventana;

    public Label_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Text = "Tiempo desde la última vez que tome agua:";    
        this.Height = 30;  
        this.Font = new Font("Arial", 20);
        this.TextAlign = ContentAlignment.MiddleCenter;
        this.AutoSize = true;
        this.Width = TextRenderer.MeasureText(this.Text, this.Font).Width;

        // Posicionarse inicialmente
        Reposicionar();

        // Agregar el Label a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

    private void Reposicionar()
    {
        this.Left = 25;
        this.Top = ventana.ClientSize.Height / 2; // Justo encima del TextBox
    }
}

// ListBox para guardar las marcas y las cantidades del txtbox
class ListBox_Basico : ListBox
{
    private Ventana_Basica ventana;

    public ListBox_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Width = 300;
        this.Height = 100;  
        this.BackColor = Color.White;
        this.Font = new Font("Arial", 12);
        this.BorderStyle = BorderStyle.FixedSingle;

        // Posicionarse inicialmente
        this.Left = 10;
        this.Top = 10; // Debajo del TextBox

        // Agregar el ListBox a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }
}

class Program
{
    static int segundos = 0;

    static void Cronometro(Timer_Basico timer, Label_Basico label)
    {
        int minutos = 0;
        int resto = 0;
        int horas = 0;

        timer.Interval = 1000; // 1 segundo
        timer.Tick += (s, e) =>
        {
            horas = (segundos / 3600) % 24;
            minutos = (segundos / 60) % 60;
            resto = segundos % 60;
            label.Text = $"{horas:D2}:{minutos:D2}:{resto:D2}";
            segundos++;
        };
        timer.Start();
    }
    static void ReposicionarControles(Ventana_Basica ventana, TextBox_Basico textBox, Boton_Basico boton, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        // Ajustar tamaño de la ventana según el texto
        ventana.ClientSize = new Size(TextRenderer.MeasureText(label.Text, label.Font).Width + 70, ventana.ClientSize.Height);
        //textBox.Width = new Size(TextRenderer.MeasureText(textBox.Text, textBox.Font).Width + 20, textBox.Height).Width;

        // Reposicionar controles
        cronometro.Left = (ventana.ClientSize.Width - cronometro.Width) / 2;
        textBox.Top = cronometro.Top + cronometro.Height + 20;
        textBox.Left = (ventana.ClientSize.Width - textBox.Width) / 2;
        boton.Top = textBox.Top + textBox.Height + 20;
        boton.Left = (ventana.ClientSize.Width - boton.Width) / 2;
        listBox.Left = (ventana.ClientSize.Width - listBox.Width) / 2;
        listBox.Top = boton.Top + boton.Height + 20;

    }
    static void Reiniciar(Boton_Basico boton, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {

        segundos = 0; // Reiniciar contador al hacer clic
        cronometro.Text = "00:00";
        textBox.Text = "";
        ventana.BackColor = Color.LightBlue; // Restaurar color original
        timer.Stop();
        timer.Start();

        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, boton, label, cronometro,listBox);
    }
    static void Marcar(Boton_Basico boton, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        boton.Click += (s, e) =>
        {
            //label.Text += $" {cronometro.Text}"; // Mantener el texto del cronómetro
            listBox.Items.Add($" {cronometro.Text} - {textBox.Text}");
            textBox.Text = "";
            ventana.BackColor = Color.LightGreen;
            //ventana.BackColor == Color.LightGreen ? Color.LightBlue : Color.LightGreen
            //Reiniciar(boton, ventana, textBox, timer, label, cronometro, listBox);
           
            // Ajustar tamaño de la ventana según el texto
            ReposicionarControles(ventana, textBox, boton, label, cronometro,listBox);
        };
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
        ListBox_Basico listBox = new(ventana);


        // Configurar cronómetro y reinicio
        //label.Top = label.Top - 100;
        //cronometro.Top = label.Top + 40;
        label.Top = 20;
        cronometro.Top = label.Top + label.Height + 10;

        cronometro.Text = "00:00";
        listBox.Items.Add("Marcas de agua tomadas:");

        // Iniciar cronómetro y reinicio
        Cronometro(timer, cronometro);
        ReposicionarControles(ventana, textBox, boton, label, cronometro, listBox);
        Marcar(boton, ventana, textBox, timer, label, cronometro, listBox);
        // Mostrar ventana
        Application.Run(ventana);
    }
}

