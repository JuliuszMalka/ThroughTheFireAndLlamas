using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace huehue_uczonko
{
    /// <summary>
    /// Class of generic Tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> where T : class
    {
        public class Node<T> where T : class
        {
            private int ID;
            private T Value;
            private bool IsAllocated;

            private List<Node<T>> Children = new List<Node<T>>();
            
            public Node()
            {
                this.Value = null;
                this.ID = 0;
                this.IsAllocated = true;
            }

            public Node(int ID)
            {
                this.Value = null;
                this.ID = ID;
                this.IsAllocated = false;
            }

            public Node(int ID, T Value)
            {
                this.Value = Value;
                this.ID = ID;
                this.IsAllocated = true;
            }

            public void Allocate(T Value)
            {
                this.Value = Value;
                this.IsAllocated = true;
            }

            public int GetID()
            {
                return this.ID;
            }

            public T GetValue(int iD)
            {
                return this.Value;
            }
            public bool CheckAlloc()
            {
                return IsAllocated;
            }

            public void AddChild(T Value, int ID)
            {
                Children.Add(new Node<T>(ID));
                Children[Children.Count - 1].Allocate(Value);
            }

            public void AddChild(Node<T> Child)
            {
                Children.Add(Child);
            }

            public void Clear()
            {
                foreach (Node<T> temp in Children)
                    temp.Clear();

                this.Children.Clear();
                this.Value = null;
                this.IsAllocated = false;
            }

            public Node<T> FindValue(T ToFind)
            {
                if (this.Value == ToFind)
                    return this;

                Node<T> ret = null;
                foreach (Node<T> temp in Children)
                    if ((ret = temp.FindValue(ToFind)) != null) return ret;

                return null;
            }

            public Node<T> FindID(int ToFind)
            {
                if (this.ID == ToFind)
                {
                 
                    
                    return this;
                }

                Node<T> ret = null;
                foreach (Node<T> temp in Children)
                    if ((ret = temp.FindID(ToFind)) != null) return ret;
                return null;
            }

            public bool Search(int SearchID)
            {
                if (this.IsAllocated && this.ID == SearchID)
                {
                    return true;
                }

                foreach (Node<T> temp in Children)
                    if (temp.Search(SearchID)) return true;
                return false;
            }
        }

        private Node<T> Root = null;

        public Tree()
        {
            this.Root = new Node<T>();
        }

        /// <summary>
        /// Finds Node with Value "ToFind"
        /// </summary>
        /// <param name="ToFind"></param>
        /// <returns>Node with searched Value</returns>
        public Node<T> FindValue(T ToFind)
        {
            Node<T> temp = this.Root.FindValue(ToFind);
            return temp;
        }

        /// <summary>
        /// Finds Node with ID "ToFind"
        /// </summary>
        /// <param name="ToFind">ID of element that method is looking for</param>
        /// <returns>Node with searched ID</returns>
        public Node<T> FindID(int ToFind)
        {
            Node<T> temp = this.Root.FindID(ToFind);
            return temp;
        }

        /// <summary>
        /// Clears Tree
        /// </summary>
        public void Clear()
        {
            this.Root.Clear();
        }

        /// <summary>
        /// Checks if Nodes wiht ID "FromID" and Node with ID "ToID" are connected through allocated nodes
        /// </summary>
        /// <param name="FromID"></param>
        /// <param name="ToID"></param>
        /// <returns></returns>
        public bool CheckAllocConnection(int FromID, int ToID)
        {
            if (this.Root != null) { return this.Root.Search(FromID) & this.Root.Search(ToID); }
            return false;
        }

        /// <summary>
        /// Adds new Node with Value "NewValue" and Node with ID "NewID" to Node with ID "ToID" as a child
        /// </summary>
        /// <param name="ToID"></param>
        /// <param name="NewValue"></param>
        /// <param name="NewID"></param>
        /// <returns></returns>
        public bool AddChildTo(int ToID, T NewValue, int NewID)
        {
            Node<T> temp = null;

            if ( (temp = Root.FindID(ToID)) != null ) { temp.AddChild(NewValue, NewID); return true; }

            return false;
        }

        /// <summary>
        /// AddChildTo Adds already existing Node "NewChild" as child to Node with ID "ToID"
        /// </summary>
        /// <param name="ToID"></param>
        /// <param name="NewChild"></param>
        /// <returns></returns>
        public bool AddChildTo(int ToID, Node<T> NewChild)
        {
            Node<T> temp = null;

            if ( (temp = Root.FindID(ToID)) != null ) { temp.AddChild(NewChild); return true; }
            return false;
        }
    }
}

//todo check if searching for node get into infinite loop, change if necessary good idea might be to add dictionary of already visited nodes