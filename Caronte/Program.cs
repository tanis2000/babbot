/*
 
 
  This program uses source code from PPather, slightly modified by barthen

    PPather and this modification are free software: you can redistribute them and/or modify
    them under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather and this modification are distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with PPather.  If not, see <http://www.gnu.org/licenses/>.

*/



using System;


namespace Caronte
{
    public class Caronte
    {
        private string continent;
        private WowTriangles.MPQTriangleSupplier mpq;
        private WowTriangles.ChunkedTriangleCollection triangleWorld;

        private Pather.Graph.PathGraph world;

        public bool Cancel
        {
            set { world.Cancel = value; }
        }

        public void Init(string iContinent)
        {
            continent = iContinent;

            mpq = new WowTriangles.MPQTriangleSupplier();
            mpq.SetContinent(continent);
            triangleWorld = new WowTriangles.ChunkedTriangleCollection(512);
            triangleWorld.SetMaxCached(9);
            triangleWorld.AddSupplier(mpq);



            /* After the init routines we create a PathGraph object (as before
               you can replace Kalimdor with whatever you need */

            world = new Pather.Graph.PathGraph(continent, triangleWorld, null);

        }

        public Pather.Graph.Path CalculatePath(Pather.Graph.Location iOrigin, 
                                                    Pather.Graph.Location iDestination)
        {
            return CalculatePath(iOrigin, iDestination, 5F);
        }

        public Pather.Graph.Path CalculatePath(Pather.Graph.Location iOrigin, 
                                    Pather.Graph.Location iDestination, float sDist)
        {
            // We try to find a path 

            Pather.Graph.Path mypath = world.CreatePath(iOrigin, iDestination, sDist);

            /* We save the PathGraph. Stores every chunk of the map we used in the 
               PPather\PathInfo\{map} folder */

            world.Save();

            return mypath;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {

            /* This init code is almost copy/pasted from PPather. Depending of where
            yo are, you can replace Kalimdor with one of:
            AhnQiraj
            AhnQirajTemple
            AlliancePVPBarracks
            AuchindounDemon
            AuchindounDraenei
            AuchindounEthereal
            AuchindounShadow
            Azeroth
            Blackfathom
            BlackrockDepths
            BlackRockSpire
            BlackTemple
            BlackwingLair
            bladesedgearena
            CavernsOfTime
            CoilfangDraenei
            CoilfangMarsh
            CoilfangPumping
            CoilfangRaid
            Collin
            CraigTest
            DalaranArena
            DeadminesInstance
            DeeprunTram
            development_nonweighted
            DireMaul
            EmeraldDream
            Expansion01
            GnomeragonInstance
            GruulsLair
            HellfireDemon
            HellfireMilitary
            HellfireRaid
            HellfireRampart
            HillsbradPast
            HordePVPBarracks
            HyjalPast
            Kalimdor
            Karazahn
            Mauradon
            MoltenCore
            Monastery
            MonasteryInstances
            NetherstormBG
            OnyxiaLairInstance
            OrgrimmarArena
            OrgrimmarInstance
            PVPLordaeron
            PVPZone01
            PVPZone02
            PVPZone03
            PVPZone04
            PVPZone05
            RazorfenDowns
            RazorfenKraulInstance
            SchoolofNecromancy
            ScottTest
            Shadowfang
            StormwindJail
            StormwindPrison
            Stratholme
            SunkenTemple
            Sunwell5Man
            Sunwell5ManFix
            SunwellPlateau
            TanarisInstance
            TempestKeepArcane
            TempestKeepAtrium
            TempestKeepFactory
            TempestKeepRaid
            test
            Uldaman
            WailingCaverns
            Zul'gurub
            ZulAman
            */

            WowTriangles.MPQTriangleSupplier mpq = new WowTriangles.MPQTriangleSupplier();
            mpq.SetContinent("Expansion01");
            WowTriangles.ChunkedTriangleCollection triangleWorld = new WowTriangles.ChunkedTriangleCollection(512);
            triangleWorld.SetMaxCached(9);
            triangleWorld.AddSupplier(mpq);



            /* After the init routines we create a PathGraph object (as before
               you can replace Kalimdor with whatever you need */

            Pather.Graph.PathGraph world = new Pather.Graph.PathGraph("Expansion01", triangleWorld, null);




            // Here we define the starting point (somewhere in the orc noob village)

            Pather.Graph.Location origin = new Pather.Graph.Location(240.94f, 2692.39f, 89.74f);



            // Here we define our destination (somewhere in Razor Hill)

            Pather.Graph.Location destination = new Pather.Graph.Location(189.63f, 2690.94f, 88.71f);

            // We try to find a path 

            Pather.Graph.Path mypath = world.CreatePath(origin, destination, 5f);

            /* We save the PathGraph. Stores every chunk of the map we used in the 
               PPather\PathInfo\{map} folder */

            world.Save();

            // Print the path 

            Console.WriteLine("Second attempt. Press a key");
            Console.ReadKey();

            mypath = world.CreatePath(origin, destination, 5f);

            Console.WriteLine("Third attempt. Press a key");
            Console.ReadKey();

            mypath = world.CreatePath(origin, destination, 5f);

            // Print the path 

            Console.WriteLine("Press a key to print the path");
            Console.ReadKey();

            foreach (Pather.Graph.Location loc in mypath.locations)
            {
                Console.WriteLine("X: {0}  Y: {1}   Z: {2}", loc.X, loc.Y, loc.Z);
            }

            Console.WriteLine("End of program. Press a key");
            Console.ReadKey();
        }
    }
}
