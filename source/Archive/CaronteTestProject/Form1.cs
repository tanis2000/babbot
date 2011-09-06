using System;
using System.Windows.Forms;


namespace CaronteTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Caronte.Caronte caronte = new Caronte.Caronte();
            caronte.Init("Expansion01");

            Pather.Graph.Path path = caronte.CalculatePath(new Pather.Graph.Location(240.94f, 2692.39f, 89.74f),
                                                           new Pather.Graph.Location(189.63f, 2690.94f, 88.71f));

            foreach (Pather.Graph.Location loc in path.locations)
            {
                Console.WriteLine("X: {0}\t\tY: {1}\t\tZ: {2}", loc.X, loc.Y, loc.Z);
            }
        }
    }
}
