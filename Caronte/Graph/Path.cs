using System;
using System.Collections.Generic;
using System.Text;

namespace Pather.Graph
{
	public class Path
	{
		public List<Location> locations = new List<Location>();

		public Path() {}

		public Path(List<Spot> steps)
		{
			foreach (Spot s in steps)
			{
				AddLast(s.location);
			}
		}

        public Location this[int idx]
        {
            get
            {
                if ((idx < 0) || (idx >= locations.Count))
                    return null;
                else
                    return locations[idx];
            }
        }

		public int Count
		{
            get { return locations.Count; }
		}

		public Location GetFirst()
		{
			return Get(0);

		}
		public Location GetSecond()
		{
			if (locations.Count > 1)
				return Get(1);
			return null;
		}

		public Location GetRandom()
		{
			if (locations.Count < 2)
				return null;
			Random r = new Random();
			return locations[r.Next(0, (locations.Count - 1))];
		}

		public Location GetLast()
		{
			return locations[locations.Count - 1];
		}

		public Location RemoveFirst()
		{
			Location l = Get(0);
			locations.RemoveAt(0);
			return l;
		}

		public Location Get(int index)
		{
            //bug fix for if we request an index that does not exist
            if (locations.Count > index)
                return locations[index];
            else
                return null;
		}

		public void AddFirst(Location l)
		{
			locations.Insert(0, l);
		}

		public void AddFirst(Path l)
		{
			locations.InsertRange(0, l.locations);
		}

		public void AddLast(Location l)
		{
			locations.Add(l);
		}


		public void AddLast(Path l)
		{
			locations.AddRange(l.locations);
		}
	}
}
