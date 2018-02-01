using System;
using System.Collections.Generic;

// Demonstrate a Priority Queue implemented with a Binary Heap

namespace Utilities
{

    public class PriorityQueue<T>
    {
        private List<QueueItem> data;

        public PriorityQueue()
        {
            this.data = new List<QueueItem>();
        }

        public void Enqueue(T item, double priority)
        {
            data.Add(new QueueItem(item, priority));
            int ci = data.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (data[ci].CompareTo(data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                QueueItem tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = data.Count - 1; // last index (before removal)
            QueueItem frontItem = data[0];   // fetch the front
            data[0] = data[li];
            data.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                QueueItem tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem.Item;
        }

        public T Peek()
        {
            return data[0].Item;
        }

        public double PeekAtPriority()
        {
            return data[0].Priority;
        }

        public int Count()
        {
            return data.Count;
        }

        public bool IsEmpty()
        {
            return data.Count == 0;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Count; ++i)
            s += data[i].ToString() + " ";
            s += "count = " + data.Count;
            return s;
        }

        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0) return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi) // each parent index
            {
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && data[pi].CompareTo(data[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].CompareTo(data[rci]) > 0) return false; // check the right child too.
            }
            return true; // passed all checks
        } // IsConsistent

        private class QueueItem : IComparable<QueueItem>
        {
            public double Priority;
            public T Item;

            public QueueItem(T item, double priority)
            {
                this.Priority = priority;
                this.Item = item;
            }

            public int CompareTo(QueueItem q)
            {
                return Math.Sign(this.Priority - q.Priority);
            }
        }
    } // PriorityQueue

} // ns
