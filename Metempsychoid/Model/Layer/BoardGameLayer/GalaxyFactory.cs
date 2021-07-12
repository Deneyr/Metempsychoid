using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class GalaxyFactory
    {
        public static void CreateDefaultGalaxy(BoardGameLayer boardGameLayer)
        {
            float cosPi4 = (float)Math.Cos(Math.PI / 4);

            // Inner circle
            StarEntity star = new StarEntity(boardGameLayer);
            boardGameLayer.AddStar(star);


            StarEntity star1 = new StarEntity(boardGameLayer);
            star1.Position = new Vector2f(-400 * cosPi4, -400 * cosPi4);
            boardGameLayer.AddStar(star1);

            StarEntity star2 = new StarEntity(boardGameLayer);
            star2.Position = new Vector2f(0, -400);
            boardGameLayer.AddStar(star2);

            StarEntity star3 = new StarEntity(boardGameLayer);
            star3.Position = new Vector2f(400 * cosPi4, -400 * cosPi4);
            boardGameLayer.AddStar(star3);


            StarEntity star4 = new StarEntity(boardGameLayer);
            star4.Position = new Vector2f(400 * cosPi4, 400 * cosPi4);
            boardGameLayer.AddStar(star4);

            StarEntity star5 = new StarEntity(boardGameLayer);
            star5.Position = new Vector2f(0, 400);
            boardGameLayer.AddStar(star5);

            StarEntity star6 = new StarEntity(boardGameLayer);
            star6.Position = new Vector2f(-400 * cosPi4, 400 * cosPi4);
            boardGameLayer.AddStar(star6);

            boardGameLayer.AddStarLink(star, star2);
            boardGameLayer.AddStarLink(star, star5);
            boardGameLayer.AddCurvedStarLink(star1, star2, 400);
            boardGameLayer.AddCurvedStarLink(star2, star3, 400);
            boardGameLayer.AddCurvedStarLink(star4, star5, 400);
            boardGameLayer.AddCurvedStarLink(star5, star6, 400);

            // Circle
            StarEntity star7 = new StarEntity(boardGameLayer);
            star7.Position = new Vector2f(-800, 0);
            boardGameLayer.AddStar(star7);

            StarEntity star8 = new StarEntity(boardGameLayer);
            star8.Position = new Vector2f(0, -800);
            boardGameLayer.AddStar(star8);

            StarEntity star9 = new StarEntity(boardGameLayer);
            star9.Name = "in_star_right";
            star9.Position = new Vector2f(800, 0);
            boardGameLayer.AddStar(star9);

            StarEntity star10 = new StarEntity(boardGameLayer);
            star10.Position = new Vector2f(0, 800);
            boardGameLayer.AddStar(star10);

            boardGameLayer.AddStarLink(star, star7);
            boardGameLayer.AddStarLink(star, star9);
            boardGameLayer.AddStarLink(star1, star7);
            boardGameLayer.AddStarLink(star6, star7);
            boardGameLayer.AddStarLink(star3, star9);
            boardGameLayer.AddStarLink(star4, star9);
            boardGameLayer.AddStarLink(star1, star8);
            boardGameLayer.AddStarLink(star3, star8);
            boardGameLayer.AddStarLink(star4, star10);
            boardGameLayer.AddStarLink(star6, star10);
            boardGameLayer.AddCurvedStarLink(star7, star8, 800);
            boardGameLayer.AddCurvedStarLink(star8, star9, 800);
            boardGameLayer.AddCurvedStarLink(star9, star10, 800);
            boardGameLayer.AddCurvedStarLink(star10, star7, 800);

            // out circle
            StarEntity star11 = new StarEntity(boardGameLayer);
            star11.Name = "out_star_left";
            star11.Position = new Vector2f(-1200, 0);
            boardGameLayer.AddStar(star11);

            StarEntity star12 = new StarEntity(boardGameLayer);
            star12.Position = new Vector2f(0, -1200);
            boardGameLayer.AddStar(star12);

            StarEntity star13 = new StarEntity(boardGameLayer);
            star13.Name = "out_star_right";
            star13.Position = new Vector2f(1200, 0);
            boardGameLayer.AddStar(star13);

            StarEntity star14 = new StarEntity(boardGameLayer);
            star14.Name = "out_star_bot";
            star14.Position = new Vector2f(0, 1200);
            boardGameLayer.AddStar(star14);

            boardGameLayer.AddStarLink(star7, star11);
            boardGameLayer.AddStarLink(star9, star13);
            boardGameLayer.AddCurvedStarLink(star11, star12, 1200);
            boardGameLayer.AddCurvedStarLink(star12, star13, 1200);
            boardGameLayer.AddCurvedStarLink(star13, star14, 1200);
            boardGameLayer.AddCurvedStarLink(star14, star11, 1200);

            // Star Domains
            CJStarDomain domain1 = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                star1,
                star2,
                star3,
                star
            }, -1);
            boardGameLayer.AddStarDomain(domain1);

            CJStarDomain domain2 = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                star7,
                star8,
                star9,
                star10
            }, -2, false);
            boardGameLayer.AddStarDomain(domain2);
        }

        public static void CreateStandardGalaxy(BoardGameLayer boardGameLayer)
        {

            // First circle
            List<StarEntity> firstCircleStars = new List<StarEntity>();
            for (int i = 0; i < 5; i++)
            {
                double angle = -Math.PI / 2 + (2 - i) * 2 * Math.PI / 5;
                StarEntity star = new StarEntity(boardGameLayer);
                star.Position = new Vector2f((float)(400 * Math.Cos(angle)), (float)(-400 * Math.Sin(angle)));
                boardGameLayer.AddStar(star);
                firstCircleStars.Add(star);
            }

            boardGameLayer.AddCurvedStarLink(firstCircleStars[0], firstCircleStars[1], 400);
            boardGameLayer.AddCurvedStarLink(firstCircleStars[1], firstCircleStars[2], 400);
            boardGameLayer.AddCurvedStarLink(firstCircleStars[2], firstCircleStars[3], 400);
            boardGameLayer.AddCurvedStarLink(firstCircleStars[3], firstCircleStars[4], 400);
            boardGameLayer.AddCurvedStarLink(firstCircleStars[4], firstCircleStars[0], 400);

            // Second circle
            List<StarEntity> secondCircleStars = new List<StarEntity>();
            for (int i = 0; i < 5; i++)
            {
                double angle = Math.PI / 2 - i * 2 * Math.PI / 5;
                StarEntity star = new StarEntity(boardGameLayer);
                star.Position = new Vector2f((float)(800 * Math.Cos(angle)), (float)(-800 * Math.Sin(angle)));
                boardGameLayer.AddStar(star);
                secondCircleStars.Add(star);
            }

            boardGameLayer.AddCurvedStarLink(secondCircleStars[0], secondCircleStars[1], 800);
            boardGameLayer.AddCurvedStarLink(secondCircleStars[1], secondCircleStars[2], 800);
            boardGameLayer.AddCurvedStarLink(secondCircleStars[2], secondCircleStars[3], 800);
            boardGameLayer.AddCurvedStarLink(secondCircleStars[3], secondCircleStars[4], 800);
            boardGameLayer.AddCurvedStarLink(secondCircleStars[4], secondCircleStars[0], 800);

            boardGameLayer.AddStarLink(secondCircleStars[0], firstCircleStars[0]);
            boardGameLayer.AddStarLink(firstCircleStars[0], secondCircleStars[1]);
            boardGameLayer.AddStarLink(secondCircleStars[1], firstCircleStars[1]);
            boardGameLayer.AddStarLink(firstCircleStars[1], secondCircleStars[2]);
            boardGameLayer.AddStarLink(secondCircleStars[2], firstCircleStars[2]);
            boardGameLayer.AddStarLink(firstCircleStars[2], secondCircleStars[3]);
            boardGameLayer.AddStarLink(secondCircleStars[3], firstCircleStars[3]);
            boardGameLayer.AddStarLink(firstCircleStars[3], secondCircleStars[4]);
            boardGameLayer.AddStarLink(secondCircleStars[4], firstCircleStars[4]);
            boardGameLayer.AddStarLink(firstCircleStars[4], secondCircleStars[0]);

            // Third circle
            List<StarEntity> thirdCircleStars = new List<StarEntity>();
            for (int i = 0; i < 5; i++)
            {
                double angle = -Math.PI / 2 + (2 - i) * 2 * Math.PI / 5;
                StarEntity star = new StarEntity(boardGameLayer);
                star.Position = new Vector2f((float)(1100 * Math.Cos(angle)), (float)(-1100 * Math.Sin(angle)));
                boardGameLayer.AddStar(star);
                thirdCircleStars.Add(star);
            }

            boardGameLayer.AddStarLink(thirdCircleStars[0], secondCircleStars[0]);
            boardGameLayer.AddStarLink(secondCircleStars[0], thirdCircleStars[4]);
            boardGameLayer.AddStarLink(thirdCircleStars[4], secondCircleStars[4]);
            boardGameLayer.AddStarLink(secondCircleStars[4], thirdCircleStars[3]);
            boardGameLayer.AddStarLink(thirdCircleStars[3], secondCircleStars[3]);
            boardGameLayer.AddStarLink(secondCircleStars[3], thirdCircleStars[2]);
            boardGameLayer.AddStarLink(thirdCircleStars[2], secondCircleStars[2]);
            boardGameLayer.AddStarLink(secondCircleStars[2], thirdCircleStars[1]);
            boardGameLayer.AddStarLink(thirdCircleStars[1], secondCircleStars[1]);
            boardGameLayer.AddStarLink(secondCircleStars[1], thirdCircleStars[0]);

            // Fourth circle
            List<StarEntity> fourthCircleStars = new List<StarEntity>();
            for (int i = 0; i < 5; i++)
            {
                double angle = Math.PI / 2 - i * 2 * Math.PI / 5;
                StarEntity star = new StarEntity(boardGameLayer);
                star.Position = new Vector2f((float)(1600 * Math.Cos(angle)), (float)(-1600 * Math.Sin(angle)));
                boardGameLayer.AddStar(star);
                fourthCircleStars.Add(star);
            }

            boardGameLayer.AddStarLink(fourthCircleStars[0], thirdCircleStars[0]);
            boardGameLayer.AddStarLink(thirdCircleStars[0], fourthCircleStars[1]);
            boardGameLayer.AddStarLink(fourthCircleStars[1], thirdCircleStars[1]);
            boardGameLayer.AddStarLink(thirdCircleStars[1], fourthCircleStars[2]);
            boardGameLayer.AddStarLink(fourthCircleStars[2], thirdCircleStars[2]);
            boardGameLayer.AddStarLink(thirdCircleStars[2], fourthCircleStars[3]);
            boardGameLayer.AddStarLink(fourthCircleStars[3], thirdCircleStars[3]);
            boardGameLayer.AddStarLink(thirdCircleStars[3], fourthCircleStars[4]);
            boardGameLayer.AddStarLink(fourthCircleStars[4], thirdCircleStars[4]);
            boardGameLayer.AddStarLink(thirdCircleStars[4], fourthCircleStars[0]);

            // Star Domains
            CJStarDomain domain = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                firstCircleStars[0],
                firstCircleStars[1],
                firstCircleStars[2],
                firstCircleStars[3],
                firstCircleStars[4]
            }, 1, true);
            boardGameLayer.AddStarDomain(domain);

            domain = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                secondCircleStars[0],
                secondCircleStars[1],
                secondCircleStars[2],
                secondCircleStars[3],
                secondCircleStars[4]
            }, 0, false);
            boardGameLayer.AddStarDomain(domain);

            domain = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                fourthCircleStars[0],
                thirdCircleStars[0],
                fourthCircleStars[1],
                thirdCircleStars[1],
                fourthCircleStars[2],
                thirdCircleStars[2],
                fourthCircleStars[3],
                thirdCircleStars[3],
                fourthCircleStars[4],
                thirdCircleStars[4]
            }, -1, false);
            boardGameLayer.AddStarDomain(domain);

            // Moon
            Vector2f offset = new Vector2f(-1700, 500);
            double offsetAngle = -Math.PI / 8;
            double angle1 = Math.PI - 3 * Math.PI / 5 - offsetAngle;
            StarEntity star1 = new StarEntity(boardGameLayer);
            star1.Position = new Vector2f((float)(400 * Math.Cos(angle1)), (float)(-400 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star1);

            angle1 = Math.PI - offsetAngle;
            StarEntity star2 = new StarEntity(boardGameLayer);
            star2.Position = new Vector2f((float)(400 * Math.Cos(angle1)), (float)(-400 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star2);

            angle1 = Math.PI + 3 * Math.PI / 5 - offsetAngle;
            StarEntity star3 = new StarEntity(boardGameLayer);
            star3.Position = new Vector2f((float)(400 * Math.Cos(angle1)), (float)(-400 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star3);

            boardGameLayer.AddCurvedStarLink(star2, star1, 400);
            boardGameLayer.AddCurvedStarLink(star3, star2, 400);

            Vector2f offsetSecond = new Vector2f((float) Math.Cos(Math.PI + offsetAngle) * 200, (float) Math.Sin(Math.PI + offsetAngle) * 200);
            double radius = Math.Sqrt(200000);

            angle1 = Math.PI - 1.4 * Math.PI / 5 - offsetAngle;
            StarEntity star4 = new StarEntity(boardGameLayer);
            star4.Position = new Vector2f((float)(radius * Math.Cos(angle1)), (float)(-radius * Math.Sin(angle1))) + offset + offsetSecond;
            boardGameLayer.AddStar(star4);

            angle1 = Math.PI + 1.4 * Math.PI / 5 - offsetAngle;
            StarEntity star5 = new StarEntity(boardGameLayer);
            star5.Position = new Vector2f((float)(radius * Math.Cos(angle1)), (float)(-radius * Math.Sin(angle1))) + offset + offsetSecond;
            boardGameLayer.AddStar(star5);

            boardGameLayer.AddCurvedStarLink(star4, star1, (float) radius);
            boardGameLayer.AddCurvedStarLink(star5, star4, (float) radius);
            boardGameLayer.AddCurvedStarLink(star3, star5, (float)radius);

            boardGameLayer.AddStarLink(star1, fourthCircleStars[4]);
            boardGameLayer.AddStarLink(star2, thirdCircleStars[3]);
            boardGameLayer.AddStarLink(star3, fourthCircleStars[3]);

            // Star Domains
            domain = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                star1,
                star2,
                star3,
                star5,
                star4
            }, 0, true);
            boardGameLayer.AddStarDomain(domain);

            // Sun
            offset = new Vector2f(1200, -1600);
            offsetAngle = Math.PI / 3 + Math.PI / 6;
            angle1 = Math.PI / 2 - offsetAngle;
            star1 = new StarEntity(boardGameLayer);
            star1.Position = new Vector2f((float)(300 * Math.Cos(angle1)), (float)(-300 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star1);

            angle1 = 2 * Math.PI / 2 - offsetAngle;
            star2 = new StarEntity(boardGameLayer);
            star2.Position = new Vector2f((float)(300 * Math.Cos(angle1)), (float)(-300 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star2);

            angle1 = 3 * Math.PI / 2 - offsetAngle;
            star3 = new StarEntity(boardGameLayer);
            star3.Position = new Vector2f((float)(300 * Math.Cos(angle1)), (float)(-300 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star3);

            angle1 = -offsetAngle;
            star4 = new StarEntity(boardGameLayer);
            star4.Position = new Vector2f((float)(300 * Math.Cos(angle1)), (float)(-300 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star4);

            boardGameLayer.AddCurvedStarLink(star2, star1, 300);
            boardGameLayer.AddCurvedStarLink(star3, star2, 300);
            boardGameLayer.AddCurvedStarLink(star4, star3, 300);
            boardGameLayer.AddCurvedStarLink(star1, star4, 300);

            double offsetSecondAngle = 0; //Math.PI / 12;
            angle1 = Math.PI / 2 - offsetAngle + offsetSecondAngle;
            star5 = new StarEntity(boardGameLayer);
            star5.Position = new Vector2f((float)(600 * Math.Cos(angle1)), (float)(-600 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star5);

            angle1 = 2 * Math.PI / 2 - offsetAngle + offsetSecondAngle;
            StarEntity star6 = new StarEntity(boardGameLayer);
            star6.Position = new Vector2f((float)(600 * Math.Cos(angle1)), (float)(-600 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star6);

            angle1 = 3 * Math.PI / 2 - offsetAngle + offsetSecondAngle;
            StarEntity star7 = new StarEntity(boardGameLayer);
            star7.Position = new Vector2f((float)(600 * Math.Cos(angle1)), (float)(-600 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star7);

            angle1 = -offsetAngle + offsetSecondAngle;
            StarEntity star8 = new StarEntity(boardGameLayer);
            star8.Position = new Vector2f((float)(600 * Math.Cos(angle1)), (float)(-600 * Math.Sin(angle1))) + offset;
            boardGameLayer.AddStar(star8);

            boardGameLayer.AddCurvedStarLink(star7, star2, 400);
            boardGameLayer.AddCurvedStarLink(star7, star3, 300);
            boardGameLayer.AddCurvedStarLink(star8, star3, 400);
            boardGameLayer.AddCurvedStarLink(star8, star4, 300);
            boardGameLayer.AddCurvedStarLink(star5, star4, 400);
            boardGameLayer.AddCurvedStarLink(star5, star1, 300);
            boardGameLayer.AddCurvedStarLink(star6, star1, 400);
            boardGameLayer.AddCurvedStarLink(star6, star2, 300);

            boardGameLayer.AddStarLink(star7, fourthCircleStars[0]);
            boardGameLayer.AddStarLink(star8, fourthCircleStars[1]);

            // Star Domains
            domain = new CJStarDomain(boardGameLayer, new List<StarEntity>()
            {
                star1,
                star2,
                star3,
                star4
            }, 0, true);
            boardGameLayer.AddStarDomain(domain);
        }
    }
}
