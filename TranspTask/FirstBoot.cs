using System.Collections.Generic;

namespace TranspTask
{
    class FirstBoot
    {
        private Matrix _transport_tables;
        private List<double> _list_of_stock;
        private List<double> _list_of_demands;

        public static int N = 0;

        public FirstBoot(Matrix transport_tables, List<double> list_of_stock, List<double> list_of_demands)
        {
            _transport_tables = transport_tables;
            _list_of_stock = new List<double>(list_of_stock);
            _list_of_demands = new List<double>(list_of_demands);
        }

        public Matrix FillMatrix()
        {
            OptimalPoint point = new OptimalPoint(0, 0);

            for (; Check_ExitCondition();)
            {
                if (_list_of_stock[point.Row] > _list_of_demands[point.Column])
                {
                    _transport_tables.Table[point.Column][point.Row].AddGoods(_list_of_demands[point.Column]);
                    _list_of_stock[point.Row] -= _list_of_demands[point.Column];
                    _list_of_demands[point.Column] = 0;

                    N++;
                    point.Column++;
                }
                else
                {
                    _transport_tables.Table[point.Column][point.Row].AddGoods(_list_of_stock[point.Row]);
                    _list_of_demands[point.Column] -= _list_of_stock[point.Row];
                    _list_of_stock[point.Row] = 0;

                    N++;
                    point.Row++;
                }
            }

            return _transport_tables;
        }

        private bool Check_ExitCondition()
        {
            foreach (double demand in _list_of_demands)
            {
                if (demand > 0) return true;
            }
            foreach (double stock in _list_of_stock)
            {
                if (stock > 0) return true;
            }
            return false;
        }
    }
}
