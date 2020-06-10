﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Entity;
using System.Configuration;

namespace VeterinariaGUI
{
    public partial class MenuServiciosFrm : Form
    {
        ServiciosService servicioservice;
        RespuestaConsultaServicio respuestaservicio = new RespuestaConsultaServicio();
        string TotalServiciosGenerales;
       

        public MenuServiciosFrm()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionRochety"].ConnectionString;
            servicioservice = new ServiciosService(connectionString);

            InitializeComponent();
        }

       

        private void MinimizarBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AgregarServicioBtn_Click(object sender, EventArgs e)
        {
            RegistrarServicioFrm registrarServicioFrm = new RegistrarServicioFrm();
            registrarServicioFrm.Show();
        }

        private void BuscarServiciosBtn_Click(object sender, EventArgs e)
        {
            serviciosDtg.DataSource = null;
            respuestaservicio = servicioservice.Consultar();
            TotalServiciosGenerales = servicioservice.SumarServicios().ToString();
            consultar();

        }

        private void consultar()
        {
            if (Servicioscmb.SelectedIndex == 0)
            {
                serviciosDtg.DataSource = respuestaservicio.servicios;
                TotalServiciosGenerales = servicioservice.SumarServicios().ToString();
            }
            Llenar();
        }

        private void Llenar()
        {
            TotalServiciostxt.Text = TotalServiciosGenerales;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CANCELARBTN_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
