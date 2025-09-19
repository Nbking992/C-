using System;
using System.Windows.Forms;
using System.Drawing;
using Accessibility;
using System.Drawing.Text;
using System.CodeDom;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

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
        this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evitar redimensionamiento
        this.MaximizeBox = false; // Deshabilitar botón de maximizar
    }
}

class Boton_Basico : Button
{
    private Ventana_Basica ventana;

    // Propieda nueva (Cosas para el diseñador de windows forms que no tengo en Code)
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // Propieda nueva
    public int UltimaVez { get; set; } = 0;

    public Boton_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;

        // Configuración visual
        this.Text = "Tome Agua";
        this.Width = 100;
        this.Height = 50;
        this.BackColor = Color.LightCoral;

        // Agregar el botón a la ventana (se mantiene en Main)
        ventana.Controls.Add(this);
    }

}

class Timer_Basico : Timer
{
    private Ventana_Basica ventana;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Segundos { get; set; } = 0;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Minutos { get; set; } = 0;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Horas { get; set; } = 0;

    public Timer_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;
        this.Interval = 1000; // 1seg
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

class Hora
{
    public int TotalSegundos { get; set; } = 0;

    public int Horas => (TotalSegundos / 3600) % 24;
    public int Minutos => (TotalSegundos / 60) % 60;
    public int Segundos => TotalSegundos % 60;
}

class Program
{
    static int segundos = 0;

    static void Cronometro(Timer_Basico timer, Label_Basico label)
    {
        var tmp = new Hora
        {
            TotalSegundos = segundos
        };

        label.Text = $"{tmp.Horas:D2}:{tmp.Minutos:D2}:{tmp.Segundos:D2}";
        segundos++;
        tmp.TotalSegundos = segundos;
        timer.Segundos = segundos;
        timer.Minutos = tmp.Minutos;
        timer.Horas = tmp.Horas;
        
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
        boton.Left = (ventana.ClientSize.Width - boton.Width) / 2 - 50;

        listBox.Left = (ventana.ClientSize.Width - listBox.Width) / 2;
        listBox.Top = boton.Top + boton.Height + 20;
    }
    static void Reiniciar(Boton_Basico boton, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        if (segundos > 0)
        {
            segundos = 0; // Reiniciar contador al hacer clic
            cronometro.Text = "00:00:00";
            textBox.Text = "";
            ventana.BackColor = Color.LightBlue; // Restaurar color original
            timer.Stop();
            timer.Segundos = 0;
            boton.UltimaVez = 0;
        }
        else
        {
            // Iniciar cronómetro
            Cronometro(timer, cronometro);
        }
        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, boton, label, cronometro, listBox);
    }
    static void Marcar(Boton_Basico boton, Ventana_Basica ventana, TextBox_Basico textBox, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox, Timer_Basico timer)
    {
        // Calcular ultima vez
        if (boton.UltimaVez != 0)
        {
            int tiempoTranscurrido = timer.Segundos - boton.UltimaVez;
            var tmp = new Hora
            {
                TotalSegundos = tiempoTranscurrido
            };
            listBox.Items.Add($"{tmp.Horas:D2}:{tmp.Minutos:D2}:{tmp.Segundos:D2} - {textBox.Text}");
        }
        else
        {
            listBox.Items.Add("Tome por Primera vez");
        }

        // Redimensiono el listbox si es necesario
        int anchoTexto = TextRenderer.MeasureText(listBox.Items[listBox.Items.Count - 1].ToString(), listBox.Font).Width;
        if (anchoTexto > listBox.Width)
        {
            listBox.Width = anchoTexto + 20; // Añadir un poco de margen
        }

        //listBox.Items.Add($" {cronometro.Text} - {textBox.Text}");
        textBox.Text = "";
        ventana.BackColor = Color.LightGreen;
        boton.UltimaVez = timer.Segundos;

        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, boton, label, cronometro, listBox);
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
        Boton_Basico botonReiniciar = new(ventana);


        // Configurar cronómetro y reinicio
        cronometro.Text = "00:00:00";
        listBox.Items.Add("Marcas de agua tomadas:");

        // Posicionar controles
        label.Top = 20;
        cronometro.Top = label.Top + label.Height + 10;
        ReposicionarControles(ventana, textBox, boton, label, cronometro, listBox);

        botonReiniciar.Text = "Iniciar/Reiniciar";
        botonReiniciar.Width = boton.Width; 
        botonReiniciar.Height = boton.Height;
        botonReiniciar.Top = boton.Top;
        botonReiniciar.Left = boton.Left + boton.Width + 10;
        botonReiniciar.BackColor = Color.LightGray;

        // Eventos
        botonReiniciar.Click += (s, e) =>
        {
            Reiniciar(boton, ventana, textBox, timer, label, cronometro, listBox);
        };

        textBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evitar el sonido de "ding"
                Marcar(boton, ventana, textBox, label, cronometro, listBox, timer);
            }
        };

        boton.Click += (s, e) =>
        {
            Marcar(boton, ventana, textBox, label, cronometro, listBox, timer);
        };

        timer.Tick += (s, e) =>
        {
            Cronometro(timer, cronometro);
        };
        
        // Mostrar ventana
        Application.Run(ventana);
    }
}

