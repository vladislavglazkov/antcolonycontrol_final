#pragma once
using System;
using System.Collections.Generic;
namespace AntAlgo {
    public class AntColonyControl
    { 
        public SortedSet<Ant> st= new SortedSet<Ant>();

        public void Move(double time)
        {
            while (st.Count > 0 && st.Min.GetTime() < time)
            {
                Ant cur = st.Min;
                Pair<double, Ant> next = null;
                
                st.Remove(cur);
                cur.MoveBy();
                Add(cur);
            }
        }
        public void Add(Ant aant)
        {
            Ant a = aant;
            a = aant;
            st.Add(a);

        }
    }
}