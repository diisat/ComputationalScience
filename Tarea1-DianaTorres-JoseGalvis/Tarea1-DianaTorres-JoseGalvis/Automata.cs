﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1_DianaTorres_JoseGalvis
{
    class Automata
    {
        
        private string tipo; //va a ser : MEALY | MOORE

        private List<Estado> estados;
        private List<Transicion> transiciones;

        private Automata automataMinimo;

        //getters & setters
        internal List<Estado> Estados { get => estados; set => estados = value; }
        internal List<Transicion> Transiciones { get => transiciones; set => transiciones = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public Automata AutomataMinimo { get => automataMinimo; set => automataMinimo = value; }


        //constructor
        public Automata(string tipo)
        {
            this.Tipo = tipo;
            estados = new List<Estado>();
            transiciones = new List<Transicion>();
            
        }


        //jose
        public Boolean EstadoMealy(string valor)
        {
            return false;
        }

        //jose
        public Boolean EstadoMoore(string valor, string respuesta)
        {
            return false;
        }

        //diana
        public Boolean agregarTransicion(string tipo, string estimulo, string respuesta, Estado llegada, Estado salida)
        {
            Boolean agregado = false;
            //switch (tipo)
            //{
            //    case "MEALY":
                    try
                    {
                        Transicion trans = new Transicion(estimulo, respuesta, llegada, salida);
                        transiciones.Add(trans);
                        agregado = true;
                    }
                    catch (Exception e)
                    {
                       
                    }
                   
                    //break;
                //case "MOORE":
                //    try
                //    {
                //        Transicion trans = new Transicion(estimulo, respuesta, llegada, salida);
                //        transiciones.Add(trans);
                //        agregado = true;
                //    }
                //    catch (Exception e)
                //    {

                //    }
                //    break;
            //}
            return agregado;
        }

        //dianayjose
        public Automata generarAutomataMinimo()
        {
            AutomataMinimo = null;
            return automataMinimo;
        }
    }
}