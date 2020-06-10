﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Entity;
using BLL;

namespace VeterinariaGUI
{
    public partial class Facturacioncs : Form
    {
        private ClienteService Clientes;
        private ServiciosService Servicios;
        private FacturaService Facturas;
        private Empleado Empleado;
        private Cliente Cliente;
      
      

        private Factura Factura;
        private DetalleFactura Detalle;
        private List<DetalleFactura> Detalles;
        private List<Servicio> servicios;
        private List<Servicio> seleccion;
        private MascotaService mascotas;

        public Facturacioncs(
            ClienteService _Clientes, 
            ServiciosService _Servicios, 
            FacturaService _Facturas, 
            Empleado _Empleado
            )             
        {
            seleccion = new List<Servicio>();
            Detalles = new List<DetalleFactura>();
            Factura = new Factura();
            Clientes = _Clientes;
            Servicios = _Servicios;
            Empleado = _Empleado;
            Facturas = _Facturas;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionRochety"].ConnectionString;
            Facturas = _Facturas;
            InitializeComponent();
            this.PreCharge();
        }

     

        public Facturacioncs(FacturaService facturas)
        {
            Facturas = facturas;
        }

        

        private void PreCharge()
        {
            textBox2.Text = this.Factura.PcjIva + "";
            textBox3.Text = this.Factura.PcjGanancia + "";
            textBox4.Text = this.Factura.PcjDescuento + "";
            label14.Text = this.Empleado.NombreCompleto();
            var Srta = this.Servicios.Consultar();
            if (Srta.Error)
                MessageBox.Show(Srta.Mensaje);
            else
            {
                dataGridView1.DataSource = Srta.servicios;
                this.servicios = (List<Servicio>)Srta.servicios;
            }


                  
            dataGridView2.DataSource = this.seleccion;
           
}

        private void PintarSeleccionados(List<Servicio> s)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if (this.Factura.Detalles.Count > 0)
            {
                var Fac = this.Factura;
                this.Factura = new Factura();
                MessageBox.Show(Fac.ToString());
                MessageBox.Show(this.Facturas.Guardar(Fac));
            }
            else
            {
                MessageBox.Show("Agrega algún servicio");
            }
    
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (Cliente == null)
            {
                MessageBox.Show("Seleccione primero el cliente");
                return;
            }

            try
            {
                Factura.PcjDescuento = Math.Abs(Double.Parse(textBox4.Text));
                textBox4.Text = Factura.PcjDescuento + "";
            }
            catch (Exception)
            {
                Factura.PcjDescuento = 0;
                MessageBox.Show("Rectifique el descuento");
                return;
            }

            try
            {
                Factura.PcjIva = Math.Abs(Double.Parse(textBox2.Text));
                textBox2.Text = Factura.PcjIva + "";
            }
            catch (Exception)
            {
                Factura.PcjIva = 0;
                MessageBox.Show("Rectifique el Iva");
                return;
            }

            try
            {
                Factura.PcjGanancia = Math.Abs(Double.Parse(textBox3.Text));
                textBox3.Text = Factura.PcjGanancia + "";
            }
            catch (Exception)
            {
                Factura.PcjGanancia = 0;
                MessageBox.Show("Rectifique la ganancia");
                return;
            }

            var row = dataGridView1.CurrentRow;
           var servicio = servicios[row.Cells[0].RowIndex];

            this.seleccion.Add(servicio);
            this.PintarSeleccionados(this.seleccion);

            var Mascota = this.Cliente.mascotas[comboBox1.SelectedIndex];
            this.Detalle = new DetalleFactura(servicio, Mascota);
            this.Detalles.Add(this.Detalle);

            this.Factura.Cliente = Cliente;
            this.Factura.Empleado = Empleado;
            Factura.Detalles = this.Detalles;            

            label12.Text = Factura.SubTotal + "";
            label13.Text = Factura.Total + "";

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                var Crta = this.Clientes.Buscar(textBox1.Text.Trim());
                if (!Crta.Error)
                {
                    this.ClienteCmabiado(Crta.cliente);
                }  

              
            }
        }

        private void ClienteCmabiado(Cliente c)
        {
            this.Cliente = c;
            label6.Text = c.NombreCompleto();
            var i = 0;
            comboBox1.Items.Clear();

            var range = new string[c.mascotas.Count];

            foreach (var item in c.mascotas)
            {
                range[i] = item.NombreMascota;
               
                i++;
            }
          
            this.comboBox1.Items.AddRange(range);

            if (comboBox1.Items.Count>0)
                comboBox1.SelectedIndex = 0;
           
        }

        private void textBox2_TextChanged(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (textBox2.Text == null) textBox2.Text = "0";
                if (textBox2.Text.Trim().Length < 1) textBox2.Text = "0";
                try
                {
                    Factura.PcjIva = Math.Abs(Double.Parse(textBox2.Text));
                    textBox2.Text = Factura.PcjIva + "";
                }
                catch (Exception)
                {
                    Factura.PcjIva = 0;
                    MessageBox.Show("Rectifique el descuento");
                }

                label12.Text = Factura.SubTotal + "";
                label13.Text = Factura.Total + "";
            }

        }

        private void textBox3_TextChanged(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (textBox3.Text == null) textBox3.Text = "0";
                if (textBox3.Text.Trim().Length < 1) textBox3.Text = "0";
                try
                {
                    Factura.PcjGanancia = Math.Abs(Double.Parse(textBox3.Text));
                    textBox3.Text = Factura.PcjGanancia + "";
                }
                catch (Exception)
                {
                    Factura.PcjGanancia = 0;
                    MessageBox.Show("Rectifique el Iva");
                }

                label12.Text = Factura.SubTotal + "";
                label13.Text = Factura.Total + "";
            }
        }

        private void textBox4_TextChanged(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (textBox4.Text == null) textBox4.Text = "0";
                if (textBox4.Text.Trim().Length < 1) textBox4.Text = "0";
                try
                {
                    Factura.PcjDescuento = Math.Abs(Double.Parse(textBox4.Text));
                    textBox4.Text = Factura.PcjDescuento + "";
                }
                catch (Exception)
                {
                    Factura.PcjDescuento = 0;
                    MessageBox.Show("Rectifique la ganancia");
                }

                label12.Text = Factura.SubTotal + "";
                label13.Text = Factura.Total + "";
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

     
    }
}
