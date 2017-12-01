﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Mvc;
using CsvHelper;
using FlightFinder.Models;

namespace FlightFinder.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// IList containing flights for front-end to retrieve
        /// </summary>
        private static IList<dynamic> flights;

        /// <summary>
        /// IList containing airports for front-end to retrieve
        /// </summary>
        private static IList<dynamic> airports;


        /// <summary>
        /// Changes the way the flights are sorted
        /// 0 descending in price, 1 ascending in price, 2 ascending Depart time
        /// </summary>
        /// <param name="by">Int sent from front end to point to which view is wanted</param>
        public void SortFlights(int by)
        {
            
            if (by == 0)
            {
                 var result = from f in flights
                              orderby f.FirstClassPrice ascending
                              select f;
                flights = result.ToList();
            }
            else if(by == 1)
            {
                var result = from f in flights
                             orderby f.MainCabinPrice ascending
                             select f;
                flights = result.ToList();
            }
            else if(by == 2)
            {
                var result = from f in flights
                             orderby Convert.ToDateTime(f.Departs) ascending
                             select f;
                flights = result.ToList();
            }
            
            
        }
       

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]

        /// <summary>
        /// Makes JSON object of flights IList
        /// </summary>
        /// <returns></returns>
        public ActionResult Flights()
        {
           

            return Json(flights, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Makes JSON object of airports IList
        /// </summary>
        /// <returns></returns>
        public ActionResult Airports()
        {
            return Json(airports, JsonRequestBehavior.AllowGet);
        }

        // GET: Home
        public ActionResult Index()
        {
           airports = ReadFiles(@"airports.csv");
           flights = ReadFiles(@"flights.csv");
            SortFlights(0);
            return View();
        }

        /// <summary>
        /// Reads csv files with CSVHelper
        /// </summary>
        /// <param name="filename">Name of file(including .csv)</param>
        /// <returns>IList of csv entries</returns>
        public IList<dynamic> ReadFiles(string filename)
        {
            List<dynamic> info = new List<dynamic>();
            
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FlightFinder."+filename);
            var textReader = new StreamReader(stream);
            var csv = new CsvReader(textReader);
            while (csv.Read())
            {
                var records = csv.GetRecord<dynamic>();
                info.Add(records);
            }


            return info;
        }

    }

}