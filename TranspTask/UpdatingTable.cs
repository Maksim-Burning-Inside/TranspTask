using System.Collections.Generic;
using System.Linq;

namespace TranspTask
{
    class UpdatingTable
    {
        private List<OptimalPoint> _contour_points;
        private Matrix _transport_tables;
        private int _N;

        public UpdatingTable(List<OptimalPoint> contour_points, Matrix transport_tables,int N)
        {
            _contour_points = contour_points;
            _transport_tables = transport_tables;
            _N = N;
        }

        public void UpdateTable()
        {
            double goods = _transport_tables.Table[_contour_points[1].Column][_contour_points[1].Row].Goods;
            for (int i = 0; i < _contour_points.Count; i++)
            {
                if (i % 2 == 0)
                {
                    _transport_tables.Table[_contour_points[i].Column][_contour_points[i].Row].AddGoods(goods);
                }
                else
                {
                    _transport_tables.Table[_contour_points[i].Column][_contour_points[i].Row].DeleteGoods(goods);
                }
            }

            for (int i = 0; i < _transport_tables.Table.Count; i++)
            {
                for (int j = 0; j < _transport_tables.Table[0].Count; j++)
                {
                    if (_transport_tables.Table[i][j].Occupation)
                    {
                        _N--;
                    }
                }
            }

            for (int i = 0; i < _N; i++)
            {
                OptimalPoint point = FindMin();
                _transport_tables.Table[point.Column][point.Row].AddGoods(0);
            }
        }

        private OptimalPoint FindMin()
        {
            List<List<MatrixElement>> table = DeepCopy(_transport_tables.Table);

            foreach (List<MatrixElement> item1 in table)
            {
                foreach (MatrixElement item2 in item1)
                {
                    if (item2.Occupation)
                    {
                        item2.Cost += item1.Max().Cost;
                    }
                }
            }

            List<MatrixElement> min_elements = new List<MatrixElement>();
            foreach (List<MatrixElement> item1 in table)
            {
                min_elements.Add(item1.Min());
            }

            int column = min_elements.IndexOf(min_elements.Min());
            int row = table[column].IndexOf(min_elements.Min());

            return new OptimalPoint(row, column);
        }

        private List<List<MatrixElement>> DeepCopy(List<List<MatrixElement>> matrixElements)
        {
            List<List<MatrixElement>> new_table = new List<List<MatrixElement>>();
            for (int i = 0; i < matrixElements.Count; i++)
            {
                new_table.Add(new List<MatrixElement>());
                for (int j = 0; j < matrixElements[0].Count; j++)
                {
                    new_table[i].Add(new MatrixElement(matrixElements[i][j].Cost, matrixElements[i][j].Occupation, matrixElements[i][j].Goods));
                }
            }

            return new_table;
        }
    }
}
