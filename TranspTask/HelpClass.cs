using System;
using System.Collections.Generic;

namespace TranspTask
{
    struct OptimalPoint
    {
        public int Row;
        public int Column;

        public OptimalPoint(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    class Variable
    {
        private int _number;
        private int _value;
        private bool _certainty = false;

        public Variable(int number)
        {
            _number = number;
        }

        public void AddValue(int value)
        {
            _value = value;
            _certainty = true;
        }

        public int Number { get => _number; }
        public int Value { get => _value; }
        public bool Certainty { get => _certainty; }
    }

    class Equal
    {
        protected Variable _u;
        protected Variable _v;
        protected int _value;

        public Equal(Variable u, Variable v, int value)
        {
            _u = u;
            _v = v;
            _value = value;
        }

        public Variable U { get => _u; }
        public Variable V { get => _v; }

        protected bool CheckEquals()
        {
            if (_u.Certainty == false && _v.Certainty == false) return false;
            else if (_u.Certainty && _v.Certainty) return false;
            else return true;
        }

        public void FindVariable()
        {
            if (CheckEquals())
            {
                if (_u.Certainty)
                {
                    _v.AddValue(_value - _u.Value);
                }
                else
                {
                    _u.AddValue(_value - _v.Value);
                }
            }
        }
    }

    class EqualOptimal : Equal, IComparer<EqualOptimal>, IComparable<EqualOptimal>
    {
        private int _delta;
        private bool _optimal;

        public EqualOptimal(Variable u, Variable v, int value) : base(u, v, value) { }

        public int Delta { get => _delta; }
        public bool Optimal { get => _optimal; }

        public void Check_OptimalityCondition()
        {
            if (_u.Value + _v.Value <= _value)
            {
                _optimal = true;
                _delta = 0;
            }
            else
            {
                _optimal = false;
                _delta = Math.Abs(_value - _u.Value - _v.Value);
            }
        }

        public int Compare(EqualOptimal element1, EqualOptimal element2)
        {
            if (element1.Delta - element2.Delta < 0)
                return -1;
            else if (element1.Delta - element2.Delta == 0)
                return 0;
            else
                return 1;
        }

        public int CompareTo(EqualOptimal element)
        {
            if (this.Delta - element.Delta < 0)
                return -1;
            else if (this.Delta - element.Delta == 0)
                return 0;
            else
                return 1;
        }
    }
}
