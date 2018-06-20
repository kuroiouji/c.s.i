using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testmvc.Models;

namespace testmvc.Controllers
{
    // [Route("m")]
    public class MasterController : Controller
    {
        // [HttpGet]
        // [Route("m")]
        public IActionResult Index()
        {
            // return RedirectToAction("MovieSetting","Master");
            return View("../Master/MovieSetting");
        }

        // [HttpPost]
        // [Route("ms")]
        public IActionResult MovieSetting()
        {
            return View();
        }

        // [Route("t")]
        public IActionResult Theatre()
        {
            return View();
        }

        public class t1{
            public int idx{get;set;}
        }

        [HttpPost]
        // [Route("GetMovieSetting")]
        public IActionResult GetMovieSetting([FromBody] t1 t1)//[FromQuery]
        {
            return new ObjectResult(new {
                Name = "Movie "+t1.idx.ToString(),
                ReleaseDate = DateTime.Now
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}