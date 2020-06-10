﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entity;

namespace BLL
{
    public class MascotaService
    {
        private readonly ConnectionManager conexion;
        private readonly MascotaRepository mascotarepositorio;

        public MascotaService(string connectionString)
        { 
            conexion = new ConnectionManager(connectionString);
            mascotarepositorio = new MascotaRepository(conexion);
        }

        public string Guardar(Mascota mascota)
        {
            try
            {
                 conexion.Open();
                 mascotarepositorio.Guardar(mascota);
                 conexion.Close();
                return $"Se guardaron los datos de la mascota satisfactoriamente";
            }
            catch (Exception e)
            {
                return $"Cliente No Existe En Base De Datos:{ e.Message}";
            }
            finally { conexion.Close(); }
        }


        public ResponseConsultaMascota Consultar()
        {
            ResponseConsultaMascota respuesta = new ResponseConsultaMascota();
            try
            {

                conexion.Open();
                respuesta.mascotas= mascotarepositorio.ConsultarMascotas();
                conexion.Close();
                if (respuesta.mascotas.Count > 0)
                {
                    respuesta.Mensaje = "Se consultan los Datos";
                }
                else
                {
                    respuesta.Mensaje = "No hay datos para consultar";
                }
                respuesta.Error = false;
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.Mensaje = $"Error de la Aplicacion: {e.Message}";
                respuesta.Error = true;
                return respuesta;
            }
            finally { conexion.Close(); }

        }


        public int TotalizarPorTipo(string tipo)
        {
            conexion.Open();
            int Total = mascotarepositorio.TotalizarTipo(tipo);
            conexion.Close();
            return Total;
        }

        public int TotalizarMascotas()
        {
            conexion.Open();
            int Total = mascotarepositorio.Totalizar();
            conexion.Close();
            return Total;
        }

        public IList<Mascota> ConsultarPerros()
        {
            conexion.Open();
            IList<Mascota> mascotas = mascotarepositorio.ConsultarPerros();
            conexion.Close();
            return mascotas;
        }

        public IList<Mascota> ConsultarLoros()
        {
            conexion.Open();
            IList<Mascota> mascotas = mascotarepositorio.ConsultarLoros();
            conexion.Close();
            return mascotas;
        }

        public IList<Mascota> ConsultarGatos()
        {
            conexion.Open();
            IList<Mascota> mascotas = mascotarepositorio.ConsultarGatos();
            conexion.Close();
            return mascotas;
        }





    }
}
