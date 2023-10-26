using System;
using System.Collections.Generic;

namespace TranspTask
{
    class TranslatorData
    {
        public static InformationHandler Create_InformationHandler(List<List<int>> _transport_tables, List<double> _list_of_stock, List<double> _list_of_demands)
        {
            List<List<MatrixElement>> matrixElements = new List<List<MatrixElement>>();
            for (int i = 0; i < _transport_tables.Count; i++)
            {
                matrixElements.Add(new List<MatrixElement>());
                for (int j = 0; j < _transport_tables[0].Count; j++)
                {
                    matrixElements[i].Add(new MatrixElement(_transport_tables[i][j]));
                }
            }

            return new InformationHandler(new Matrix(matrixElements), _list_of_stock, _list_of_demands);
        }
    }

    class InformationHandler
    {
        private Matrix _transport_tables;
        private List<double> _list_of_stock;
        private List<double> _list_of_demands;
        private bool _typefull = true;

        public InformationHandler(Matrix transport_table, List<double> list_of_stock, List<double> list_of_demands)
        {
            _transport_tables = transport_table;
            _list_of_stock = list_of_stock;
            _list_of_demands = list_of_demands;
            ConvertTusk_ToFull();
        }

        public List<double> Stocks => _list_of_stock;
        public List<double> Demands => _list_of_demands;
        public Matrix Matrix => _transport_tables;
        public bool TypeFull => _typefull;

        private void ConvertTusk_ToFull()
        {
            double sum_stock = 0, sum_demands = 0;
            for (int i = 0; i < _list_of_stock.Count; i++)
            {
                sum_stock += _list_of_stock[i];
            }
            for (int i = 0; i < _list_of_demands.Count; i++)
            {
                sum_demands += _list_of_demands[i];
            }

            if (sum_stock > sum_demands)
            {
                _list_of_demands.Add(sum_stock - sum_demands);
                _transport_tables.Add_ImagineColumn();
                _typefull = false;
            }
            else if (sum_stock < sum_demands)
            {
                _list_of_stock.Add(sum_demands - sum_stock);
                _transport_tables.Add_ImagineRow();
                _typefull = false;
            }
        }
    }

    class Matrix
    {
        private List<List<MatrixElement>> _transportation_cost_table;

        public Matrix(List<List<MatrixElement>> transportation_cost_table)
        {
            _transportation_cost_table = transportation_cost_table;
        }

        public List<List<MatrixElement>> Table { get => _transportation_cost_table; set => _transportation_cost_table = value; }

        public void Add_ImagineRow()
        {
            for (int i = 0; i < _transportation_cost_table.Count; i++)
            {
                _transportation_cost_table[i].Add(new MatrixElement(0));
            }
        }

        public void Add_ImagineColumn()
        {
            List<MatrixElement> new_column = new List<MatrixElement>();
            for (int i = 0; i < _transportation_cost_table[0].Count; i++)
            {
                new_column.Add(new MatrixElement(0));
            }
            _transportation_cost_table.Add(new_column);
        }
    }

    class MatrixElement : IComparer<MatrixElement>, IComparable<MatrixElement>
    {
        private int _cell_cost;
        private bool _occupation;
        private double _cell_goods;

        public MatrixElement(int cell_cost)
        {
            _cell_cost = cell_cost;
            _occupation = false;
        }

        public MatrixElement(int cell_cost, bool occupation, double cell_goods)
        {
            _cell_cost = cell_cost;
            _occupation = occupation;
            _cell_goods = cell_goods;
        }

        public bool Occupation { get => _occupation; set => _occupation = value; }
        public int Cost { get => _cell_cost; set => _cell_cost = value; }
        public double Goods { get => _cell_goods; set => _cell_goods = value; }

        public void AddGoods(double goods)
        {
            _cell_goods += goods;
            _occupation = true;
        }
        public void DeleteGoods(double goods)
        {
            _cell_goods -= goods;
            if (_cell_goods == 0) _occupation = false;
        }

        public int Compare(MatrixElement element1, MatrixElement element2)
        {
            if (element1.Cost - element2.Cost < 0)
                return -1;
            else if (element1.Cost - element2.Cost == 0)
                return 0;
            else
                return 1;
        }

        public int CompareTo(MatrixElement element)
         {
            if (this.Cost - element.Cost < 0)
                return -1;
            else if (this.Cost - element.Cost == 0)
                return 0;
            else
                return 1;
        }
    }
}
