using System;
using System.Windows.Forms;
using System.Drawing;
using Accessibility;
using System.Drawing.Text;
using System.CodeDom;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;


public enum Typo_De_Boton
{
    Basico,
    Inicio,
    Reseto
}
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
    public EventHandler Iniciar_timer;
    public EventHandler Reinicio;
    public EventHandler Marcar;
    private void Basico_Boton_Click(object sender, EventArgs e)
    {
        Marcar?.Invoke(this, e);
        ventana.BackColor = Color.LightGreen;
    }
    private void Inicio_Boton_Click(object sender, EventArgs e)
    {
        // Creacion de un event iniciar_timer
        Iniciar_timer?.Invoke(this, e);
        ventana.BackColor = Color.LightYellow;
    }
    private void Reseto_Boton_Click(object sender, EventArgs e)
    {
        // Acción al hacer clic en el botón "Reiniciar"
        Reinicio?.Invoke(this, e);
        ventana.BackColor = Color.LightBlue;
    }

    public Boton_Basico(Ventana_Basica ventana, Typo_De_Boton typo)
    {
        this.ventana = ventana;
        switch (typo)
        {
            case Typo_De_Boton.Basico:
                this.Text = "Tome Agua";
                this.Width = 100;
                this.Height = 50;
                this.BackColor = Color.LightCoral;
                this.Click += Basico_Boton_Click;
                break;
            case Typo_De_Boton.Inicio:
                this.Width = 100;
                this.Height = 50;
                this.BackColor = Color.LightPink;
                this.Text = "Iniciar";
                this.Click += Inicio_Boton_Click;
                break;
            case Typo_De_Boton.Reseto:
                this.Width = 100;
                this.Height = 50;
                this.BackColor = Color.LightSteelBlue;
                this.Text = "Reiniciar";
                this.Click += Reseto_Boton_Click;
                break;
            default:
                this.Text = "Marcar";
                break;
        }


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
        this.Top = ventana.ClientSize.Height - this.Height - 10;
    }
    
}

class Timer_Basico : Timer
{
    private Ventana_Basica ventana;

    public Timer_Basico(Ventana_Basica ventana)
    {
        this.ventana = ventana;
        this.Interval = 1000; // 1 segundos
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

        timer.Tick += (s, e) =>
        {
            minutos = segundos / 60;
            resto = segundos % 60;
            label.Text = $"{minutos:D2}:{resto:D2}";
            segundos++;
        };
    }
    
    static void ReposicionarControles(Ventana_Basica ventana, TextBox_Basico textBox, List<Boton_Basico> botones, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        // Ajustar tamaño de la ventana según el texto
        ventana.ClientSize = new Size(TextRenderer.MeasureText(label.Text, label.Font).Width + 70, ventana.ClientSize.Height);
        //textBox.Width = new Size(TextRenderer.MeasureText(textBox.Text, textBox.Font).Width + 20, textBox.Height).Width;

        // Reposicionar controles
        cronometro.Left = (ventana.ClientSize.Width - cronometro.Width) / 2;
        textBox.Top = cronometro.Top + cronometro.Height + 20;
        textBox.Left = (ventana.ClientSize.Width - textBox.Width) / 2;
        for (int i = 0; i < botones.Count; i++)
        {
            botones[i].Top = textBox.Top + textBox.Height + 20;
            botones[i].Left = (ventana.ClientSize.Width - (botones.Count * botones[i].Width + (botones.Count - 1) * 10)) / 2 + i * (botones[i].Width + 10);
        }
        listBox.Left = (ventana.ClientSize.Width - listBox.Width) / 2;
        listBox.Top = botones[0].Top + botones[0].Height + 20;

    }
    static void Reiniciar(List<Boton_Basico> botones, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {

        timer.Stop();
        segundos = 0; // Reiniciar contador al hacer clic
        cronometro.Text = "00:00";
        textBox.Text = "";
        ventana.BackColor = Color.LightBlue; // Restaurar color original

        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, botones, label, cronometro,listBox);
    }
    
    static void Marcar(List<Boton_Basico> botones, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        // Acción al hacer clic en el botón "Tome Agua"
        // Mantener el texto del cronómetro
        listBox.Items.Add($" {cronometro.Text} - {textBox.Text}");
        textBox.Text = "";
        ventana.BackColor = Color.LightGreen;

        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, botones, label, cronometro, listBox);
    }
    
    static void Iniciar(List<Boton_Basico> botones, Ventana_Basica ventana, TextBox_Basico textBox, Timer_Basico timer, Label_Basico label, Label_Basico cronometro, ListBox_Basico listBox)
    {
        // Acción al hacer clic en el botón "Iniciar"
        timer.Start();

        // Ajustar tamaño de la ventana según el texto
        ReposicionarControles(ventana, textBox, botones, label, cronometro, listBox);
    }

    static void Main()
    {
        // Crear ventana y controles
        Ventana_Basica ventana = new();
        Timer_Basico timer = new(ventana);
        TextBox_Basico textBox = new(ventana);
        Label_Basico label = new(ventana);
        Label_Basico cronometro = new(ventana);
        ListBox_Basico listBox = new(ventana);
        Boton_Basico botonBasico = new(ventana, Typo_De_Boton.Basico);
        Boton_Basico botonInicio = new(ventana, Typo_De_Boton.Inicio);
        Boton_Basico botonReset = new(ventana, Typo_De_Boton.Reseto);
        List<Boton_Basico> botones = new List<Boton_Basico> { botonBasico, botonInicio, botonReset };
        botonInicio.Iniciar_timer += (s, e) => Iniciar(botones, ventana, textBox, timer, label, cronometro, listBox);
        botonReset.Reinicio += (s, e) => Reiniciar(botones, ventana, textBox, timer, label, cronometro, listBox);
        botonBasico.Marcar += (s, e) => Marcar(botones, ventana, textBox, timer, label, cronometro, listBox);


        // Configurar cronómetro y reinicio
        label.Top = 20;
        cronometro.Top = label.Top + label.Height + 10;

        cronometro.Text = "00:00";
        listBox.Items.Add("Marcas de agua tomadas:");

        // Iniciar cronómetro y reinicio
        Cronometro(timer, cronometro);
        ReposicionarControles(ventana, textBox, botones, label, cronometro, listBox);
        Marcar(botones, ventana, textBox, timer, label, cronometro, listBox);
        // Mostrar ventana
        Application.Run(ventana);
    }
}

