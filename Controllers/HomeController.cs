using CricketPlayers.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;


namespace CricketPlayers.Controllers
{
    public class HomeController : Controller
    {
        private CricketDBEntities db = new CricketDBEntities();

        // GET: Home
        public ActionResult Index(string playerTeam, string searchString)
        {
            //team search
            var TeamList = new List<string>();
            var TeamQuery = from teamData in db.Players
                            orderby teamData.Current_Team
                            select teamData.Current_Team;

            TeamList.AddRange(TeamQuery.Distinct());
            ViewBag.playerTeam = new SelectList(TeamList);

            var players = from p in db.Players select p;

            //name search
            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(s => s.Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(playerTeam))
            {
                players = players.Where(x => x.Current_Team == playerTeam);
            }
            return View(players);
            /*
            var players = from p in db.Players select p;
            return View(players);
            */
        }
    

            public ActionResult Indexpartial(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Player player = db.Players.Find(id);
                if (player == null)
                {
                    return HttpNotFound();
                }
                return PartialView(player);
            }
        


        [HttpPost]
        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            int currentLikes = player.Likes;
            player.Likes = currentLikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
            }

            player = db.Players.Find(id);
            return PartialView("Indexpartial", player);
        }

        [HttpPost]
        public ActionResult Dislike(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            int currentDislikes = player.Dislikes;
            player.Dislikes = currentDislikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
            }

            player = db.Players.Find(id);
            return PartialView("Indexpartial", player);
        }

        public new ActionResult Profile(int id)
        {
            Player player = db.Players.Find(id);
            return View(player);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include =
            "id, Name, Description, Nationality, Current_Team, Matches_Played, Highest_Score, Likes, Dislikes, Picture, User")]
            Player player)
        {
            if (player.Picture == null)
            {
                player.Picture = "https://ae01.alicdn.com/kf/HTB1FohERFXXXXXBXpXXq6xXFXXX5/14-1-CM-12-5-CM-do-Jogador-de-Basquetebol-Silhueta-Etiqueta-Do-Carro-Vinil-S9.jpg_640x640.jpg";
            }
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        public ActionResult Edit(int? id)
        {
            Player player = db.Players.Find(id);
            return View(player);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include =
            "id, Name, Description, Nationality, Current_Team, Matches_Played, Highest_Score,Likes, Dislikes, Picture, User")]
            Player player)
        {
            if (player.Picture == null)
            {
                player.Picture = "https://ae01.alicdn.com/kf/HTB1FohERFXXXXXBXpXXq6xXFXXX5/14-1-CM-12-5-CM-do-Jogador-de-Basquetebol-Silhueta-Etiqueta-Do-Carro-Vinil-S9.jpg_640x640.jpg";
            }
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        public ActionResult Delete(int? id)
        {
            Player player = db.Players.Find(id);
            return View(player);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //find the player again
            Player player = db.Players.Find(id);

            //delete from the database
            db.Players.Remove(player);

            db.SaveChanges();

            //return to the index page
            return RedirectToAction("Index");
        }
    }
}