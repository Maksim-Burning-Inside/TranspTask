using System.Collections.Generic;
using System.Linq;

namespace TranspTask
{
    class OptimalityCoefficients
    {
        protected Matrix _transport_tables;
        protected List<Variable> _v = new List<Variable>();
        protected List<Variable> _u = new List<Variable>();

        protected List<Equal> _equals = new List<Equal>();

        public OptimalityCoefficients(Matrix transport_tables)
        {
            _transport_tables = transport_tables;
        }

        public void Find_VU()
        {
            PrepareStep();

            for (; CheckStop();)
            {
                foreach (Equal equal in _equals)
                {
                    equal.FindVariable();
                }
            }
        }

        private void PrepareStep()
        {
            for (int i = 0; i < _transport_tables.Table.Count; i++)
            {
                _v.Add(new Variable(i));
            }
            for (int i = 0; i < _transport_tables.Table[0].Count; i++)
            {
                _u.Add(new Variable(i));
            }

            for (int i = 0; i < _transport_tables.Table.Count; i++)
            {
                for (int j = 0; j < _transport_tables.Table[0].Count; j++)
                {
                    if (_transport_tables.Table[i][j].Occupation)
                    {
                        _equals.Add(new Equal(_u[j], _v[i], _transport_tables.Table[i][j].Cost));
                    }
                }
            }

            _u[0].AddValue(0);
        }

        private bool CheckStop()
        {
            foreach (Variable variable in _v)
            {
                if (variable.Certainty == false) return true;
            }
            foreach (Variable variable in _v)
            {
                if (variable.Certainty == false) return true;
            }
            return false;
        }
    }

    class Optimality : OptimalityCoefficients
    {
        private List<EqualOptimal> _equals_optimal = new List<EqualOptimal>();
        private bool _optimal = true;

        public bool Optimal { get => _optimal; }

        public Optimality(Matrix transport_tables) : base(transport_tables) { }

        public OptimalPoint Select_WorstOption()
        {
            PrepareStep();

            List<EqualOptimal> no_optimal = new List<EqualOptimal>();
            foreach (EqualOptimal equal in _equals_optimal)
            {
                equal.Check_OptimalityCondition();
                if (equal.Optimal == false)
                {
                    no_optimal.Add(equal);
                    _optimal = false;
                }
            }

            if (_optimal)
            {
                return new OptimalPoint();
            }

            EqualOptimal no_optimal_max = no_optimal.Max();
            OptimalPoint point = new OptimalPoint(no_optimal_max.U.Number, no_optimal_max.V.Number);

            return point;
        }

        private void PrepareStep()
        {
            Find_VU();

            for (int i = 0; i < _transport_tables.Table.Count; i++)
            {
                for (int j = 0; j < _transport_tables.Table[0].Count; j++)
                {
                    if (_transport_tables.Table[i][j].Occupation == false)
                    {
                        _equals_optimal.Add(new EqualOptimal(_u[j], _v[i], _transport_tables.Table[i][j].Cost));
                    }
                }
            }
        }
    }
}
