﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1_DianaTorres_JoseGalvis
{
    public class Automata
    {

        private string tipo; //va a ser : MEALY | MOORE
        private Hashtable estadosConRespuestas;
        private Hashtable transiciones;

        private List<Estado> estados;
        private List<String> estimulos;
        private List<String> respuestas;


        //getters & setters
        public Hashtable EstadosConRespuestas { get => estadosConRespuestas; set => estadosConRespuestas = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public Hashtable Transiciones { get => transiciones; set => transiciones = value; }
        public List<Estado> Estados { get => estados; set => estados = value; }
        public List<string> Estimulos { get => estimulos; set => estimulos = value; }
        public List<string> Respuestas { get => respuestas; set => respuestas = value; }


        //constructor
        public Automata(string tipo, List<Estado> estados, List<String> estimulos, List<string> rpta)
        {
            this.Tipo = tipo;

            transiciones = new Hashtable();
            this.Estados = estados;
            this.Estimulos = estimulos;
            this.Respuestas = rpta;

            //inicializarEstadosConRespuestas();
        }


        public void inicializarEstadosConRespuestas()
        {
            estadosConRespuestas = new Hashtable();
            switch (tipo)
            {
                case "MEALY":
                    foreach (Estado est in estados)
                    {
                        estadosConRespuestas.Add(est, "");
                    }
                    break;
                case "MOORE":

                    foreach (string res in respuestas)
                    {
                        estadosConRespuestas.Add(res, "");
                    }
                    break;
            }
        }





        //
        public void BFS()
        {
            //try
            //{
            reestablecerEstados();

            Estado estadoInicial = estados.First();
            estadoInicial.setEstaVisitado(true);
            Queue<string> cola = new Queue<string>();
            cola.Enqueue(estadoInicial.getValor());
            List<string> lista = new List<string>();

            while (cola.Count != 0)
            {
                string actual = cola.Dequeue();
                lista.Add(actual);
                List<string> aux = new List<string>();

                string t = transiciones[actual] + "";
                string[] transActuales = t.Split(',');
                for (int i = 0; i < transActuales.Length; i++)
                {
                    aux.Add(transActuales[i]);
                }

                for (int i = 0; i < aux.Count; i++)
                {
                    string sig = aux.ElementAt(i);

                    if (buscarEstado(sig).isEstaVisitado() == false)
                    {
                        buscarEstado(sig).setEstaVisitado(true);
                        cola.Enqueue(sig);
                    }


                }
            }


            List<Estado> estadosConexos = estados.Where(e => e.isEstaVisitado() == true).ToList();

            List<Estado> estadosMalos = estados.Where(e => e.isEstaVisitado() == false).ToList();

            actualizarHashtables(estadosMalos);
            estados = estadosConexos;



            //}
            //    catch (Exception ex)
            //    {
            //        throw new Exception("No se pudo encontrar el automata conexo equivalente. Intente de nuevo. ");
            //    }








        }



        public Estado buscarEstado(string valor)
        {



            return estados.Where(v => v.getValor().Equals(valor)).First();
        }






        //Este metodo reestablece el atributo de estaVisitado a false 
        //para todos los estados del automata
        public void reestablecerEstados()
        {
            for (int i = 0; i < estados.Count; i++)
            {
                estados.ElementAt(i).setEstaVisitado(false);
            }
        }

        public void actualizarHashtables(List<Estado> estadosBorrar)
        {
            foreach (Estado est in estadosBorrar)
            {
                transiciones.Remove(est.getValor());
                estadosConRespuestas.Remove(est.getValor());

            }
        }

        /// <summary>
        /// PARTICIONAMIENTO
        /// </summary>
        /// <returns></returns>
        public List<List<string>> particionamiento()
        {
            List<List<string>> particion = new List<List<string>>();
            particion = particion1();


            List<List<string>> particionAnterior = new List<List<string>>();


            
            while (particionAnterior.Count != particion.Count)
            {
                particionAnterior = particion;
                //recorro cada conjunto del particionamiento
                //i es el indice del conjunto actual
                for (int i = 0; i < particionAnterior.Count; i++)
                {
                   

                    particionUnConjunto(particion[i]);

                    particion = actualizarParticion(particion);
                }


            }



            return particion;
        }

        //Actualiza la particion en la lista de listas segun los indices de los conjuntos
        //donde pertenecen los estados
        public List<List<String>> actualizarParticion(List<List<String>> lista)
        {
            List<List<String>> particion = new List<List<string>>();


            for (int i = 0; i < estados.Count; i++)
            {
                List<string> conjunto = new List<string>();


                foreach (Estado e in estados)
                {
                    if (e.IndiceConjunto == i)
                    {
                        conjunto.Add(e.getValor());

                    }

                }
                if(conjunto.Count!=0) particion.Add(conjunto);
            }
            
            return particion;

        }

        //Particiona 1 conjunto especifico de estados segun sus transiciones
        private void particionUnConjunto(List<string> conjunto)
        {
            List<string> yaRevisado = new List<string>();
            Boolean next = false;

            //foreach (string eActual in conjunto)
            //{
                string eActual = conjunto.ElementAt(0);
                String[] tActual = transiciones[eActual].ToString().Split(',');
            yaRevisado.Add(eActual);
            foreach (string eSiguiente in conjunto)
                {

                    String[] tSiguiente = transiciones[eSiguiente].ToString().Split(',');
                    next = false;
                    for (int i = 0; i < estimulos.Count && next == false 
                        && !yaRevisado.Contains(eSiguiente); i++)
                    {
                        Estado estadoActual = buscarEstado(tActual[i]);
                        Estado estadoSiguiente = buscarEstado(tSiguiente[i]);

                        if (estadoActual.IndiceConjunto != estadoSiguiente.IndiceConjunto)
                        {
                            Estado cambiar = buscarEstado(eSiguiente);
                            cambiar.IndiceConjunto += 1;
                            next = true;
                        }
                    }

                }
               
            //}

        }


        //este metodo hace la particion 1
        //agrupa los estados segun sus respuestas
        //varía dependiendo del tipo de la maquina de estado
        public List<List<string>> particion1()
        {
            List<List<string>> particion = new List<List<string>>();

            Hashtable hashNueva = new Hashtable();
            for(int i = 0; i < respuestas.Count; i++)
            {

            }

            //List<Estado> auxEstados = new List<Estado>();
            //auxEstados = estados;
            //List<Estado> auxEstados2 = new List<Estado>();


            //foreach (Estado eActual in auxEstados)
            //{

            //    string resActual = estadosConRespuestas[eActual.getValor()].ToString();

            //    foreach (Estado eSiguiente in auxEstados)
            //    {
            //        if (eActual != eSiguiente && !auxEstados2.Contains(eSiguiente))
            //        {
            //            if (!resActual.Equals(estadosConRespuestas[eSiguiente.getValor()]))
            //            {

            //                eSiguiente.IndiceConjunto +=1;

            //            }
            //        }
            //    }

            //    auxEstados2.Add(eActual);

            //}

            //List<string> listita = new List<string>();
            //List<string> auxEstados3 = new List<string>();
            //auxEstados2 = new List<Estado>();

            //foreach (Estado estActual in estados)
            //{
            //    listita = new List<string>();
            //    foreach (Estado estSiguiente in estados)
            //    {

            //        if ( estActual.IndiceConjunto == estSiguiente.IndiceConjunto
            //            && !auxEstados2.Contains(estSiguiente))
            //        {
            //            listita.Add(estSiguiente.getValor());
            //            auxEstados2.Add(estSiguiente);
            //        }
            //    }
            //    if(listita.Count!=0) particion.Add(listita);

            //}





            return particion;
        }






    }
}
