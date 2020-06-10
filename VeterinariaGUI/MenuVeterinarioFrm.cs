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
using BLL;
using Entity;

namespace VeterinariaGUI
{
    public partial class MenuVeterinarioFrm : Form
    {
        EmpleadoService empleadoService;
        ResponseEmpleado respuestaConsulta = new ResponseEmpleado();
        private Empleado empleado;

        public MenuVeterinarioFrm()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionRochety"].ConnectionString;
            empleadoService = new EmpleadoService(connectionString);
            InitializeComponent();
        }

        public MenuVeterinarioFrm(Empleado empleado)
        {
            this.empleado = empleado;
        }

        private void AgregarVeterinarioBtn_Click(object sender, EventArgs e)
        {
            RegistrarVeterinarioFrm registrarVeterinarioFrm = new RegistrarVeterinarioFrm();
            registrarVeterinarioFrm.Show();
        }

        private void VeterinarioDtg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MinimizarBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BuscarVeterinarioBtn_Click(object sender, EventArgs e)
        {
            VeterinarioDtg.DataSource = null;
            respuestaConsulta = empleadoService.Consultar();

            consultar();

        }

        private void consultar()
        {
            if (TipoVeterinarioCmb.SelectedIndex== 0){
              VeterinarioDtg.DataSource = respuestaConsulta.empleados;

            }

            else if (TipoVeterinarioCmb.SelectedIndex == 1){
                DateTime fecha = DateFecha.Value.Date;
                VeterinarioDtg.DataSource = empleadoService.ConsultarXFecha(fecha);

            }
        }

        private void CANCELARBTN_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
