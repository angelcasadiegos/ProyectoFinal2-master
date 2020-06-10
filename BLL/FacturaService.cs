﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entity;

namespace BLL
{
    public class FacturaService
    {
        private readonly ConnectionManager Conexion;
        private readonly FacturaRepository FacturaRepo;
        private readonly DetalleRepository DetalleRepo;
        private readonly ClienteRepository ClienteRepo;
        private readonly EmpleadoRepository EmpleadoRepo;
        private Response Response;



        public FacturaService(string _conection)
        {
            this.Conexion = new ConnectionManager(_conection);
            this.FacturaRepo = new FacturaRepository(this.Conexion);
            this.DetalleRepo = new DetalleRepository(this.Conexion);
            this.EmpleadoRepo = new EmpleadoRepository(this.Conexion);
            this.ClienteRepo = new ClienteRepository(this.Conexion);         
        }

        public string Guardar(Factura fact)
        {
     
            try
            {
                Conexion.Open();
                
                FacturaRepo.Guardar(fact);
                var id = FacturaRepo.Last();
                for (int i = 0; i < fact.Detalles.Count; i++)
                {
                   fact.Detalles[i].Factura = id;
                    DetalleRepo.Guardar(fact.Detalles[i]);
                }
                Conexion.Close();
                return $"Se facturó";
            }
            catch (Exception e)
            {
                Conexion.Close();
                return $"Error de la Aplicacion: {e.Message}";
            }

        }
        public Response Consultar()
        {
            Response = new Response();
            try
            {
                Conexion.Open();
                var facts = FacturaRepo.Consultar();
                for (int i = 0; i < facts.Count; i++)
                {
                    facts[i].Detalles = (List<DetalleFactura>)DetalleRepo.BuscarFac(facts[i].Codigo);
                }
                Response.facturas = facts;
                Conexion.Close();

                if (Response.facturas.Count > 0)
                {
                    Response.Mensaje = "Se consultan los Datos";
                }
                else
                {
                    Response.Mensaje = "No hay datos para consultar";
                }
                Response.Error = false;

                return Response;
            }
            catch (Exception e)
            {
                Response.Mensaje = $"Error de la Aplicacion: {e.Message}";
                Response.Error = true;

                Conexion.Close();
                return Response;
            }

        }

        public Response Buscar(int codigo)
        {
            Response = new Response();

            try
            {
                Conexion.Open();
                var Fac = FacturaRepo.Buscar(codigo);

                Fac.Detalles = (List<DetalleFactura>)DetalleRepo.BuscarFac(Fac.Codigo);
                Fac.Cliente = ClienteRepo.Buscar(Fac.Cliente.Identificacion);
                Fac.Empleado = EmpleadoRepo.Buscar(Fac.Empleado.Identificacion);

                Response.factura= FacturaRepo.Buscar(codigo);

                Conexion.Close();
                Response.Mensaje = (Response.factura!= null) ? "Se encontró la factura solicitada" : $"La factura {codigo} no existe";
                Response.Error = false;
                return Response;
            }
            catch (Exception e)
            {
                Response.Mensaje = $"Error de la Aplicacion: {e.Message}";
                Response.Error = true;
                Conexion.Close();
                return Response;
            }
        }


    }

        }

 